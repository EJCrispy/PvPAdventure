using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PvPAdventure.Core.Config;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Team = Terraria.Enums.Team;

namespace PvPAdventure.Common.World.Outlines.ProjectileOutlines;

[Autoload(Side = ModSide.Client)]
internal sealed class TeamProjectileOutlines : GlobalProjectile
{
    public override bool PreDraw(Projectile projectile, ref Color lightColor)
    {
        if (!ModContent.GetInstance<ClientConfig>().ProjectileOutlines || !TryGetTeam(projectile, out Team team))
            return true;

        Color border = Main.teamColor[(int)team];
        border.A = 255;

        if (!ModContent.GetInstance<ProjectileOutlineSystem>().TryGet(projectile, border, out RenderTarget2D target, out Vector2 origin))
            return true;

        Color drawColor = lightColor * projectile.Opacity;
        drawColor.A = (byte)(255f * projectile.Opacity);
        SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        Main.spriteBatch.Draw(target, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, drawColor, projectile.rotation, origin, projectile.scale, effects, 0f);
        return true;
    }

    private static bool TryGetTeam(Projectile projectile, out Team team)
    {
        team = Team.None;
        if (!projectile.active || projectile.type <= ProjectileID.None || projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
            return false;

        Player owner = Main.player[projectile.owner];
        team = owner?.active == true ? (Team)owner.team : Team.None;
        return team != Team.None;
    }
}

[Autoload(Side = ModSide.Client)]
internal sealed class ProjectileOutlineSystem : ModSystem
{
    private readonly Dictionary<Key, ProjectileOutlineRenderTarget> cache = [];

    public bool TryGet(Projectile projectile, Color border, out RenderTarget2D target, out Vector2 origin)
    {
        target = null;
        origin = Vector2.Zero;

        if (!ProjectileOutlineRenderTarget.TryGetFrame(projectile, out Rectangle frame))
            return false;

        int w = Math.Max(32, frame.Width + 32);
        int h = Math.Max(32, frame.Height + 32);
        Key key = new(projectile.type, frame.X, frame.Y, frame.Width, frame.Height, border.PackedValue);

        if (!cache.TryGetValue(key, out ProjectileOutlineRenderTarget rt))
        {
            rt = new ProjectileOutlineRenderTarget();
            cache[key] = rt;
            Main.ContentThatNeedsRenderTargets.Add(rt);
        }

        rt.UseProjectile(projectile.type, frame, w, h, border);
        target = rt.GetOutlineTarget();

        if (target == null)
            return false;

        origin = new(target.Width * 0.5f, target.Height * 0.5f);
        return true;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
            foreach (ProjectileOutlineRenderTarget rt in cache.Values)
                Main.ContentThatNeedsRenderTargets.Remove(rt);

        cache.Clear();
    }

    private readonly record struct Key(int Type, int FrameX, int FrameY, int FrameW, int FrameH, uint Color);
}

internal sealed class ProjectileOutlineRenderTarget : ARenderTargetContentByRequest
{
    private int type;
    private int width;
    private int height;
    private Rectangle frame;
    private Color border;
    private RenderTarget2D helperTarget;
    private EffectPass colorOnlyPass;

    public void UseProjectile(int type, Rectangle frame, int width, int height, Color border)
    {
        if (this.type == type && this.frame == frame && this.width == width && this.height == height && this.border.PackedValue == border.PackedValue)
        {
            if (!IsReady)
                Request();

            return;
        }

        this.type = type;
        this.frame = frame;
        this.width = width;
        this.height = height;
        this.border = border;
        Request();
    }

    public RenderTarget2D GetOutlineTarget() => _target;

    public static bool TryGetFrame(Projectile projectile, out Rectangle frame)
    {
        frame = default;
        if (projectile.type <= ProjectileID.None || projectile.type >= TextureAssets.Projectile.Length)
            return false;

        Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
        int frames = Math.Max(1, Main.projFrames[projectile.type]);
        int frameIndex = Math.Clamp(projectile.frame, 0, frames - 1);
        frame = texture.Frame(1, frames, 0, frameIndex);

        return frame.Width > 0 && frame.Height > 0;
    }

    protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        Effect shader = Main.pixelShader;
        colorOnlyPass ??= shader.CurrentTechnique.Passes["ColorOnly"];

        PrepareARenderTarget_AndListenToEvents(ref _target, device, width, height, RenderTargetUsage.PreserveContents);
        PrepareARenderTarget_WithoutListeningToEvents(ref helperTarget, device, width, height, RenderTargetUsage.DiscardContents);

        device.SetRenderTarget(helperTarget);
        device.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        colorOnlyPass.Apply();
        DrawMask(spriteBatch);
        shader.CurrentTechnique.Passes[0].Apply();
        spriteBatch.End();

        device.SetRenderTarget(_target);
        device.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        DrawOutline(spriteBatch);
        spriteBatch.End();

        device.SetRenderTarget(null);
        _wasPrepared = true;
    }

    private void DrawMask(SpriteBatch spriteBatch)
    {
        Texture2D texture = TextureAssets.Projectile[type].Value;
        spriteBatch.Draw(texture, new Vector2(width * 0.5f, height * 0.5f), frame, Color.White, 0f, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
    }

    private void DrawOutline(SpriteBatch spriteBatch)
    {
        if (helperTarget == null)
            return;

        DrawRing(spriteBatch, 4, Color.Black);
        DrawRing(spriteBatch, 2, border);
    }

    private void DrawRing(SpriteBatch spriteBatch, int distance, Color color)
    {
        const int step = 2;
        for (int x = -distance; x <= distance; x += step)
            for (int y = -distance; y <= distance; y += step)
                if (Math.Abs(x) + Math.Abs(y) == distance)
                    spriteBatch.Draw(helperTarget, new Vector2(x, y), color);
    }
}

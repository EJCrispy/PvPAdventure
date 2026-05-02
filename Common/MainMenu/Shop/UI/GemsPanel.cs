using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PvPAdventure.Common.MainMenu.Profile;
using PvPAdventure.Core.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;

namespace PvPAdventure.Common.MainMenu.Shop.UI;

public class GemsPanel : UIPanel
{
    private int gems;
    private bool hasProfile;

    public GemsPanel()
    {
        Height.Set(42f, 0f);
        BackgroundColor = new Color(63, 82, 151) * 0.7f;
        BorderColor = new Color(15, 15, 15);
        PaddingTop = 8f;
        PaddingLeft = 6f;
    }

    public void SetContent(int gems, bool hasProfile)
    {
        this.gems = gems;
        this.hasProfile = hasProfile;
    }

    public void RefreshFromProfile()
    {
        MainMenuProfileState profile = MainMenuProfileState.Instance;
        SetContent(profile.Gems, profile.HasSyncedFromBackend);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering)
        {
            string tooltip = hasProfile
                ? "Gems are awarded for achievements and high placement in official TPVPA matches."
                : "Could not load your TPVPA profile. Shop items can still be browsed.";

            Main.LocalPlayer.mouseInterface = true;
            UICommon.TooltipMouseText(tooltip);
        }
    }

    public override void Draw(SpriteBatch sb)
    {
        base.Draw(sb);

        CalculatedStyle inner = GetInnerDimensions();
        Vector2 pos = new(inner.X - 2f, inner.Y - 2f);
        sb.Draw(Ass.Icon_Gem.Value, pos, null, Color.White, 0f, Vector2.Zero, 1.3f, SpriteEffects.None, 0f);

        string gemsText = "";
        if (!hasProfile)
        {
            gemsText = "Error";
        }
        else
        {
            if (this.Width.Precent > 0)
            {
                gemsText = gems > 0 ? gems.ToString() + " Gems" : "No Gems";
            }
            else
            {
                gemsText = gems.ToString();
            }
        }

        float textAreaLeft = pos.X + 50f;
        float textAreaRight = inner.X + inner.Width - 10f;
        float textAreaWidth = System.Math.Max(0f, textAreaRight - textAreaLeft);

        Vector2 textSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, gemsText, Vector2.One);
        float textX = textAreaLeft + (textAreaWidth - textSize.X) * 0.5f;

        ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.MouseText.Value, gemsText, new Vector2(textX, pos.Y + 4f), Color.WhiteSmoke, 0f, Vector2.Zero, Vector2.One);
    }
}

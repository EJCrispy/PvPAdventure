using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PvPAdventure.Core.Compat;

public static class ErkySSCCompat
{
    public static void TrySendErkySSCSave()
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
            return;

        if (!ModLoader.TryGetMod("ErkySSC", out Mod erkySsc))
        {
            DebugLog.Warn("[PvPAdventure] Could not request ErkySSC save because ErkySSC is not loaded.");
            return;
        }

        Type saveSystemType = erkySsc.Code.GetType("ErkySSC.Common.SSC.SSCSaveSystem");

        if (saveSystemType == null)
        {
            DebugLog.Warn("[PvPAdventure] Could not find ErkySSC.Common.SSC.SSCSaveSystem.");
            return;
        }

        MethodInfo getInstanceMethod = typeof(ModContent).GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static)?.MakeGenericMethod(saveSystemType);
        object saveSystem = getInstanceMethod?.Invoke(null, []);

        if (saveSystem == null)
        {
            DebugLog.Warn("[PvPAdventure] Could not get SSCSaveSystem instance.");
            return;
        }

        MethodInfo sendMethod = saveSystemType.GetMethod("SendPacketToSavePlayerFile", BindingFlags.Instance | BindingFlags.Public);

        if (sendMethod == null)
        {
            DebugLog.Warn("[PvPAdventure] Could not find SSCSaveSystem.SendPacketToSavePlayerFile().");
            return;
        }

        sendMethod.Invoke(saveSystem, []);
    }
}

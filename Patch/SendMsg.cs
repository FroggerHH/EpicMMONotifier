using EpicMMOSystem;

namespace EpicMMONotifier.Patch;

[HarmonyPatch]
public static class SendMsg
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerFVX), nameof(PlayerFVX.levelUp))]
    private static void PlayerFVXLevelUpPatch()
    {
        GetPlugin<Plugin>().SendDiscordMessage(Player.m_localPlayer.GetPlayerName(), LevelSystem.Instance.getLevel());
    }
}
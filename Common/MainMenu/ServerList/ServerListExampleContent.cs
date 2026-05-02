namespace PvPAdventure.Common.MainMenu.ServerList;

internal static class ServerListExampleContent
{
    public static ServerListUIContent Create()
    {
        return new ServerListUIContent(
        [
            new ServerEntryContent("94.130.143.111", 5555, 0, 16, false),
            new ServerEntryContent("127.0.0.1", 5555, 0, 16, false),
            new ServerEntryContent("eu.tpvpa.net", 7777, 0, 16, false),
            new ServerEntryContent("dev.tpvpa.net", 5555, 0, 16, false)
        ]);
    }
}

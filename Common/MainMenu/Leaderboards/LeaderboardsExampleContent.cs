namespace PvPAdventure.Common.MainMenu.Leaderboards;

internal static class LeaderboardsExampleContent
{
    public static LeaderboardsUIContent Create()
    {
        return new LeaderboardsUIContent(
        [
            new LeaderboardEntryContent(1, "Matte", 412, 121, 58),
            new LeaderboardEntryContent(2, "EJ", 398, 160, 61),
            new LeaderboardEntryContent(3, "Erky", 355, 144, 49),
            new LeaderboardEntryContent(4, "Underscore", 301, 201, 63),
            new LeaderboardEntryContent(5, "Assorted", 280, 132, 40),
            new LeaderboardEntryContent(6, "Playering", 267, 173, 52),
            new LeaderboardEntryContent(7, "deval2004", 240, 118, 34),
            new LeaderboardEntryContent(8, "Rar", 221, 199, 57),
            new LeaderboardEntryContent(9, "Eco", 205, 142, 39),
        ]);
    }
}

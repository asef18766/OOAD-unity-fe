public enum GameMode
{
    Offline,
    Online,
    Server
}
public class GameChoice
{
    public static GameMode GameMode;
    public static string ServerName;
    public static string Winner;

    public static bool IsServer()
    {
        return GameMode == GameMode.Server;
    }
}
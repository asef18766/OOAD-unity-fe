namespace Map.Platforms
{
    public enum PlatformTypes
    {
        Direction,
        Fragile,
        Freeze,
        Normal,
        Spike,
        Time
    }
    public interface IPlatform
    {
        void SetSpeed(float speed);
    }
}
using Event;
namespace Map
{
    public class MapFactory
    {
        private static MapFactory instance= null;
        private MapFactory()
        {
            var eventManager = EventManager.GetInstance();
            eventManager.RegisterEvent("CreatePlatform" , CreatePlatform);
        }
        public static MapFactory GetInstance()
        {
            return instance ?? (instance = new MapFactory());
        }
        void CreatePlatform(string name , JSONObject obj)
        {
            //TODO: finishing creating platforms
            return;
        }
    }
}
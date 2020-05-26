using Map.Platforms;
using UnityEngine;

namespace Init.Methods
{
    public interface IObjectConstructor
    {
        Player PlayerConstructor(Vector3 pos , Vector2 scale , PlayerState iniState);
        IPlatform PlatformConstructor(Vector3 pos , Vector2 scale , PlatformTypes type);
    }
}
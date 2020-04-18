using System;
using Init.Methods;
using UnityEngine;
namespace Init
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    break;
                case RuntimePlatform.LinuxPlayer:
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    PCInit.BuildPc();
                    break;
                case RuntimePlatform.OSXPlayer:
                    break;
                default:
                    throw new ArgumentException($"does not support at platform {Application.platform}");
            }
        }
    }
}

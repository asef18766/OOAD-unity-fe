using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.LandscapeLeft;   
        #endif
    }
}

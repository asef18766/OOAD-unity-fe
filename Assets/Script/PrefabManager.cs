using System;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Prefab Manager",fileName = "PrefabManager")]
public class PrefabManager : ScriptableObject
{ 
    [SerializeField] private StringPrefabDictionary prefabs=null;
    [SerializeField] private PrefabManager selfInstance = null;
    private static PrefabManager _instance=null;
    public static PrefabManager GetInstance()
    {
        if(_instance == null)
            _instance = Resources.Load<PrefabManager>("ScriptableObject/PrefabManager");
        return _instance;
    }

    public GameObject GetGameObject(string objectName)
    {
        return prefabs.ContainsKey(objectName) ? prefabs[objectName] : null;
    }
}
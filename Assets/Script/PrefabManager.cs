using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Prefab Manager",fileName = "PrefabManager")]
public class PrefabManager : ScriptableObject
{ 
    [SerializeField] private StringPrefabDictionary prefabs=null;
    [SerializeField] private PrefabManager selfInstance = null;
    private static PrefabManager _instance=null;

    private void OnEnable()
    {
        _instance = selfInstance;
    }

    public static PrefabManager GetInstance()
    {
        return _instance;
    }

    public GameObject GetGameObject(string objectName)
    {
        return prefabs.ContainsKey(objectName) ? prefabs[objectName] : null;
    }
}
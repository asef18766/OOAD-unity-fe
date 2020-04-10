using UnityEditor;

// ReSharper disable once CheckNamespace
namespace Editor
{
    [CustomPropertyDrawer(typeof(StringPrefabDictionary))]
    public class DictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}
using UnityEngine;

namespace Utils
{
    public class Jsonify : MonoBehaviour
    {
        public static JSONObject VectortoJson(Vector3 v)
        {
            return new JSONObject($"[{v.x}, {v.y}, {v.z}]");
        }

        public static Vector3 JsontoVector(JSONObject obj)
        {
            return new Vector3(obj[0].n, obj[1].n, obj[2].n);
        }
    }
}
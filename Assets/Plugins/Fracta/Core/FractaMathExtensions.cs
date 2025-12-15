using UnityEngine;

namespace Fracta.Core
{
    public static class FractaMathExtensions
    {
        public static float Abs(this float a)
        {
            return Mathf.Abs(a);
        }

        public static int Abs(this int a)
        {
            return Mathf.Abs(a);
        }

        public static bool Approximately(this float a, float b, float precision = 0.001f)
        {
            if (Mathf.Abs(a - b) <= precision) return true;
            return false;
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Vector3Int RoundToInt(this Vector3 v)
        {
            var rounded = new Vector3Int();
            rounded.x = Mathf.RoundToInt(v.x);
            rounded.y = Mathf.RoundToInt(v.y);
            rounded.z = Mathf.RoundToInt(v.z);
            return rounded;
        }

        public static Vector3 LerpTowards(this Vector3 v, Vector3 to, float time)
        {
            var x = Mathf.Lerp(v.x, to.x, time);
            var y = Mathf.Lerp(v.y, to.y, time);
            var z = Mathf.Lerp(v.z, to.z, time);
            return new Vector3(x, y, z);
        }

        public static bool Approximately(this Vector3 a, Vector3 b, float precision = 0.001f)
        {
            return a.x.Approximately(b.x, precision) &&
                   a.y.Approximately(b.y, precision) &&
                   a.z.Approximately(b.z, precision);
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
        
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
    }
}
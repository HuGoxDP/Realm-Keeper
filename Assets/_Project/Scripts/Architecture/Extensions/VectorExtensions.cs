using UnityEngine;

namespace Architecture
{
    public static class VectorExtensions
    {
        public static Vector3 Center(this Vector3 a, Vector3 b) => (a + b) * 0.5f;
        public static Vector2 Center(this Vector2 a, Vector2 b) => (a + b) * 0.5f;
    }
}
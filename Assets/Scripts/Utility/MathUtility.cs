using UnityEngine;

namespace Utility
{
    public class MathUtility
    {
        /// <summary>
        /// Rotates given vector by angleDeg's.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angleDeg"></param>
        /// <returns></returns>
        public static Vector2 RotateVectorBy(Vector2 vector, float angleDeg)
        {
            float angleRad = angleDeg * Mathf.Deg2Rad;
            return new Vector2(vector.x*Mathf.Cos(angleRad)-vector.y*Mathf.Sin(angleRad), vector.x*Mathf.Sin(angleRad)+vector.y*Mathf.Cos(angleRad));
        }
        
        /// <summary>
        /// Returns x modulo m
        /// </summary>
        /// <param name="x"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static float Mod(float x, float m) {
            return (x%m + m)%m;
        }

        /// <summary>
        /// Returns x modulo m
        /// </summary>
        /// <param name="x"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int Mod(int x, int m)
        {
            return (x%m + m)%m;
        }
        
        /// <summary>
        /// Returns a unit vector angleDeg's from the x-axis.
        /// </summary>
        /// <param name="angleDeg"></param>
        /// <returns></returns>
        public static Vector3 NormDirFromAngle(float angleDeg)
        {
            return new Vector3(Mathf.Sin((angleDeg + 0f) * Mathf.Deg2Rad), Mathf.Cos((angleDeg + 0f) * Mathf.Deg2Rad), 0);
        }
        
        /// <summary>
        /// Returns a unit vector angleDeg's from the x-axis.
        /// </summary>
        /// <param name="angleDeg"></param>
        /// <returns></returns>
        public static Vector2 NormDirFromAngle2D(float angleDeg)
        {
            return new Vector2(Mathf.Sin((angleDeg + 0f) * Mathf.Deg2Rad), Mathf.Cos((angleDeg + 0f) * Mathf.Deg2Rad));
        }
    }
}
using System;
using UnityEngine;

namespace Actions
{
    class VectorAngle
    {
        

        public static double AngleBetween(Vector3 u, Vector3 v)
        {
            double dot = u[0]*v[0] + u[1]*v[1] + u[2]*v[2];
            double norm_u = Math.Sqrt(u[0]*u[0] + u[1]*u[1] + u[2]*u[2]);
            double norm_v = Math.Sqrt(v[0]*v[0] + v[1]*v[1] + v[2]*v[2]);
            double angle = Math.Acos(dot / (norm_u * norm_v));
            return angle;
        }
    }
    

    public class AvoidObstacle : MonoBehaviour
    {
        public Transform obstacle; // l'objet à éviter
        public float speed = 5.0f; // la vitesse de déplacement de l'objet

        void Update()
        {
            
        }
    }
}
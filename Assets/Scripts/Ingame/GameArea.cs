using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zchfvy.Plus;

namespace MiniPlanetDefense
{
    public class GameArea : MonoBehaviour
    {


        public float Radius;
        public Vector3 Center;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            GizmosPlus.Circle(Center, Radius*Vector3.back);
            GizmosPlus.Text(Center + Vector3.up*Radius, "Game Area");
        }

        void OnValidate()
        {
            transform.localScale = new Vector3(Radius*2, Radius*2, 0.1f);
            transform.position = Center;
            InitGameArea(transform.position);
        }

        void Awake()
        {
            InitGameArea(transform.position);
        }

        public void InitGameArea(Vector3 c)
        {
            Vector3 pos = new Vector3(c.x, c.y, 5.0f);
            Center = pos;
            transform.position = pos;
            Radius = transform.localScale.x / 2f;    
        
        }
        
    }
}
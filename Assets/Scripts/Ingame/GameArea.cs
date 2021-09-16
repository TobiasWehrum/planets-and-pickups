using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class GameArea : MonoBehaviour
    {
        

        public float Radius { get; private set; }
        public Vector3 Center { get; private set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(Center, Radius);
        }

        void Awake()
        {
            InitGameArea(Vector2.zero);
        }

        public void InitGameArea(Vector3 c)
        {
            Vector3 pos = new Vector3(c.x, c.y, 5.0f);
            Center = pos;
            if (gameObject != null)
            {
                transform.position = pos;
                Radius = transform.localScale.x / 2f;    
            }
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonTester : MonoBehaviour
{
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public bool circularRegion = false;

    List<Vector2> points;

    void OnValidate() {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples, circularRegion);
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0,255,0, 0.05f);
        Gizmos.DrawCube(regionSize/2,regionSize);
        if (points != null) {
            foreach (Vector2 point in points) {
                Gizmos.color = new Color(220, 220,220, 0.1f);
                
                Gizmos.DrawSphere(point, radius/2.0f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}

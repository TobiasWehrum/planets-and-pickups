using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaribleRadiiTester : MonoBehaviour
{
    public bool minimumLimit = false;
    public int seed;
    public float maxRadius = 1f,minRadius = 0.5f;
    public float regionSize = 10f;
    public int iterationsPerCell = 10;
    
    Color LightGray =  new Color(220, 220,220); 

    private List<VariableRadiiDiskSampler.Point> points;

    void OnValidate()
    {
        if (minimumLimit)
            points = VariableRadiiDiskSampler.GeneratePoints(seed, maxRadius, minRadius, regionSize, iterationsPerCell);
        else
            points = VariableRadiiDiskSampler.GeneratePoints(seed, maxRadius, regionSize, iterationsPerCell);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(regionSize/2,regionSize/2,0.5f),new Vector3(regionSize,regionSize,1f));
        if (points != null)
        {
            foreach (VariableRadiiDiskSampler.Point point in points)
            {
                
                Color c = Color.Lerp(Color.gray, LightGray, (point.radius-minRadius)/(maxRadius-minRadius));
                c.a = 0.1f;
                Gizmos.color = c;
                Gizmos.DrawSphere(new Vector3(point.x,point.y,0.5f), point.radius);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(point.x,point.y,0.5f), 0.1f);
            }
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            new Vector3(regionSize/2,regionSize/2,0.5f),
            regionSize/2
        );
        
        
    }
}

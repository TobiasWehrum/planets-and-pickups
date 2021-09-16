/*
 * Fast Poisson Disk Sampling in Arbitrary Dimensions
 * https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class PoissonDiscSampling
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="radius"></param> Radius of each disk
    /// <param name="sampleRegionSize"></param> Bounds for grid
    /// <param name="numSamplesBeforeRejection"></param> Num samples before rejecting gridpt (already has too many pts next to this pt)
    /// <returns></returns>
    public static List<Vector2> GeneratePoints(
        float radius,
        Vector2 sampleRegionSize,
        int numSamplesBeforeRejection = 30,
        bool circularRegion = false
    )
    {
        // cell size bounded by r/√2 so each grid cell will contain at most 1 sample,
        // thus the grid can be implemented as a simple 2D array of integers:
        //  -- the default −1 indicates no sample,
        //  -- a non-negative integer gives the index of the sample located in a cell.
        float cellSize = radius / Mathf.Sqrt(2);

        // Step 0 :
        // Initialize an 2d background grid for storing
        // samples and accelerating spatial searches
        int[,] grid = new int[
            Mathf.CeilToInt(sampleRegionSize.x / cellSize),
            Mathf.CeilToInt(sampleRegionSize.y / cellSize)
        ];
        List<Vector2> points = new List<Vector2>(); // point in grid
        List<Vector2> spawnPoints = new List<Vector2>(); // 


        // Step 1:
        // Select the initial sample, x0, randomly chosen uniformly
        // from the domain. Insert it into the background grid, and initialize
        // the “active list” (an array of sample indices) with this index (zero).
        spawnPoints.Add(sampleRegionSize / 2);


        while (spawnPoints.Count > 0) // keep iterating over spawnPts
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            // Checks if spn pt 
            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                // Get a random vector from current to new candidate pt 
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);


                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid, circularRegion))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int) (candidate.x / cellSize), (int) (candidate.y / cellSize)] =
                        points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    /// <summary>
    /// Check if candidate is a valid pt
    /// </summary>
    /// <param name="candidate"></param>
    /// <param name="sampleRegionSize"></param>
    /// <param name="cellSize"></param>
    /// <param name="radius"></param>
    /// <param name="points"></param>
    /// <param name="grid"></param>
    /// <param name="circularRegion"></param>
    /// <returns></returns>
    static bool IsValid(
        Vector2 candidate,
        Vector2 sampleRegionSize,
        float cellSize,
        float radius,
        List<Vector2> points,
        int[,] grid,
        bool circularRegion
    )
    {

        if (circularRegion &&
            !isInsideCircle(sampleRegionSize, sampleRegionSize.x / 2, candidate))
            return false;
        
        
        if (
            candidate.x >= 0
            && candidate.x < sampleRegionSize.x
            && candidate.y >= 0 
            && candidate.y < sampleRegionSize.y
        )
        {
            int cellX = (int) (candidate.x / cellSize);
            int cellY = (int) (candidate.y / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }


    static bool isInsideCircle(
        Vector2 sampleRegion,
        float sampleRadius,
        Vector2 point)
    {
        float circle_x = sampleRegion.x / 2;
        float circle_y = sampleRegion.y / 2;

        // Compare radius of circle with distance
        // of its center from given point
        if (
            Mathf.Pow((point.x - circle_x), 2) + Mathf.Pow((point.y - circle_y), 2)
            <= Mathf.Pow(sampleRadius, 2)
        )
            return true;

        return false;
    }
}
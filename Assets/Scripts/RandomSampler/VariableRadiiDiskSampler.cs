using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class VariableRadiiDiskSampler : MonoBehaviour
{
    public class Point
    {
        public float x, y, radius;

        public Point(float x, float y, float radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }
    }

    public class Cell
    {
        public int x, y;
        public List<Point> points;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            points = new List<Point>();
        }
    }

    public static List<Point> GeneratePoints(int seed, float maxRadius, float minRadius,
        float sampleRegionSize, int iterationsPerCell)
    {
        Random.InitState(seed);
        List<Cell> cells = new List<Cell>();
        int gridSize = Mathf.FloorToInt(sampleRegionSize / maxRadius);
        Cell tempCell = null;
        Point tempPoint = null;
        float gridX, gridY, d, r;
        bool invalid = true;
        for (int i = 0; i <= gridSize; i++)
        for (int j = 0; j <= gridSize; j++)
        {
            tempCell = new Cell(i, j);
            cells.Add(tempCell);
            gridX = i * maxRadius;
            gridY = j * maxRadius;
            for (int k = 0; k < iterationsPerCell; k++)
            {
                if (invalid)
                {
                    tempPoint = new Point(gridX + Random.Range(0f, maxRadius),
                        gridY + Random.Range(0f, maxRadius),
                        Random.Range(minRadius, maxRadius));
                    invalid = false;
                }

                if (tempPoint.x > sampleRegionSize || tempPoint.y > sampleRegionSize)
                {
                    invalid = true;
                    continue;
                }
                
                if (!isInsideCircle(
                        sampleRegionSize,
                        sampleRegionSize / 2.0f,
                        new Vector2(tempPoint.x, tempPoint.y)
                    )
                )
                {
                    invalid = true;
                    continue;
                }

                for (int l = 0; l < cells.Count; l++)
                {
                    if ((cells[l].x + 2 >= i) && (cells[l].x - 2 <= i) &&
                        (cells[l].y + 2 >= j))
                    {
                        for (int m = 0; m < cells[l].points.Count; m++)
                        {
                            d = Mathf.Sqrt(
                                ((cells[l].points[m].x - tempPoint.x) *
                                 (cells[l].points[m].x - tempPoint.x)) +
                                ((cells[l].points[m].y - tempPoint.y) *
                                 (cells[l].points[m].y - tempPoint.y)));
                            r = cells[l].points[m].radius + tempPoint.radius;
                            if (d > r)
                                continue;
                            tempPoint.radius = tempPoint.radius + d - r;
                            if (tempPoint.radius < minRadius)
                            {
                                invalid = true;
                                break;
                            }
                        }

                        if (invalid)
                            break;
                    }
                }

                if (!invalid)
                    tempCell.points.Add(tempPoint);
                invalid = true;
            }
        }

        return cells.SelectMany(R => R.points).ToList();
    }

    public static List<Point> GeneratePoints(int seed, float radius,
        float sampleRegionSize, int iterationsPerCell)
    {
        Random.InitState(seed);
        List<Cell> cells = new List<Cell>();
        int gridSize = Mathf.FloorToInt(sampleRegionSize / radius);
        Cell tempCell = null;
        Point tempPoint = null;
        float gridX, gridY, d, r;
        bool invalid = true;
        for (int i = 0; i <= gridSize; i++)
        for (int j = 0; j <= gridSize; j++)
        {
            tempCell = new Cell(i, j);
            cells.Add(tempCell);
            gridX = i * radius;
            gridY = j * radius;
            for (int k = 0; k < iterationsPerCell; k++)
            {
                if (invalid)
                {
                    tempPoint = new Point(gridX + Random.Range(0f, radius),
                        gridY + Random.Range(0f, radius), radius);
                    invalid = false;
                }

                if (tempPoint.x > sampleRegionSize || tempPoint.y > sampleRegionSize)
                {
                    invalid = true;
                    continue;
                }

                if (!isInsideCircle(
                        sampleRegionSize,
                        sampleRegionSize / 2.0f,
                        new Vector2(tempPoint.x, tempPoint.y)
                    )
                )
                {
                    invalid = true;
                    continue;
                }

                for (int l = 0; l < cells.Count; l++)
                {
                    if ((cells[l].x + 2 >= i) && (cells[l].x - 2 <= i) &&
                        (cells[l].y + 2 >= j))
                    {
                        for (int m = 0; m < cells[l].points.Count; m++)
                        {
                            d = Mathf.Sqrt(
                                ((cells[l].points[m].x - tempPoint.x) *
                                 (cells[l].points[m].x - tempPoint.x)) +
                                ((cells[l].points[m].y - tempPoint.y) *
                                 (cells[l].points[m].y - tempPoint.y)));
                            r = cells[l].points[m].radius + tempPoint.radius;
                            if (d < r)
                                invalid = true;
                        }

                        if (invalid)
                            break;
                    }
                }

                if (!invalid)
                    tempCell.points.Add(tempPoint);
                invalid = true;
            }
        }

        return cells.SelectMany(R => R.points).ToList();
    }


    static bool isInsideCircle(
        float sampleRegion,
        float sampleRadius,
        Vector2 point)
    {
        float circle_x = sampleRegion / 2.0f;
        float circle_y = sampleRegion / 2.0f;

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
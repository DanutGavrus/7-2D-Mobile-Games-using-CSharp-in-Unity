using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Line : MonoBehaviour {
    /* Controls the drawing and characteristics of the new line. */
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCol;
    public float minDistanceBetwPoints = 0.1f; // Smaller value makes the line smoother, but the game slower

    private readonly List<Vector2> pointsPositions = new List<Vector2>();

    public void UpdateLine(Vector2 touchPosition)
    {
        // If no point yet, insert one
        if (pointsPositions.Count == 0)
        {
            SetPoint(touchPosition);
            return;
        }
        // Check if the finger has moved enough for us to insert another point
        if (Vector2.Distance(touchPosition, pointsPositions.Last()) > minDistanceBetwPoints)
        {
            SetPoint(touchPosition);
        }
    }

    public void SetPoint(Vector2 pointPosition)
    {
        pointsPositions.Add(pointPosition);
        lineRenderer.positionCount = pointsPositions.Count;
        lineRenderer.SetPosition(pointsPositions.Count - 1, pointPosition);
        if (pointsPositions.Count >= 2) // The edge collider needs to have at least 2 points
        {
            edgeCol.points = pointsPositions.ToArray();
        }
    }
}

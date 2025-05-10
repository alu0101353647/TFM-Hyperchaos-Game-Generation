using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawInUnity : MonoBehaviour
{
    [SerializeField] GameObject CylinderPrefab;
    public void Draw(List<Vector3> positions)
    {
        // Made up take
        for (int i = 0; i < positions.Count-1; ++i)
        {
            // Place in the middle of two points
            GameObject betweenLine = Instantiate(CylinderPrefab, (positions[i + 1] - positions[i]) / 2 + positions[i], transform.rotation, this.transform);
            betweenLine.name = "Line";
            // Rotate so the ends point toward the points
            betweenLine.transform.LookAt(positions[i+1]);
            betweenLine.transform.Rotate(new(1, 0, 0), 90);
            // Stretch so it connects the points
            float distance = Vector3.Distance(positions[i + 1], positions[i]) / 2;
            betweenLine.transform.localScale = new(betweenLine.transform.localScale.x, distance / betweenLine.transform.localScale.y, betweenLine.transform.localScale.z);
        }
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Instrument to build path.
/// V0.1.0
/// </summary>
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class PathBuilder : MonoBehaviour
{
    /// <summary>
    /// Path points
    /// </summary>
    public List<Vector3> PathPoints => pathPoints;

    [SerializeField, Header("Point disc radius")]
    float pointRadius = 2f;

    [SerializeField]
    private List<Vector3> pathPoints = new List<Vector3>();

    [SerializeField]
    private List<Vector3> normalPoints = new List<Vector3>();

    [SerializeField]
    private int currentPoint = 0;
#if UNITY_EDITOR
    public void DuringScene(SceneView view)
    {
        if(Selection.count != 1)
        {
            return;
        }

        if (Event.current.type == EventType.MouseMove)
        {
            view.Repaint();
        }

        bool isHoldShift = (Event.current.modifiers & EventModifiers.Shift) != 0;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Handles.DrawWireDisc(hit.point, hit.normal, pointRadius);
            Handles.color = Color.green;
            Handles.DrawWireDisc(hit.point, hit.normal, pointRadius / 5, 2f);
  
            if(Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.V && currentPoint != pathPoints.Count - 1 && pathPoints.Count != 0 && isHoldShift)
            {
                pathPoints[currentPoint] = hit.point;
                normalPoints[currentPoint] = hit.normal;
            }
            else if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.V & isHoldShift)
            {
                currentPoint = pathPoints.Count;
                pathPoints.Add(hit.point);
                normalPoints.Add(hit.normal);
            }
        }

        if(Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Q && isHoldShift)
        {
            currentPoint = currentPoint - 1 < 0 ? 0 : currentPoint - 1;
        }
        if(Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.E && isHoldShift)
        {
            currentPoint = currentPoint + 1 > pathPoints.Count - 1 ? pathPoints.Count - 1 : currentPoint + 1;
        }
        if(Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.R && isHoldShift)
        {
            currentPoint = pathPoints.Count - 1;
        }
        if(Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.C && isHoldShift && pathPoints.Count != 0)
        {
            pathPoints.RemoveAt(currentPoint);
            normalPoints.RemoveAt(currentPoint);
            currentPoint = currentPoint - 1 < 0 ? 0 : currentPoint - 1;
        }

        if (pathPoints.Count != 0 && normalPoints.Count != 0)
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(pathPoints[currentPoint], normalPoints[currentPoint], pointRadius);
            Handles.DrawWireDisc(pathPoints[currentPoint], normalPoints[currentPoint], pointRadius / 5, 2f);
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(5f, pathPoints[0], pathPoints[0] + normalPoints[0]);
        }

        Handles.color = Color.blue;

        Handles.DrawAAPolyLine(2f, pathPoints.ToArray());
    }
    public void ClearList()
    { 
        pathPoints.Clear();
        normalPoints.Clear();
        currentPoint = 0;
    }
#endif
}

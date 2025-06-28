using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    public LayerMask      groundMask;
    public Transform      playerTransform;
    public float          pointSpacing = 0.01f;
    public float          startDistanceThreshold = 0.3f;

    private LineRenderer  lineRenderer;
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3       initialPlayerPosition;
    private Quaternion    initialPlayerRotation;
    private bool          isDrawing = false;
    private bool          finished = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialPlayerPosition = playerTransform.position;
        initialPlayerRotation = playerTransform.rotation;
    }

    public void DrawPath()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!TryGetMouseHitPoint(out Vector3 startPoint))
            {
                // ’n–Ê‚Éƒqƒbƒg‚µ‚Ä‚¢‚È‚¯‚ê‚Î‰½‚à‚µ‚È‚¢
                isDrawing = false;
                finished = false;
                return;
            }

            Vector2 startXZ = new Vector2(startPoint.x, startPoint.z);
            Vector2 currentXZ = new Vector2(playerTransform.position.x, playerTransform.position.z);
            Vector2 initialXZ = new Vector2(initialPlayerPosition.x, initialPlayerPosition.z);

            bool fromPlayer = Vector2.Distance(startXZ, currentXZ) < startDistanceThreshold;
            bool fromInitial = Vector2.Distance(startXZ, initialXZ) < startDistanceThreshold;

            if (fromInitial)
            {
                playerTransform.position = initialPlayerPosition;
                playerTransform.rotation = initialPlayerRotation;
                pathPoints.Clear();
                lineRenderer.positionCount = 0;
            }
            else if (!fromPlayer)
            {
                isDrawing = false;
                finished = false;
                return;
            }

            isDrawing = true;
            finished = false;

            startPoint = playerTransform.position + Vector3.up * 0.1f;
            pathPoints.Add(startPoint);
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());
        }

        if (isDrawing && Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, groundMask))
            {
                Vector3 point = hit.point + Vector3.up * 0.1f;

                if (pathPoints.Count == 0 || Vector3.Distance(pathPoints[^1], point) > pointSpacing)
                {
                    pathPoints.Add(point);
                    lineRenderer.positionCount = pathPoints.Count;
                    lineRenderer.SetPositions(pathPoints.ToArray());
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                isDrawing = false;
                finished = true;

                playerTransform.position = initialPlayerPosition;
                playerTransform.rotation = initialPlayerRotation;
            }
            else
            {
                finished = false;
            }
        }
    }

    private bool TryGetMouseHitPoint(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            point = hit.point + Vector3.up * 0.1f;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    public List<Vector3> GetPathPoints()
    {
        return new List<Vector3>(pathPoints);
    }

    public bool IsPathFinished()
    {
        return finished;
    }

    public void ResetPathState()
    {
        finished = false;
    }
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    public LayerMask      groundMask;
    public Transform      playerTransform;
    public Collider       goalCollider;
    public float          pointSpacing = 0.01f;
    public float          startDistanceThreshold = 0.3f;

    private LineRenderer  lineRenderer;
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3       initialPlayerPosition;
    private Quaternion    initialPlayerRotation;
    private bool          isDrawing = false;
    private bool          finished = false;
    private bool          isPathDrawingEnabled = true;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialPlayerPosition = playerTransform.position;
        initialPlayerRotation = playerTransform.rotation;
    }

    public void DrawPath()
    {
        if (!isPathDrawingEnabled)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!TryGetMouseHitPoint(out Vector3 startPoint))
            {
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

                    Vector2 pointXZ = new Vector2(point.x, point.z);
                    Vector2 goalXZ = new Vector2(goalCollider.bounds.center.x, goalCollider.bounds.center.z);

                    float distanceToGoal = Vector2.Distance(pointXZ, goalXZ);

                    // ゴールのXZ方向の半径（BoxCollider想定）
                    float stopThreshold = Mathf.Min(goalCollider.bounds.extents.x, goalCollider.bounds.extents.z) * 0.3f;

                    if (distanceToGoal < stopThreshold)
                    {
                        Debug.Log("ゴールエリア内に少し入ったタイミングで停止！");
                        DisablePathDrawing();
                        return;
                    }
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

    public void DisablePathDrawing()
    {
        isPathDrawingEnabled = false;
        isDrawing = false;
        finished = true;
    }
}
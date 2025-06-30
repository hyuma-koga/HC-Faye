using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    public LayerMask      groundMask;
    public Transform      playerTransform;
    public Collider       goalCollider;
    public float          pointSpacing = 0.05f;
    public float          startDistanceThreshold = 0.3f;

    private LineRenderer  lineRenderer;
    private List<Vector3> pathPoints = new List<Vector3>();
    private List<Vector3> smoothedPoints = new List<Vector3>();

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
        if (!isPathDrawingEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            ResetPath();
            TryGetMouseHitPoint(out Vector3 startPoint);
            startPoint = playerTransform.position + Vector3.up * 0.1f;
            pathPoints.Add(startPoint);
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());
            isDrawing = true;
        }

        if (isDrawing && Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, groundMask))
            {
                Vector3 point = hit.point + Vector3.up * 0.1f;
                if (Vector3.Distance(pathPoints[^1], point) > pointSpacing)
                {
                    pathPoints.Add(point);
                    lineRenderer.positionCount = pathPoints.Count;
                    lineRenderer.SetPositions(pathPoints.ToArray());

                    // ★ ゴール判定
                    if (IsPointInGoal(point))
                    {
                        Debug.Log("✅ ゴールエリアに到達したので描画停止！");
                        StopDrawing();
                        return;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                StopDrawing();
            }
        }
    }

    private void StopDrawing()
    {
        isDrawing = false;
        finished = true;

        // スムーズ化
        smoothedPoints = SmoothPath(pathPoints, 0.5f);

        // 線更新
        lineRenderer.positionCount = smoothedPoints.Count;
        lineRenderer.SetPositions(smoothedPoints.ToArray());
    }

    private void ResetPath()
    {
        pathPoints.Clear();
        smoothedPoints.Clear();
        lineRenderer.positionCount = 0;
        playerTransform.position = initialPlayerPosition;
        playerTransform.rotation = initialPlayerRotation;
        isDrawing = false;
        finished = false;
    }

    private List<Vector3> SmoothPath(List<Vector3> points, float smoothingFactor)
    {
        List<Vector3> result = new List<Vector3>(points);
        for (int i = 1; i < result.Count - 1; i++)
        {
            result[i] = Vector3.Lerp(result[i], (result[i - 1] + result[i + 1]) / 2f, smoothingFactor);
        }
        return result;
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

    private bool IsPointInGoal(Vector3 point)
    {
        Vector3 goalCenter = goalCollider.bounds.center;
        Vector3 goalExtents = goalCollider.bounds.extents;

        // x-z 平面のみ比較
        Vector2 pointXZ = new Vector2(point.x, point.z);
        Vector2 goalCenterXZ = new Vector2(goalCenter.x, goalCenter.z);
        float goalRadius = Mathf.Min(goalExtents.x, goalExtents.z) * 0.2f;

        return Vector2.Distance(pointXZ, goalCenterXZ) < goalRadius;
    }

    public List<Vector3> GetPathPoints()
    {
        return new List<Vector3>(smoothedPoints.Count > 0 ? smoothedPoints : pathPoints);
    }

    public bool IsPathFinished() => finished;

    public void ResetPathState() => finished = false;

    public void DisablePathDrawing()
    {
        isPathDrawingEnabled = false;
        isDrawing = false;
        finished = true;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class CarMover : MonoBehaviour
{
    public GoalColorChange goalColorChanger;
    public Color           playerColor = Color.red;
    public float           speed = 3f;

    private List<Vector3>  path;
    private int            currentIndex = 0;
    private bool           isMoving = false;

    public void SetPath(List<Vector3> newPath)
    {
        path = new List<Vector3>(newPath);
        currentIndex = 0;
        isMoving = true;
    }

    public void MoveAlongPath()
    {
        if (!isMoving || path == null || currentIndex >= path.Count)
        {
            return;
        }

        Vector3 target = path[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= path.Count)
            {
                isMoving = false;
                Debug.Log("ゴールに到達！クリア！");

                if (goalColorChanger != null)
                {
                    goalColorChanger.ChangeColor(playerColor);
                    Debug.Log("色変わり");
                }
            }
        }
    }
}

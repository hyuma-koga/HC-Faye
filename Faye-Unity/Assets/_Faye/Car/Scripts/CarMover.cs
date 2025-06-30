using UnityEngine;
using System.Collections.Generic;

public class CarMover : MonoBehaviour
{
    public GoalColorChange goalColorChanger;
    public Color playerColor = Color.red;
    public float speed = 3f;

    private GameClearUIHandler clearUIHandler; // Prefab外のオブジェクトを後でFindする

    private List<Vector3> path;
    private int currentIndex = 0;
    private bool isMoving = false;
    private bool hasCleared = false;
    private bool reachedGoalArea = false; // ← ゴールに入ったかフラグ

    private void Awake()
    {
        // GameClearUIHandler をシーン内から探して取得
        clearUIHandler = FindFirstObjectByType<GameClearUIHandler>();

        if (clearUIHandler != null)
        {
            clearUIHandler.HideUIImmediately();
        }
    }

    public void SetPath(List<Vector3> newPath)
    {
        path = new List<Vector3>(newPath);
        currentIndex = 0;
        isMoving = true;
        hasCleared = false;
        reachedGoalArea = false;

        if (path.Count > 0)
        {
            transform.position = path[0];
        }

        if (clearUIHandler != null)
        {
            clearUIHandler.HideUIImmediately();
        }
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
            }
        }

        // 「停止していて」「ゴールに入っていて」「まだ処理していない」場合に処理
        if (!isMoving && reachedGoalArea && !hasCleared)
        {
            if (goalColorChanger != null)
            {
                goalColorChanger.ChangeColor(playerColor);
            }

            if (clearUIHandler != null)
            {
                clearUIHandler.StartClearSequence();
            }

            hasCleared = true;
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            reachedGoalArea = true; // フラグを立てるだけ
        }
    }
}
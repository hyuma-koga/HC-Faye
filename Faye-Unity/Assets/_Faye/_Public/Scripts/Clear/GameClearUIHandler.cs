using UnityEngine;
using System.Collections;

public class GameClearUIHandler : MonoBehaviour
{
    public GameObject clearUI; // ← Inspector でセット
    private StageManager stageManager;

    private void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();

        if (clearUI != null)
        {
            clearUI.SetActive(false);
        }
    }

    public void HideUIImmediately()
    {
        if (clearUI != null)
        {
            clearUI.SetActive(false);
        }
    }

    public void StartClearSequence()
    {
        StartCoroutine(ClearSequenceCoroutine());
    }

    private IEnumerator ClearSequenceCoroutine()
    {
        // ゴール色変更後に1秒待つ
        yield return new WaitForSeconds(1f);

        // クリアUIを表示
        if (clearUI != null)
        {
            clearUI.SetActive(true);
        }

        // さらに2秒待つ
        yield return new WaitForSeconds(2f);

        // ステージ切り替え
        if (stageManager != null)
        {
            stageManager.OnPlayerReachGoal();
        }
    }
}
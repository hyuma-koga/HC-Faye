using UnityEngine;
using System.Collections;

public class GameClearUIHandler : MonoBehaviour
{
    public GameObject clearUI; // �� Inspector �ŃZ�b�g
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
        // �S�[���F�ύX���1�b�҂�
        yield return new WaitForSeconds(1f);

        // �N���AUI��\��
        if (clearUI != null)
        {
            clearUI.SetActive(true);
        }

        // �����2�b�҂�
        yield return new WaitForSeconds(2f);

        // �X�e�[�W�؂�ւ�
        if (stageManager != null)
        {
            stageManager.OnPlayerReachGoal();
        }
    }
}
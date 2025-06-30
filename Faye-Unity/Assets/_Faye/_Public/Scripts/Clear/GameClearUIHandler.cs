using UnityEngine;
using System.Collections;

public class GameClearUIHandler : MonoBehaviour
{
    public GameObject     clearUI;
    public ParticleSystem crackerEffect;

    private StageManager  stageManager;

    private void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();

        if (clearUI != null)
        {
            clearUI.SetActive(false);
        }

        if (crackerEffect != null)
        {
            crackerEffect.Stop();
        }
    }

    public void HideUIImmediately()
    {
        if (clearUI != null)
        {
            clearUI.SetActive(false);
        }
        
        if (crackerEffect != null)
        {
            crackerEffect.Stop();
        }
    }

    public void StartClearSequence()
    {
        StartCoroutine(ClearSequenceCoroutine());
    }

    private IEnumerator ClearSequenceCoroutine()
    {
        yield return new WaitForSeconds(1f);

        if (clearUI != null)
        {
            clearUI.SetActive(true);
        }

        if (crackerEffect != null)
        {
            crackerEffect.Play();
        }

        yield return new WaitForSeconds(4f);

        if (stageManager != null)
        {
            stageManager.OnPlayerReachGoal();
        }
    }
}
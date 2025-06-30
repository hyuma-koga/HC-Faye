using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class StageManager : MonoBehaviour
{
    public List<GameObject> stagePrefabs;
    public TMP_Text         stageNumberText;
    private GameObject      currentStage;
    private int             currentStageIndex = 0;

    private void Start()
    {
        LoadStage(currentStageIndex);
    }

    public void LoadStage(int index)
    {
        if (currentStage != null)
        {
            Destroy(currentStage);
        }

        if (index < stagePrefabs.Count)
        {
            currentStage = Instantiate(stagePrefabs[index], Vector3.zero, Quaternion.identity);
            UpdateStageNumberText(index + 1);
        }
    }

    // ƒS[ƒ‹‚É“ž’B‚µ‚½‚Æ‚«‚ÉŒÄ‚Î‚ê‚é
    public void OnPlayerReachGoal()
    {
        currentStageIndex++;
        LoadStage(currentStageIndex);
    }

    private void UpdateStageNumberText(int stageNumber)
    {
        if (stageNumberText != null)
        {
            stageNumberText.text = $"{stageNumber}";
        }
    }
}
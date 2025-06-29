using UnityEngine;
using TMPro;

public class GameScreenUI : MonoBehaviour
{
    public GameObject uiRoot; // UI全体まとめた親オブジェクト
    public TMP_Text   scoreText;

    private void Start()
    {
        if (uiRoot != null)
        {
            uiRoot.SetActive(true);
        }

        UpdateScoreText();
    }

    private void Update()
    {
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Instance.GetScore()}";
        }
    }

    public void ShowUI()
    {
        if (uiRoot != null)
        {
            uiRoot.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (uiRoot != null)
        {
            uiRoot.SetActive(false);
        }
    }
}
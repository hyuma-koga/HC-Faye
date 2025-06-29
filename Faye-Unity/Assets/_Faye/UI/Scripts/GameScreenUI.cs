using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameScreenUI : MonoBehaviour
{
    public GameObject uiRoot; // UI全体まとめた親オブジェクト
    public TMP_Text   scoreText;
    public Image[]    keyIcons;
    public Color      keyActiveColor = Color.yellow;
    public Color      keyDefaultColor = Color.white;

    private void Start()
    {
        if (uiRoot != null)
        {
            uiRoot.SetActive(true);
        }

        foreach (var icon in keyIcons)
        {
            if (icon != null)
            {
                icon.color = keyDefaultColor;
            }
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

    public void ActivateKey(int index)
    {
        if (index >= 0 && index < keyIcons.Length)
        {
            keyIcons[index].color = keyActiveColor;
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
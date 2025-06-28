using UnityEngine;

public class GoalColorChange : MonoBehaviour
{
    public MeshRenderer   baseRenderer;
    public SpriteRenderer childSpriteRenderer;
    public Color          defaultColor = Color.white;

    private void Awake()
    {
        if (baseRenderer != null)
        {
            baseRenderer.material.color = defaultColor;
        }
    }

    public void ChangeColor(Color newColor)
    {
        if (baseRenderer != null)
        {
            baseRenderer.material.color = newColor;
        }

        if (childSpriteRenderer != null)
        {
            Color c = childSpriteRenderer.color;
            c.a = 0f;
            childSpriteRenderer.color = c;
        }
    }

    public void ResetColor()
    {
        if (baseRenderer != null)
        {
            baseRenderer.material.color = defaultColor;
        }

        if (childSpriteRenderer != null)
        {
            Color c = childSpriteRenderer.color;
            c.a = 1f;
            childSpriteRenderer.color = c;
        }
    }
}
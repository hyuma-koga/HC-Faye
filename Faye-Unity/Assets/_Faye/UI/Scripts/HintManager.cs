using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject hintObject;
    public Transform  playerTransform;
    public float      floatAmplitude = 0.2f;
    public float      floatSpeed = 2f;
    public float      scaleAmplitude = 0.05f;
    public float      scaleSpeed = 2f;

    private Vector3   basePosition;
    private Vector3   initialScale;
    private float     animationTime = 0f;

    private void Awake()
    {
        if (hintObject == null)
        {
            return;
        }

        if (playerTransform == null)
        {
            return;
        }

        basePosition = hintObject.transform.position;
        initialScale = hintObject.transform.localScale;
        animationTime = 0f;
    }

    private void Update()
    {
        if (hintObject == null || playerTransform == null)
        {
            return;
        }

        bool currentlyInside = IsPlayerInsideHint();

        if (currentlyInside && hintObject.activeSelf)
        {
            hintObject.SetActive(false);
        }
        else if (!currentlyInside && !hintObject.activeSelf)
        {
            hintObject.SetActive(true);
            animationTime = 0f;
        }

        if (hintObject.activeSelf)
        {
            AnimateHint();
        }
    }

    private void AnimateHint()
    {
        animationTime += Time.deltaTime;

        float newY = basePosition.y + Mathf.Sin(animationTime * floatSpeed) * floatAmplitude;
        hintObject.transform.position = new Vector3(basePosition.x, newY, basePosition.z);

        float scale = 1f + Mathf.Sin(animationTime * scaleSpeed) * scaleAmplitude;
        hintObject.transform.localScale = initialScale * scale;
    }

    private bool IsPlayerInsideHint()
    {
        Collider hintCollider = hintObject.GetComponent<Collider>();
        Collider playerCollider = playerTransform.GetComponent<Collider>();
        if (hintCollider == null || playerCollider == null)
        {
            return false;
        }

        return hintCollider.bounds.Intersects(playerCollider.bounds);
    }
}
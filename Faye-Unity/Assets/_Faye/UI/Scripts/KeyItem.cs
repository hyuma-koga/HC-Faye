using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public int KeyIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var ui = FindFirstObjectByType<GameScreenUI>();
            if (ui != null)
            {
                ui.ActivateKey(KeyIndex);
            }

            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter called with tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Tag matched: Player");
            CarManager carManager = collision.gameObject.GetComponent<CarManager>();
            if (carManager != null)
            {
                Debug.Log("CarManager found! Calling CrashCar()");
                carManager.CrashCar();
            }
            else
            {
                Debug.LogError("CarManager not found on collided object!");
            }
        }
    }
}

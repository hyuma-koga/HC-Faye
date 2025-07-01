using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CarManager carManager = collision.gameObject.GetComponent<CarManager>();
            if (carManager != null)
            {
                carManager.CrashCar();
            }
        }
    }
}

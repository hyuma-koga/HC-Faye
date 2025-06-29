using UnityEngine;

public class CarCrashHandler : MonoBehaviour
{
    private bool isCrashed = false;

    public void TriggerCrash()
    {
        if (isCrashed)
        {
            return;
        }

        isCrashed = true;
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(-transform.forward * 400f);
            Vector3 smallTorque = new Vector3(Random.Range(50, 100), Random.Range(20, 50), Random.Range(50, 100));
            rb.AddTorque(smallTorque);
            rb.linearDamping = 2f;
            rb.angularDamping = 5f;
        }
    }

    public void ResetCrash()
    {
        isCrashed = false;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class CarManager : MonoBehaviour
{
    private PathDrawer      pathDrawer;
    private CarMover        carMover;
    private CarCrashHandler carCrashHandler;
    private Rigidbody       rb;
    private Vector3         initialPosition;
    private Vector3         initialScale;
    private Quaternion      initialRotation;

    private void Awake()
    {
        pathDrawer = GetComponent<PathDrawer>();
        carMover = GetComponent<CarMover>();
        carCrashHandler = GetComponent<CarCrashHandler>();
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    private void Update()
    {
        pathDrawer.DrawPath();

        if (pathDrawer.IsPathFinished())
        {
            carMover.SetPath(pathDrawer.GetPathPoints());
            pathDrawer.ResetPathState();
        }

        carMover.MoveAlongPath();

        if (Input.GetMouseButtonDown(0))
        {
            ResetCar();
        }
    }

    public void CrashCar()
    {
        if (carMover != null)
        {
            carMover.StopMoving();
        }

        if (rb == null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        if (carCrashHandler != null)
        {
            carCrashHandler.TriggerCrash();
        }
    }

    public void ResetCar()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }

        if (carCrashHandler != null)
        {
            carCrashHandler.ResetCrash();
        }

        if (carMover != null)
        {
            carMover.enabled = true;
        }
    }
}

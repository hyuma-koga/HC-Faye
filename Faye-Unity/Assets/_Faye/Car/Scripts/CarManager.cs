using UnityEngine;
using System.Collections.Generic;

public class CarManager : MonoBehaviour
{
    private PathDrawer pathDrawer;
    private CarMover   carMover;

    private void Awake()
    {
        pathDrawer = GetComponent<PathDrawer>();
        carMover = GetComponent<CarMover>();
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
    }
}

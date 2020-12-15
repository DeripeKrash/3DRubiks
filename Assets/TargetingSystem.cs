using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float rayCastLength = 1;
    [SerializeField] private LayerMask layer;

    public Vector3 rotationVector;

    private Vector3 lockPoint;
    private Vector3 worldPosition;
    private Vector3 localPosition;
    private bool getPointOnTheCube;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength) && getPointOnTheCube == false)
            {
                worldPosition = hit.point;
                localPosition = transform.InverseTransformPoint(hit.point);
                getPointOnTheCube = true;
                lockPoint = worldPosition;
            }

            else if (getPointOnTheCube == true)
            {
                Vector3 mousePos    = Input.mousePosition;
                mousePos.z          = (hit.point - Camera.main.transform.position).magnitude;
                worldPosition       = Camera.main.ScreenToWorldPoint(mousePos);
                rotationVector      = worldPosition - lockPoint;
            }
        }

        else if (!Input.GetMouseButton(0))
        {
            getPointOnTheCube = false;
        }
    }
}

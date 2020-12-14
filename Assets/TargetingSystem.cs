using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float rayCastLength = 1;
    [SerializeField] private LayerMask layer;

    Vector3 worldPosition;
    Vector3 localPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                worldPosition = hit.point;
                localPosition = transform.InverseTransformPoint(hit.point);
                print(localPosition);
            }
        }
    }
}

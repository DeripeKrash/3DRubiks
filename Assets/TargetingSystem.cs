using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    Rubickscube rubick;

    [SerializeField] private float rayCastLength = 1;
    [SerializeField] private LayerMask layer;

    public Vector3 worldPosition;
    public Vector3 localPosition;

    Vector3 refPos;
    Vector3 refworld;
    Vector3 refNormal;

    void Start()
    {
        rubick = GetComponent<Rubickscube>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                refPos = transform.InverseTransformPoint(hit.point);
                refworld = hit.point;
                refNormal = hit.normal;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                worldPosition = hit.point;
                localPosition = transform.InverseTransformPoint(hit.point);

                if ((refPos - localPosition).magnitude > 0.1)
                {

                    Vector3 axis = rubick.SortVector(hit.normal, (worldPosition - refworld).normalized);

                    Vector3 localAxis = transform.InverseTransformPoint(axis);
                    Vector3 Value = localPosition;
                    Value.x *= localAxis.x;
                    Value.y *= localAxis.y;
                    Value.z *= localAxis.z;

                    float value1 = Mathf.Max(Value.x, Value.y, Value.z);
                    float value2 = Mathf.Min(Value.x, Value.y, Value.z);

                    float value;
                    float direction = 1;

                    if (Mathf.Abs(value1) > Mathf.Abs(value2))
                    {
                        value = value1;
                    }
                    else
                    {
                        value = value2;
                    };

                    StartCoroutine(rubick.RotateLineAround(axis, value, 0.5f, direction));
                }
            }
        }
    }
}

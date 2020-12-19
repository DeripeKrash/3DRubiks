using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    Rubickscube rubick;

    [SerializeField] private float rayCastLength = 1000.0f;
    [SerializeField] private LayerMask layer;

    public Vector3 rotationVector;

    Vector3 refPos;
    Vector3 refworld;
    Vector3 refNormal;

    bool animating = false;

    void Start()
    {
        rubick = GetComponent<Rubickscube>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rubick.rotate)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !animating)
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
        else if (Input.GetMouseButton(0) && !animating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                Vector3 localPosition = transform.InverseTransformPoint(hit.point);

                if ((refPos - localPosition).magnitude > 0.2f)
                {
                    Vector3 axis = SortVector(hit.normal, (hit.point - refworld).normalized);

                    if (Vector3.Angle(refNormal, hit.normal) != 0 && Vector3.Angle(refNormal, hit.normal) != 180) // Check if two different faces aren't checked
                    {
                        return;
                    }

                    Vector3 localAxis = transform.InverseTransformPoint(axis);

                    float value = Vector3.Dot(localAxis, refPos);
                    float direction = Direction(axis, refworld, hit.point);


                    StartCoroutine(rubick.RotateLineAround(axis, value, 0.5f, direction));
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animating = false;
        }
    }

    public Vector3 SortVector(Vector3 normal, Vector3 vect)
    {
        Vector3 localNormal = transform.InverseTransformPoint(normal);
        Vector3 localVect = transform.InverseTransformPoint(vect);

        localNormal.x = Mathf.Abs(localNormal.x);
        localNormal.y = Mathf.Abs(localNormal.y);
        localNormal.z = Mathf.Abs(localNormal.z);

        if ((localNormal - Vector3.forward).magnitude <= 0.1)
        {
            if (Mathf.Abs(Vector3.Dot(localVect, Vector3.up)) < Mathf.Abs(Vector3.Dot(localVect, Vector3.right)))
            {
                return transform.up;
            }
            else
            {
                return transform.right;
            }
        }
        else if ((localNormal - Vector3.up).magnitude <= 0.1)
        {
            if (Mathf.Abs(Vector3.Dot(localVect, Vector3.right)) < Mathf.Abs(Vector3.Dot(localVect, Vector3.forward)))
            {
                return transform.right;
            }
            else
            {
                return transform.forward;
            }
        }
        else if ((localNormal - Vector3.right).magnitude <= 0.1)
        {
            if (Mathf.Abs(Vector3.Dot(localVect, Vector3.forward)) < Mathf.Abs(Vector3.Dot(localVect, Vector3.up)))
            {
                return transform.forward;
            }
            else
            {
                return transform.up;
            }
        }

        return Vector3.zero;
    }

    float Direction(Vector3 axis, Vector3 start, Vector3 end)
    {
        return Vector3.SignedAngle(start, end, axis) / Mathf.Abs(Vector3.SignedAngle(start, end, axis));
    }
}



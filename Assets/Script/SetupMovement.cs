using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMovement : MonoBehaviour
{
    [SerializeField] private float mouseSpeed       = 1;
    [SerializeField] private float brakeSpeed       = 0;
    [SerializeField] private bool inertia           = false;
    [SerializeField] private float rayCastLength    = 1000.0f;
    [SerializeField] private float zoomMin          = 0;
    [SerializeField] private float zoomMax          = 1000.0f;

    Rubickscube rubick;

    Vector3 refNormal;
    Vector3 lastPos;
    Vector3 axis;

    Plane plane;
    float velocity;
    bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
        rubick = GetComponent<Rubickscube>();
        Camera.main.transform.LookAt(transform);
    }

    void Update()
    {
        //Acquire reference point on the cube and initialize all parameter needed for the rotation when pressing right-click;
        if (Input.GetMouseButtonDown(1) && !rotating && !rubick.rotate)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                refNormal           = hit.normal;
                plane.SetNormalAndPosition(refNormal, hit.point);
                rotating            = true;
                lastPos             = hit.point;
            }
        }

        //Rotate the cube;
        else if (Input.GetMouseButton(1) && rotating && !rubick.rotate)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 point = ray.GetPoint(distance);
                
                if (point != lastPos)
                {
                    Vector3 vect = (point - lastPos).normalized;
                    velocity = vect.magnitude;
                    axis = Vector3.Cross(refNormal, vect);
                    transform.rotation = Quaternion.AngleAxis(velocity * mouseSpeed * Time.deltaTime, axis) * transform.rotation;

                    Debug.Log("Movement : Rotation Axis" + axis);

                }
                else
                {
                    velocity = 0;
                }

                lastPos = point;
            }
        }

        //Stop rotating the cube or apply inertia when realeasing the key;
        else
        {
            if (velocity > 0 && inertia && !rubick.rotate)
            {
                transform.rotation  = Quaternion.AngleAxis(velocity * mouseSpeed * Time.deltaTime, axis) * transform.rotation;
                velocity           -= brakeSpeed;
            }
            else
            {
                velocity = 0;
            }

            rotating = false;
        }

        //ZoomIn/ZoomOut with mouse scrolling;
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            Camera.main.transform.Translate(Camera.main.transform.forward * -Input.mouseScrollDelta.y);

            if ( (transform.position - Camera.main.transform.position).magnitude < zoomMin || (transform.position - Camera.main.transform.position).magnitude > zoomMax)
            {
                Camera.main.transform.Translate(Camera.main.transform.forward * Input.mouseScrollDelta.y);
            }
        }
    }


    public void SwitchInertia()
    {
        inertia = !inertia;
    }
}

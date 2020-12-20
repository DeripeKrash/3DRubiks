using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMovement : MonoBehaviour
{
    [SerializeField] private float mouseSpeed       = 1;
    [SerializeField] private float brakeSpeed       = 0;
    [SerializeField] private bool inertia           = false;
    [SerializeField] private float rayCastLength    = 1000.0f;

    Rubickscube rubick;

    Vector3 refPos;
    Vector3 refNormal;
    Vector3 axis;

    Plane plane;
    float magnitude;
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
        if (Input.GetMouseButtonDown(1) && !rotating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                refPos      = hit.point;
                refNormal   = hit.normal;
                plane.SetNormalAndPosition(refNormal, refPos);
                rotating    = true;
            }
        }

        //Rotate the cube;
        else if (Input.GetMouseButton(1) && rotating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 point       = ray.GetPoint(distance);
                Vector3 vect        = point - refPos;
                axis                = Vector3.Cross(refNormal, vect);
                transform.rotation  = Quaternion.AngleAxis(vect.magnitude * mouseSpeed * Time.deltaTime, axis) * transform.rotation;
                magnitude           = vect.magnitude;
            }
        }

        //Stop rotating the cube or apply inertia when realeasing the key;
        else
        {
            if (magnitude > 0 && inertia && !rubick.rotate)
            {
                transform.rotation  = Quaternion.AngleAxis(magnitude * mouseSpeed * Time.deltaTime, axis) * transform.rotation;
                magnitude           -= brakeSpeed;
            }

            else
            {
                magnitude = 0;
            }

            rotating = false;
        }

        //ZoomIn/ZoomOut with mouse scrolling;
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            Camera.main.transform.Translate(Camera.main.transform.forward * Input.mouseScrollDelta.y);
        }
    }


    public void SwitchInertia()
    {
        inertia = !inertia;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMovement : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 1;

    Rubickscube rubick;

    Vector3 refPos;
    Vector3 refNormal;

    Vector3 lastLocation;

    [SerializeField] private float rayCastLength = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {
        rubick = GetComponent<Rubickscube>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength))
            {
                refPos = hit.point;
                refNormal = hit.normal;
                //Cursor.lockState = CursorLockMode.Locked;
            }
        }

        else if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayCastLength) && !rubick.rotate)
            {
                if (lastLocation == hit.point)
                {
                    return;
                }

                Vector3 vect = hit.point - refPos;

                Vector3 axis = Vector3.Cross(refNormal, vect);

                transform.rotation = Quaternion.AngleAxis(vect.magnitude * mouseSpeed * Time.deltaTime, axis) * transform.rotation;

                lastLocation = hit.point;
            }
        }

        else
        {
            //Cursor.lockState = CursorLockMode.None;
        }
    }

}

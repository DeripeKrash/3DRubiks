using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float mouseSpeed = 1;

    Rubickscube rubick;

    // Start is called before the first frame update
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

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Height") != 0)
        {
            Vector3 newPos = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Height"), Input.GetAxis("Vertical"));
            transform.Translate(newPos * speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            transform.Rotate(new Vector3(1, 0, 0), Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime, Space.World);
            transform.Rotate( new Vector3(0,1,0) , Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime, Space.World);
        }

        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

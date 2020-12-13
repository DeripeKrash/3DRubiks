using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float rayCastLength = 1;
    [SerializeField] private LayerMask layer;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //print(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane)));
            Ray ray  = Camera.main.ScreenPointToRay(Input.mousePosition);
            //print(ray);
            ray.direction *= rayCastLength;
            print(ray);
            RaycastHit hit;
            //if (Physics.Raycast(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane)), this.transform.forward, out hit, rayCastLength, layer))
            if (Physics.Raycast(ray, out hit, rayCastLength, layer))
            {
                print("yolo");
                print(hit.point);
            }
        }

        /*else if (Input.GetMouseButtonUp(0))
        {

        }*/
    }
}

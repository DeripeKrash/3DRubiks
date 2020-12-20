using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCube : MonoBehaviour
{
    // List used to show or hide the face that don't have a color
    List<Transform> listFaces = new List<Transform>();

    public void AddFace(List<Transform> list, Vector3 FaceVector, Material mat) // The face of the cube that have the same normal as FaceVector have their color changed
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if ((transform.GetChild(0).GetChild(i).forward - FaceVector).magnitude < 0.2)
            {
                
                Renderer rend = transform.GetChild(0).GetChild(i).GetComponent<Renderer>();

                if (rend)
                {
                    rend.material = mat;
                }

                transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

                listFaces.Add(transform.GetChild(0).GetChild(i));
                list.Add(transform.GetChild(0).GetChild(i));
                return;

            }
        }
    }

    public void Optimize(bool active = true) // Show/Hide the Hidden faces
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            bool clear = true;
            for (int e = 0; e < listFaces.Count; e++)
            {
                if (transform.GetChild(0).GetChild(i) == listFaces[e])
                {
                    clear = false;
                }
            }
            if (clear)
            {
                transform.GetChild(0).GetChild(i).gameObject.SetActive(!active);
            }
        }
    }
}

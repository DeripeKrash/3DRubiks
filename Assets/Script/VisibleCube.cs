using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public uint color;
    public Transform trsFace;

    public Face(uint _color, Transform _trsFace) 
    {
        color = _color;
        trsFace = _trsFace;
    }
};

public class VisibleCube : MonoBehaviour
{
    List<Transform> listFaces = new List<Transform>();

    public void AddFace(List<Transform> list, Vector3 FaceVector, Material mat, uint color) // The face of the cube that have the 
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

                Face newFace = new Face();
                newFace.trsFace = transform.GetChild(0).GetChild(i);
                newFace.color = color;

                listFaces.Add(transform.GetChild(0).GetChild(i));
                list.Add(transform.GetChild(0).GetChild(i));

            }
        }
    }

    public void Optimize(bool active = true)
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
                Destroy(transform.GetChild(0).GetChild(i).gameObject);
                //transform.GetChild(0).GetChild(i).gameObject.SetActive(!active);
            }
        }
    }
}

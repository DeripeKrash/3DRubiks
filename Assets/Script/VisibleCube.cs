using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public uint color;
    public Transform trsFace;
};

public class VisibleCube : MonoBehaviour
{
    List<Face> listFaces = new List<Face>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddFace(List<Transform> list, Vector3 FaceVector, Material mat, uint color)
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

                listFaces.Add(newFace);
                list.Add(transform.GetChild(0).GetChild(i));

            }
        }
    }

    public void Clean()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            bool clear = true;
            for (int e = 0; e < listFaces.Count; e++)
            {
                if (transform.GetChild(0).GetChild(i) == listFaces[e].trsFace)
                {
                    clear = false;
                }
            }
            if (clear)
            {
                Destroy(transform.GetChild(0).GetChild(i).gameObject);
            }
        }
    }
}

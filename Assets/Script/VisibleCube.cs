using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
struct Face
{
    Vector3 normal;
    uint color;
=======
public struct Face
{
    public uint color;
    public Transform trsFace;
>>>>>>> 020053c5d12f23fcccac5972ea835d757f590869
};

public class VisibleCube : MonoBehaviour
{
    List<Face> listFaces = new List<Face>();

    List<Face> listFaces;

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

<<<<<<< HEAD
    public void AddFace(Vector3 normal, uint color)
    {
        //listFaces.Add({normal, color});
    }

    public void SetPositionOnCube(uint I, uint J, uint K)
=======
    public void Clean()
>>>>>>> 020053c5d12f23fcccac5972ea835d757f590869
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

    public void AddFace(Vector3 FaceVector, Material mat)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).forward == FaceVector)
            {
                Renderer rend = transform.GetChild(0).GetChild(i).GetComponent<Renderer>();
                //transform.GetChild(i).GetComponent < Material > = mat;

                if (rend)
                {
                    rend.material = mat;
                }
            }
        }
    }
}

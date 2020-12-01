using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Face
{
    Vector3 normal;
    uint color;
};

public class VisibleCube : MonoBehaviour
{
    public uint i;
    public uint j;
    public uint k;

    List<Face> listFaces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFace(Vector3 normal, uint color)
    {
        //listFaces.Add({normal, color});
    }

    public void SetPositionOnCube(uint I, uint J, uint K)
    {
        i = I;
        j = J;
        k = K;
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

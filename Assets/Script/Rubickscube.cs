using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubickscube : MonoBehaviour
{
    [SerializeField] uint size = 3;

    [SerializeField] VisibleCube refCube;

    [SerializeField] List<VisibleCube> visibleCubes;

    [SerializeField] Material Color1;
    [SerializeField] Material Color2;
    [SerializeField] Material Color3;
    [SerializeField] Material Color4;
    [SerializeField] Material Color5;
    [SerializeField] Material Color6;

    // Start is called before the first frame update
    void Start()
    {

        float U = (0.5f * (size - 1)) / size;

        for (uint i = 0; i < size; i++)
        {
            for (uint j = 0; j < size; j++)
            {
                for (uint k = 0; k < size; k++)
                {
                    if (i == 0 || j == 0 || k == 0 || i == size - 1 || j == size - 1 || k == size - 1)
                    {

                        VisibleCube newCube = Instantiate(refCube);

                        newCube.transform.localScale = new Vector3(1.0f / size, 1.0f / size, 1.0f / size);
                        
                        newCube.transform.parent = transform; // Put new Cube under the Rubicks Cube

                        newCube.transform.position = transform.position;

                        newCube.transform.GetChild(0).position = new Vector3(i * (1.0f / size), j * (1.0f / size), k * (1.0f / size));
                        newCube.transform.GetChild(0).position -= new Vector3(U, U, U);

                        newCube.SetPositionOnCube(i, j, k);

                        
                        if (i == 0)
                        {
                            newCube.AddFace(Vector3.forward, Color1);
                        }
                        /*if (j == 0)
                        {
                            newCube.AddFace(Vector3.up, Color2);
                        }
                        if (k == 0)
                        {
                            newCube.AddFace(Vector3.forward, Color3);
                        }*/

                        visibleCubes.Add(newCube);
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        //RotateLine(2, 1);
    }

    /*void RotateLine(uint J, float angle)
    {
        for (int i = 0; i < visibleCubes.Count; i++)
        {
            if (visibleCubes[i].j == J)
            {
                visibleCubes[i].transform.Rotate(new Vector3(0,angle,0));
            }

        }
    }*/

    IEnumerator RotateLine(float duration)
    {
        float actualTime = 0;
        float lastTime = Time.time;

        while (duration - actualTime > 0)
        {
            actualTime += Time.time - lastTime;
            lastTime = Time.time;



            yield return new WaitForEndOfFrame();
        }

        yield break;
    }
}

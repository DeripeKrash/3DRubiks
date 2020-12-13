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

    List<Transform> color1Faces = new List<Transform>();
    List<Transform> color2Faces = new List<Transform>();
    List<Transform> color3Faces = new List<Transform>();
    List<Transform> color4Faces = new List<Transform>();
    List<Transform> color5Faces = new List<Transform>();
    List<Transform> color6Faces = new List<Transform>();

    [SerializeField] int rot;

    [SerializeField] bool optimizedDisplay = true;

    float TimeCount;
    bool rotate = false;

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
                        
                        if (i == 0)
                        {
                            newCube.AddFace(color1Faces, Vector3.right, Color1, 1);
                        }
                        if (j == 0)
                        {
                            newCube.AddFace(color2Faces, Vector3.up, Color2, 2);
                        }
                        if (k == 0)
                        {
                            newCube.AddFace(color3Faces, Vector3.forward, Color3, 3);
                        }
                        if (i == size - 1)
                        {
                            newCube.AddFace(color4Faces, - Vector3.right, Color4, 4);
                        }
                        if (j == size - 1)
                        {
                            newCube.AddFace(color5Faces, - Vector3.up, Color5, 5);
                        }
                        if (k == size - 1)
                        {
                            newCube.AddFace(color6Faces, - Vector3.forward, Color6, 6);
                        }

                        visibleCubes.Add(newCube);
                    }
                }
            }
        }

        if (optimizedDisplay)
        {
            for (int i = 0; i < visibleCubes.Count; i++)
            {
                visibleCubes[i].Clean();
            }
        }

        //Shuffle(100);
    }


    // Update is called once per frame
    void Update()
    {
        TimeCount += Time.deltaTime;

        if (TimeCount > 2)
        {
            if (!rotate)
            {
                rotate = true;
                //StartCoroutine(RotateLineAround(Vector3.forward, 0.0f, 3));
                
                int axisNb = Random.Range(0, 2);
                axisNb = 1;

                if (rot == 0)
                {
                    StartCoroutine(RotateLineAround(Vector3.forward, 0.4f, 0.5f));
                }
                else if (rot == 1)
                {
                    StartCoroutine(RotateLineAround(Vector3.right, 0.4f, 0.5f));
                }
                else if (rot == 2)
                {
                    StartCoroutine(RotateLineAround(Vector3.up, 0.4f, 0.5f));
                }
            }
        }
    }

    void Shuffle(int shuffle)
    {
        for (int i = 0; i < shuffle; i++)
        {
            int axisNb = Random.Range(0, 2);
            
            if (axisNb == 0)
            {
                RotateLineAroundAxis(Vector3.up, 1, Random.Range(-1, 1));
            }
            else if (axisNb == 1)
            {
                RotateLineAroundAxis(Vector3.forward, 1, Random.Range(-1, 1));
            }
            else if (axisNb == 2)
            {
                RotateLineAroundAxis(Vector3.right, 1, Random.Range(-1, 1));
            }
        }
    }

    void RotateLineAroundAxis(Vector3 axis, float factor, float height)
    {
        Quaternion Start = transform.rotation;
        Quaternion End = Quaternion.Euler(axis * 90) * Start;

        for (int i = 0; i < visibleCubes.Count; i++)
        {
            float localHeight = Vector3.Dot(visibleCubes[i].transform.GetChild(0).position, axis);

            if (Mathf.Abs(height - localHeight) < (1.0f / size) / 2.0f)
            {
                visibleCubes[i].transform.rotation = End  * visibleCubes[i].transform.rotation;
            }
        }

    }

    IEnumerator RotateLineAround(Vector3 axis, float height, float duration)
    {
        float actualTime = 0;
        float lastFrame  = 0;
        float lastTime   = Time.time;

        Quaternion Start = transform.rotation;
        Quaternion End = Quaternion.Euler(axis * 90) * Start;

        while (duration - actualTime > 0)
        {
            lastFrame = actualTime;
            actualTime += Time.time - lastTime;
            lastTime = Time.time;

            for (int i = 0; i < visibleCubes.Count; i++)
            {
                float localHeight = Vector3.Dot(visibleCubes[i].transform.GetChild(0).position, axis);

                if (Mathf.Abs(height - localHeight) < (1.0f / size) / 2.0f)
                {
                    Quaternion reset = Quaternion.Slerp(Start, End, lastFrame / duration);
                    reset.x *= -1;
                    reset.y *= -1;
                    reset.z *= -1;

                    visibleCubes[i].transform.rotation = reset * visibleCubes[i].transform.rotation;
                    visibleCubes[i].transform.rotation = Quaternion.Slerp(Start, End, actualTime / duration) * visibleCubes[i].transform.rotation;
                }
            }

            yield return new WaitForEndOfFrame();

        }

        Debug.Log(CheckVictory());
        rotate = false;
        TimeCount = 0;

        yield break;
    }

    bool CheckVictory()
    {
        if (CheckFaceList(color1Faces))
        {
            if (CheckFaceList(color2Faces))
            {
                if (CheckFaceList(color3Faces))
                {
                    if (CheckFaceList(color4Faces))
                    {
                        if (CheckFaceList(color5Faces))
                        {
                            if (CheckFaceList(color6Faces))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

        }


        return false;
    }

    bool CheckFaceList(List<Transform> list)
    {
        //Debug.Log(list.Count);

        for (int i = 1; i < list.Count; i++)
        {
            if ((list[i].forward - list[0].forward).magnitude > 0.1f)
            {
                return false;
            }
        }

        return true;
    }
}

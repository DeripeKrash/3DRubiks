using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class Rubickscube : MonoBehaviour
{
    [SerializeField] [Range(2, 10)] public uint size = 3;

    [SerializeField] VisibleCube refCube;

    [SerializeField] public List<VisibleCube> visibleCubes;

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

    [SerializeField] public uint shuffleNumber = 0;

    [SerializeField] bool optimizedDisplay = true;

    [SerializeField] VictoryDisplay victoryDisplay = null;



    float lastUsedFactor;

    public bool rotate = false;


    // Start is called before the first frame update
    void Start()
    {
        Launch();

        Shuffle();
    }

    public void SetSize(System.Single _size) // used for the UI
    {
        size = (uint)_size;
    }
    public void SetShuffle(System.Single shufle) // used for the UI
    {
        shuffleNumber = (uint)shufle;
    }

    void Launch()  // Create the rubicks Cube
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
                            newCube.AddFace(color4Faces, -Vector3.right, Color4, 4);
                        }
                        if (j == size - 1)
                        {
                            newCube.AddFace(color5Faces, -Vector3.up, Color5, 5);
                        }
                        if (k == size - 1)
                        {
                            newCube.AddFace(color6Faces, -Vector3.forward, Color6, 6);
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
                visibleCubes[i].Optimize();
            }
        }
    }

    public void ReLaunch()
    {
        for (int i = 0; i < visibleCubes.Count; i++)
        {
            Destroy(visibleCubes[i].gameObject);
        }
        visibleCubes.Clear();

        color1Faces.Clear();
        color2Faces.Clear();
        color3Faces.Clear();
        color4Faces.Clear();
        color5Faces.Clear();
        color6Faces.Clear();

        transform.rotation = Quaternion.identity;

        Launch();

    }

    public void Reset()
    {
        ReLaunch();
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReLaunch();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    void Shuffle()
    {
        for (int i = 0; i < shuffleNumber; i++)
        {
            int axisNb = Random.Range(0, 3);

            if (axisNb == 0)
            {
                QuickRotateLineAroundAxis(Vector3.up, Random.Range(-0.49f, 0.49f));
            }
            else if (axisNb == 1)
            {
                QuickRotateLineAroundAxis(Vector3.forward, Random.Range(-0.49f, 0.49f));
            }
            else if (axisNb == 2)
            {
                QuickRotateLineAroundAxis(Vector3.right, Random.Range(-0.49f, 0.49f));
            }
        }
    }

    void RotateLineAroundAxis(Vector3 axis, float factor, float oldFactor, float height, float direction = 1.0f)
    {
        Quaternion Start = transform.rotation;
        Quaternion End = Quaternion.AngleAxis(90 * direction, axis) * Start;

        for (int i = 0; i < visibleCubes.Count; i++)
        {
            //Vector3 LocalPosition = transform.worldToLocalMatrix * visibleCubes[i].transform.GetChild(0).position;

            float localHeight = Vector3.Dot(visibleCubes[i].transform.GetChild(0).position, axis);

            if (Mathf.Abs(height - localHeight) < (1.0f / size) / 2.0f)
            {
                Quaternion reset = Quaternion.Slerp(Start, End, oldFactor);
                reset.x *= -1;
                reset.y *= -1;
                reset.z *= -1;

                visibleCubes[i].transform.rotation = reset * visibleCubes[i].transform.rotation;
                visibleCubes[i].transform.rotation = Quaternion.Slerp(Start, End, 1) * visibleCubes[i].transform.rotation;
            }
        }
    }

    void QuickRotateLineAroundAxis(Vector3 axis, float height) // Only used for Shuffle
    {
        Quaternion End = Quaternion.AngleAxis(90, axis) * transform.rotation;

        for (int i = 0; i < visibleCubes.Count; i++)
        {
            float localHeight = Vector3.Dot(visibleCubes[i].transform.GetChild(0).position, axis);

            if (Mathf.Abs(height - localHeight) < (1.0f / size) / 2.0f)
            {
                visibleCubes[i].transform.rotation = End * visibleCubes[i].transform.rotation;
            }
        }
    }

    public Vector3 SortVector(Vector3 normal, Vector3 vect)
    {
        Vector3 localNormal = transform.InverseTransformPoint(normal);
        Vector3 localVect   = transform.InverseTransformPoint(vect);

        localNormal.x = Mathf.Abs(localNormal.x);
        localNormal.y = Mathf.Abs(localNormal.y);
        localNormal.z = Mathf.Abs(localNormal.z);

        if ((localNormal - Vector3.forward).magnitude <= 0.1)
        {
            if (Mathf.Abs(Vector3.Dot(localVect, Vector3.up)) < Mathf.Abs(Vector3.Dot(localVect, Vector3.right)))
            {
                return transform.up;
            }
            else
            {
                return transform.right;
            }
        }
        else if ((localNormal - Vector3.up).magnitude <= 0.1)
        {
            if (Mathf.Abs(Vector3.Dot(localVect, Vector3.right)) < Mathf.Abs(Vector3.Dot(localVect, Vector3.forward)))
            {
                return transform.right;
            }
            else
            {
                return transform.forward;
            }
        }
        else if ((localNormal - Vector3.right).magnitude <= 0.1)
        {
            if (Mathf.Abs(Vector3.Dot(localVect, Vector3.forward)) < Mathf.Abs(Vector3.Dot(localVect, Vector3.up)))
            {
                return transform.forward;
            }
            else
            {
                return transform.up;
            }
        }

        return Vector3.zero;
    }

    public float Direction(Vector3 axis, Vector3 start, Vector3 end)
    {
        return Vector3.SignedAngle(start, end, axis) / Mathf.Abs(Vector3.SignedAngle(start, end, axis));
    }

    public IEnumerator RotateLineAround(Vector3 axis, float height, float duration, float direction = 1.0f)
    {
        if (rotate)
        {
            yield break;
        }

        rotate = true;

        float actualTime = 0;
        float lastFrame;
        float lastTime   = Time.time;

        Quaternion Start = transform.rotation;
        Quaternion End = Quaternion.AngleAxis(90 * direction, axis) * Start;

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

            DisplayVictory();

            yield return new WaitForEndOfFrame();

        }

        rotate = false;

        yield break;
    }

    void DisplayVictory()
    {
        if (victoryDisplay)
        {
            victoryDisplay.UpdateDisplay(CheckVictory());
        }
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
        for (int i = 1; i < list.Count; i++)
        {
            if ((list[i].forward - list[0].forward).magnitude > 0.1f)
            {
                return false;
            }
        }

        return true;
    }



    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/RubicksCubeWilliamDenis.save";
        FileStream stream = new FileStream(path, FileMode.Create);


        SaveData data = new SaveData(this);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/RubicksCubeWilliamDenis.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            data.Load(this);
            stream.Close();
        }
    }

}
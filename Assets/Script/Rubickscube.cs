using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class Rubickscube : MonoBehaviour
{
    [SerializeField] VisibleCube referenceCube;

    /* Cube Option */
    [SerializeField] [Range(2, 10)] public uint size = 3;
    [SerializeField] public uint shuffleNumber = 0;

    [SerializeField] Material Color1;
    [SerializeField] Material Color2;
    [SerializeField] Material Color3;
    [SerializeField] Material Color4;
    [SerializeField] Material Color5;
    [SerializeField] Material Color6;

    [SerializeField] VictoryDisplay victoryMessage = null;

    public List<VisibleCube> visibleCubes;

    // List of all the sames faces. Those list are used to check if the cube is completed 
    List<Transform> color1Faces = new List<Transform>();
    List<Transform> color2Faces = new List<Transform>();
    List<Transform> color3Faces = new List<Transform>();
    List<Transform> color4Faces = new List<Transform>();
    List<Transform> color5Faces = new List<Transform>();
    List<Transform> color6Faces = new List<Transform>();

    public bool rotate = false; // used to know when the RubicksCube has a rotationg line


    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    public void SetSize(System.Single _size) // used for the UI
    {
        size = (uint)_size;
    }
    public void SetShuffle(System.Single shufle) // used for the UI
    {
        shuffleNumber = (uint)shufle;
    }

    void Launch()  // Create the RubicksCube. It is create in position {0,0,0} and a null rotation 
    {
        float U = (0.5f * (size - 1)) / size; // used to place the cubes in the RubickCube

        for (uint i = 0; i < size; i++)
        {
            for (uint j = 0; j < size; j++)
            {
                for (uint k = 0; k < size; k++)
                {
                    if (i == 0 || j == 0 || k == 0 || i == size - 1 || j == size - 1 || k == size - 1)
                    {

                        VisibleCube newCube = Instantiate(referenceCube);

                        // each cube has a parent which is place at the origin.
                        //This parent will be rotate to move the slide of the cube without having to control the new position
                        newCube.transform.localScale = new Vector3(1.0f / size, 1.0f / size, 1.0f / size); 
                        newCube.transform.parent = transform; // Put new Cube under the Rubicks Cube
                        newCube.transform.position = transform.position;

                        // Place the cube in the RubickCube
                        newCube.transform.GetChild(0).position = new Vector3(i * (1.0f / size), j * (1.0f / size), k * (1.0f / size));
                        newCube.transform.GetChild(0).position -= new Vector3(U, U, U);

                        if (i == 0)
                        {
                            newCube.AddFace(color1Faces, Vector3.right, Color1);
                        }
                        if (j == 0)
                        {
                            newCube.AddFace(color2Faces, Vector3.up, Color2);
                        }
                        if (k == 0)
                        {
                            newCube.AddFace(color3Faces, Vector3.forward, Color3);
                        }
                        if (i == size - 1)
                        {
                            newCube.AddFace(color4Faces, -Vector3.right, Color4);
                        }
                        if (j == size - 1)
                        {
                            newCube.AddFace(color5Faces, -Vector3.up, Color5);
                        }
                        if (k == size - 1)
                        {
                            newCube.AddFace(color6Faces, -Vector3.forward, Color6);
                        }

                        visibleCubes.Add(newCube);
                    }
                }
            }
        }
    }

    public void ReLaunch() // Clear all the lists and recreate the cube
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

    public void Restart() // restart the game
    {
        ReLaunch();
        Shuffle();
        DisplayVictory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.R))
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

    public void QuitGame()// Used in UI
    {
        Application.Quit();
    }

    public void RotateLineAroundAxis(Vector3 axis, float height, float factor, float oldFactor, float direction = 1.0f) // Rotate a line of 
    {
        Quaternion Start = transform.rotation;
        Quaternion End = Quaternion.AngleAxis(90 * direction, axis) * Start;

        Debug.Log("Rotate Line : Rotation Axis = " + axis);
        Debug.Log("Rotate Line : Line Height = " + height);

        for (int i = 0; i < visibleCubes.Count; i++)
        {
            float localHeight = Vector3.Dot(visibleCubes[i].transform.GetChild(0).position, axis);

            if (Mathf.Abs(height - localHeight) < (1.0f / size) / 2.0f) // check if the cube is in the selected line
            {
                Quaternion reset = Quaternion.Slerp(Start, End, oldFactor);
                reset.x *= -1;
                reset.y *= -1;
                reset.z *= -1;

                visibleCubes[i].transform.rotation = reset * visibleCubes[i].transform.rotation; // reset the cube to his old rotation
                visibleCubes[i].transform.rotation = Quaternion.Slerp(Start, End, factor) * visibleCubes[i].transform.rotation;
            }
        }

        DisplayVictory(); // Check if the RubicksCube is complete
    }

    void QuickRotateLineAroundAxis(Vector3 axis, float height) // Only used for Shuffle. 
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
        DisplayVictory(); // Check if the RubicksCube is complete
    }

    public IEnumerator RotateLineAroundAxisTime(Vector3 axis, float height, float duration, float direction = 1.0f, float startFactor = 0.0f)
    {
        if (rotate)
        {
            yield break;
        }

        rotate = true;

        float actualTime = 0;
        float lastFrame;
        float lastTime   = Time.time;

        ShowMovingFace(axis, height, true);

        while (duration * (1 - startFactor) - actualTime > 0)
        {
            lastFrame = actualTime;
            actualTime += Time.time - lastTime;
            lastTime = Time.time;

            RotateLineAroundAxis(axis, height, (actualTime / duration) + startFactor, (lastFrame / duration) + startFactor, direction);

            yield return new WaitForEndOfFrame();

        }

        ShowMovingFace(axis, height, false);
        rotate = false;

        yield break;
    }

    public void ShowMovingFace(Vector3 axis, float height, bool show) // Make the hidden face of the line visible or invisible
    {                                                                 // Used when rotating a line
        for (int i = 0; i < visibleCubes.Count; i++)
        {
            float localHeight = Vector3.Dot(visibleCubes[i].transform.GetChild(0).position, axis);

            if (Mathf.Abs(height - localHeight) < (1.0f / size) * 2.0f)
            {
                visibleCubes[i].Optimize(!show);
            }
        }
    }

    void DisplayVictory()
    {
        if (victoryMessage)
        {
            victoryMessage.UpdateDisplay(CheckVictory());
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


    /* ==== Save Functions ==== */ 

    public void OnDestroy() // Save the RubicksCube at the end of the game
    {
        Save();
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
        else
        {
            Restart();
        }

        DisplayVictory();
    }

}
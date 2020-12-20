using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuaternionSave // Quaternion aren't serizable. This class is used to save the values
{
    public float w;
    public float x;
    public float y;
    public float z;

    public QuaternionSave(Quaternion _quat)
    {
        w = _quat.w;
        x = _quat.x;
        y = _quat.y;
        z = _quat.z;
    }

    public Quaternion GetQuaternion() 
    {
        return new Quaternion(x, y, z, w);
    }
};

[System.Serializable]
public class SaveData
{
    public uint size;
    QuaternionSave rubicksRotation;
    public List<QuaternionSave> list;

    public SaveData(Rubickscube rubick)
    {
        size = rubick.size;

        list = new List<QuaternionSave>();

        rubicksRotation = new QuaternionSave(rubick.transform.rotation);

        Quaternion InvertRubick = rubick.transform.rotation; // The rotation of the RubicksCube is removed to avoid conflict at the RubicksCube creation

        InvertRubick.x *= -1;
        InvertRubick.y *= -1;
        InvertRubick.z *= -1;

        for (int i = 0; i < rubick.visibleCubes.Count; i++) // Save the quaternion of all cube.
        {
            Quaternion saveQuaternion = InvertRubick * rubick.visibleCubes[i].transform.rotation;

            list.Add(new QuaternionSave(saveQuaternion));
        }
    }

    public void Load(Rubickscube rubick)
    {
        rubick.size = size;

        rubick.ReLaunch();

        for (int i = 0; i < rubick.visibleCubes.Count; i++) // Apply the quaternion to all the cube to recreate the save Cube
        {
            rubick.visibleCubes[i].transform.rotation = list[i].GetQuaternion();
        }

        rubick.transform.rotation = rubicksRotation.GetQuaternion();
    }
}

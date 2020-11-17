using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCube : MonoBehaviour
{
    public uint i;
    public uint j;
    public uint k;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPositionOnCube(uint I, uint J, uint K)
    {
        i = I;
        j = J;
        k = K;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayForward : MonoBehaviour
{
    [SerializeField] Vector3 forward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        forward = transform.forward;
    }
}

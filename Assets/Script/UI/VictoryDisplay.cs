using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryDisplay : MonoBehaviour
{
   // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdateDisplay(bool active)
    {
        gameObject.SetActive(active);
    }
}

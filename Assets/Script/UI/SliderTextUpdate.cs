using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextUpdate : MonoBehaviour
{
    [SerializeField] Text textComponent = null;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetText()
    {
        textComponent.text = slider.value.ToString();
    }

}

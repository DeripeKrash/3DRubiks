using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkSizeValueToSlider : MonoBehaviour
{
    Slider slider;

    [SerializeField] Text textComponent = null;
    [SerializeField] Rubickscube rubicks = null;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = rubicks.size;
        textComponent.text = slider.value.ToString();
    }

    public void Set()
    {
        slider.value = rubicks.size;
        textComponent.text = slider.value.ToString();
    }

    public void SetText()
    {
        textComponent.text = slider.value.ToString();
    }
}

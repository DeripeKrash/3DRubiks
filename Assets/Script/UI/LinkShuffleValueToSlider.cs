using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkShuffleValueToSlider : MonoBehaviour
{
    Slider slider;

    [SerializeField] Text textComponent = null;
    [SerializeField] Rubickscube rubicks = null;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = rubicks.shuffleNumber;
        textComponent.text = slider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText()
    {
        textComponent.text = slider.value.ToString();
    }
}

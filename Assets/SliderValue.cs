using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValue : MonoBehaviour
{
    private Slider slider;
    // public Text textField;

    public TextMeshProUGUI textField;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    void Update()
    {
        float value = slider.value * 0.5f;
        textField.text = "×" + value.ToString();
    }
}
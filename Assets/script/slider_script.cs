using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider_script : MonoBehaviour
{
    private Slider Slider_g;
    public Text gText;

    // Start is called before the first frame update
    void Start()
    {
        Slider_g = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = Slider_g.value * 10;
        gText.text = (Slider_g.value * 10).ToString().Length >= 3
            ? (Slider_g.value * 10).ToString().Substring(0, 3) + " x"
            : (Slider_g.value * 10).ToString() + " x";
    }
}
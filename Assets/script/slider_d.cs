using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider_d : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider Slider_d;
    public Text dText;
    public Text pText;
    public Text rText;
    public Slider Slider_t;
    public Toggle mask;
    void Start()
    {
        Slider_d = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        dText.text = (Slider_d.value).ToString().Length >= 3
            ? (Slider_d.value).ToString().Substring(0, 3) + " m"
            : (Slider_d.value).ToString() + " m";
        
        var p = combine_distribution(Slider_d.value, Slider_t.value);
        pText.text = p.ToString();
        
        // Let's say mask have 15% protection rate, 2 % / 15 % = 13 %
        rText.text = mask.isOn ? (2 * p).ToString().Substring(0,3) + " %" : (13 * p).ToString().Substring(0,3) + " %";
    }
    
    double combine_distribution(double x, double y)
    {
        return LogSigmoid(x * -2 + 5) * exponential_cdf(y);
    }

    double LogSigmoid(double x)
    {
        if (x < -45.0) return 0.0;
        if (x > 45.0) return 1.0;
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    double exponential_cdf(double x)
    {
        return 1.0 - Math.Exp(-x);
    }
}
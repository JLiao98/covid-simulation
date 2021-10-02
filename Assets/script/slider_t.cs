using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider_t : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider Slider_t;
    public Text tText;
    public Text pText;
    public Text rText;
    public Slider Slider_d;
    public Toggle mask;

    void Start()
    {
        Slider_t = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        tText.text = (Slider_t.value).ToString().Length >= 3
            ? (Slider_t.value).ToString().Substring(0, 3) + " min"
            : (Slider_t.value).ToString() + " min";

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
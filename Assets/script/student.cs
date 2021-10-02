using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class student : MonoBehaviour
{
    public GameObject text;
    public GameObject t_ST;
    public GameObject t_MS;
    public GameObject t_ENGG;
    public GameObject t_ICT;
    public GameObject t_machall;
    public GameObject t_qurantine;
    public GameObject healthy;
    public GameObject incubation;
    public GameObject infectious;
    public GameObject confirmed;
    public GameObject vaccinated;
    public GameObject if_qurantine;
    public GameObject distance;
    public GameObject contact_time;
    public GameObject if_mask;
    
    private GameObject t_class;
    private NavMeshAgent agent;
    private Vector3 startPoint;

    public int infected = 0; //0 Healthy 1 Incubation 2 Infectious 3 Confirmed 4 Vaccinated


    private int window_period = 999;
    private int infectedDay = 999;


    Color orange = new Color(255f / 255f, 112f / 255f, 0f / 255f);

    private Text mText;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mText = text.GetComponent<Text>();
        startPoint = gameObject.transform.position;
    }


    private int state = 0;

    private int count = 10 * 60;

    private int day = 0;


    void FixedUpdate()
    {
        // T-2 day is infectious
        if (window_period + infectedDay - 2 <= day && infected == 1)
        {
            infected = 2;
            incubation.GetComponent<Text>().text = (int.Parse(incubation.GetComponent<Text>().text) - 1).ToString();
            infectious.GetComponent<Text>().text = (int.Parse(infectious.GetComponent<Text>().text) + 1).ToString();
        }

        if (window_period + infectedDay <= day && infected == 2)
        {
            infected = 3;
            infectious.GetComponent<Text>().text = (int.Parse(infectious.GetComponent<Text>().text) - 1).ToString();
            confirmed.GetComponent<Text>().text = (int.Parse(confirmed.GetComponent<Text>().text) + 1).ToString();
        }

        switch (infected)
        {
            case 1:
                this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 2:
                this.gameObject.GetComponent<Renderer>().material.color = orange;
                break;
            case 3:
                this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 4:
                this.gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
        }


        count++;

        if (count > 10 * 60)
        {
            var i = Random.Range(0, 4);
            switch (i)
            {
                case 0:
                    t_class = t_ST;
                    break;
                case 1:
                    t_class = t_MS;
                    break;
                case 2:
                    t_class = t_ENGG;
                    break;
                case 3:
                    t_class = t_ICT;
                    break;
            }

            count = 0;

            switch (state)
            {
                case 0:
                    day++;
                    mText.text = day.ToString();
                    agent.enabled = true;
                    state = 1;
                    agent.destination = t_class.transform.position;
                    break;
                case 1:
                    agent.enabled = true;
                    state = 2;
                    agent.destination = t_machall.transform.position;
                    break;
                case 2:
                    agent.enabled = true;
                    state = 0;
                    agent.destination = startPoint;
                    break;
            }
            
            if (infected == 3 && if_qurantine.GetComponent<Toggle>().isOn)
            {
                agent.destination = t_qurantine.transform.position;
            }
        }
    }

    //private int cnt = 0;
    private void OnCollisionEnter(Collision collision)
    {
        // Tested and there are around 1000 interactions per day per sphere

        // cnt++;
        //
        // if (cnt > int.Parse(vaccinated.GetComponent<Text>().text))
        // {
        //     vaccinated.GetComponent<Text>().text = cnt.ToString();
        // }

        if (collision.collider.tag == "stu")
        {
            double combine_prob = combine_distribution(distance.GetComponent<Slider>().value,
                contact_time.GetComponent<Slider>().value);

            if (combine_prob != 0)
            {
                bool mask = if_mask.GetComponent<Toggle>().isOn;
                switch (infected)
                {
                    case 2:
                        Random.InitState((int) DateTime.Now.Ticks);
                        if (collision.collider.GetComponent<student>().infected == 0)
                        {
                            var k = (int) Math.Ceiling(10000 / combine_prob);
                            var i = Random.Range(0, k);
                            if (i <= (mask ? 45 : 130))
                            {
                                collision.gameObject.SendMessage("infect");
                            }
                        }
                        else if (collision.collider.GetComponent<student>().infected == 4)
                        {
                            var k = (int) Math.Ceiling(10000 / combine_prob);
                            var i = Random.Range(0, k);
                            if (mask ? i == 0 : i <= 6)
                            {
                                collision.gameObject.SendMessage("infect");
                            }
                        }

                        break;
                    case 3:
                        Random.InitState((int) DateTime.Now.Ticks);
                        if (collision.collider.GetComponent<student>().infected == 0)
                        {
                            var k = (int) Math.Ceiling(10000 / combine_prob);
                            var i = Random.Range(0, k);
                            if (i <= (mask ? 110 : 330))
                            {
                                collision.gameObject.SendMessage("infect");
                            }
                        }
                        else if (collision.collider.GetComponent<student>().infected == 4)
                        {
                            var k = (int) Math.Ceiling(10000 / combine_prob);
                            var i = Random.Range(0, k);
                            if (i <= (mask ? 3 : 13))
                            {
                                collision.gameObject.SendMessage("infect");
                            }
                        }

                        break;
                }
            }
        }
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

    void infect()
    {
        if (infected == 0)
        {
            infectedDay = day;
            Random.InitState((int) DateTime.Now.Ticks);
            window_period = Random.Range(3, 7);
            infected = 1;
            healthy.GetComponent<Text>().text = (int.Parse(healthy.GetComponent<Text>().text) - 1).ToString();
            incubation.GetComponent<Text>().text = (int.Parse(incubation.GetComponent<Text>().text) + 1).ToString();
        }
        else if (infected == 4)
        {
            infectedDay = day;
            Random.InitState((int) DateTime.Now.Ticks);
            window_period = Random.Range(3, 7);
            infected = 1;
            vaccinated.GetComponent<Text>().text = (int.Parse(vaccinated.GetComponent<Text>().text) - 1).ToString();
            incubation.GetComponent<Text>().text = (int.Parse(incubation.GetComponent<Text>().text) + 1).ToString();
        }
    }

    void infectious_func()
    {
        if (infected == 0)
        {
            infectedDay = day;
            window_period = 2;
            infected = 2;
            healthy.GetComponent<Text>().text = (int.Parse(healthy.GetComponent<Text>().text) - 1).ToString();
            infectious.GetComponent<Text>().text = (int.Parse(infectious.GetComponent<Text>().text) + 1).ToString();
        }
    }
}
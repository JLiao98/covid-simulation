using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class button_vaccinate : MonoBehaviour
{
    public GameObject text;
    public GameObject ST;
    public GameObject ICT;
    public GameObject ENGG;
    public GameObject MS;
    public GameObject MH;
    public GameObject QR;
    public GameObject healthy;
    public GameObject incubation;
    public GameObject infectious;
    public GameObject confirmed;
    public GameObject vaccinated;
    public GameObject if_quarantine;
    public GameObject distance;
    public GameObject contact_time;
    public GameObject if_mask;
    
    Color orange = new Color(255f / 255f, 112f / 255f, 0f / 255f);

    private List<GameObject> _students = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Vaccinate);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                GameObject sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphereObject.transform.localScale = new Vector3((float) 0.4, (float) 0.4, (float) 0.4);
                sphereObject.transform.position = new Vector3((float) j / 50, 0, (float) -10.5 + i);
                sphereObject.AddComponent<Rigidbody>();
                sphereObject.AddComponent<SphereCollider>();
                sphereObject.AddComponent<NavMeshAgent>();
                sphereObject.GetComponent<Renderer>().material.color = Color.cyan;
                sphereObject.GetComponent<Rigidbody>().drag = 999;
                sphereObject.GetComponent<SphereCollider>().material = new PhysicMaterial();
                sphereObject.GetComponent<SphereCollider>().material.bounciness = 0;
                sphereObject.GetComponent<SphereCollider>().material.dynamicFriction = 1;
                sphereObject.GetComponent<SphereCollider>().material.staticFriction = 1;
                sphereObject.GetComponent<SphereCollider>().material.frictionCombine = PhysicMaterialCombine.Maximum;
                sphereObject.GetComponent<SphereCollider>().material.bounceCombine = PhysicMaterialCombine.Maximum;
                sphereObject.GetComponent<SphereCollider>().tag = "stu";
                healthy.GetComponent<Text>().text = (int.Parse(healthy.GetComponent<Text>().text) + 1).ToString();
                sphereObject.AddComponent<student>();
                sphereObject.GetComponent<student>().text = text;
                sphereObject.GetComponent<student>().t_ST = ST;
                sphereObject.GetComponent<student>().t_MS = MS;
                sphereObject.GetComponent<student>().t_ENGG = ENGG;
                sphereObject.GetComponent<student>().t_ICT = ICT;
                sphereObject.GetComponent<student>().t_machall = MH;
                sphereObject.GetComponent<student>().t_qurantine = QR;
                sphereObject.GetComponent<student>().if_qurantine = if_quarantine;
                sphereObject.GetComponent<student>().healthy = healthy;
                sphereObject.GetComponent<student>().incubation = incubation;
                sphereObject.GetComponent<student>().infectious = infectious;
                sphereObject.GetComponent<student>().confirmed = confirmed;
                sphereObject.GetComponent<student>().vaccinated = vaccinated;
                sphereObject.GetComponent<student>().distance = distance;
                sphereObject.GetComponent<student>().contact_time = contact_time;
                sphereObject.GetComponent<student>().if_mask = if_mask;
                _students.Add(sphereObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray01 = new Ray(Camera.main.transform.position, Vector3.forward);
            RaycastHit hit;
            bool isCollider = Physics.Raycast(ray, out hit);
            //bool isCollider01= Physics.Raycast(Camera.main.transform.position, Vector3.forward, 
            if (isCollider)
            {
                if (hit.collider.gameObject.tag == "stu")
                {
                    hit.collider.gameObject.GetComponent<student>().SendMessage("infectious_func");
                }
            }
        }
    }


    void Vaccinate()
    {
        Random.InitState((int) DateTime.Now.Ticks);
        for (int i = 0; i < _students.Count * 0.1; i++)
        {
            GameObject student = _students[Random.Range(0, _students.Count)];
            if (student.GetComponent<student>().infected == 0)
            {
                student.GetComponent<student>().infected = 4;
                healthy.GetComponent<Text>().text = (int.Parse(healthy.GetComponent<Text>().text) - 1).ToString();
                vaccinated.GetComponent<Text>().text =
                    (int.Parse(vaccinated.GetComponent<Text>().text) + 1).ToString();
            }
        }
    }
}
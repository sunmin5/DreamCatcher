using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParkPlayer : PlayerMove
{
    [SerializeField] GameObject startPos;
    [SerializeField] GameObject barrier;

    [SerializeField] float pushSpeed;
    [SerializeField] float maxDistance;

    int curEnergy = 0;
    [SerializeField] int plusEnergy;
    public int energy;
    [SerializeField] int maxEnergy;

    float delayTime = 0;

    public bool isDreamCatcher = false;

    public bool isBarrier = false;

    GameObject sun;

    void Start()
    {
        sun = FindObjectOfType<SunMove>().gameObject;
        cc = GetComponent<CharacterController>();
        TextObject.instance.energyText.text = energy.ToString() + " / " + maxEnergy.ToString();
    }

    void Update()
    {
        Move();
        delayTime += Time.deltaTime;
        ProtectBarrier();
        Push();
        curEnergy = energy;
        TextObject.instance.energyText.text = energy.ToString() + " / " + maxEnergy.ToString();
        TextObject.instance.ParkTextSign();
    }

    void Push()
    {
        if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, 1 << 7))
            {
                if (energy > 0)
                {
                    hit.transform.gameObject.transform.position = Vector3.MoveTowards(hit.transform.gameObject.transform.position, startPos.transform.position, pushSpeed * Time.deltaTime);

                    if (delayTime > 1)
                    {
                        energy -= 5;
                        curEnergy = energy;
                        delayTime = 0;
                    }
                }
                else TextObject.instance.enoughText.enabled = true;
            }
            else return;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One)) TextObject.instance.enoughText.enabled = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Potion")
        {
            energy += plusEnergy;

            if (maxEnergy - curEnergy <= plusEnergy) energy += maxEnergy - energy;
            Destroy(other.gameObject);
        }
        else if (other.tag == "DreamCatcher")
        {
            isDreamCatcher = true;
            other.gameObject.SetActive(false);
            TextObject.instance.overText.enabled = true;
            sun.SetActive(false);
            GameManager.instacne.parkClear = true;
            GameManager.instacne.dreamCatcherCount++;
        }
    }

    void ProtectBarrier()
    {
        //if (Input.GetMouseButtonDown(1))
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            if (energy >= 50)
            {
                energy = 0;
                isBarrier = true;
                barrier.SetActive(true);
                GameObject itemSpawner = FindObjectOfType<ItemSpawner>().gameObject;
                itemSpawner.SetActive(false);
            }
            else StartCoroutine(TextObject.instance.EnoughEnergy());
        }
    }
}
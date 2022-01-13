using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextObject : MonoBehaviour
{
    public static TextObject instance;

    public Text enoughText;
    public Text protectText;
    public Text overText;
    public Text failText;
    public Text energyText;
    public Text timer;
    public Text hpText;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            enoughText = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            protectText = gameObject.transform.GetChild(1).GetChild(1).GetComponent<Text>();
            timer = gameObject.transform.GetChild(1).GetChild(2).GetComponent<Text>();
            overText = gameObject.transform.GetChild(1).GetChild(3).GetComponent<Text>();
            failText = gameObject.transform.GetChild(1).GetChild(4).GetComponent<Text>();
            energyText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            hpText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            overText = gameObject.transform.GetChild(1).GetComponent<Text>();
            failText = gameObject.transform.GetChild(2).GetComponent<Text>();
        }
    }

    public IEnumerator EnoughEnergy()
    {
        enoughText.enabled = true;
        yield return new WaitForSeconds(1f);
        enoughText.enabled = false;
    }

    public void ParkTextSign()
    {
        transform.GetChild(0).transform.position = ARAVRInput.RHand.position + Vector3.up * 0.025f;
        transform.GetChild(0).transform.rotation = ARAVRInput.RHand.rotation;

        enoughText.transform.position = ARAVRInput.RHand.position + Vector3.up * 0.12f;
        enoughText.transform.rotation = ARAVRInput.RHand.rotation;

        timer.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        timer.transform.rotation = Camera.main.transform.rotation;

        protectText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f + Camera.main.transform.up * 0.15f;
        protectText.transform.rotation = Camera.main.transform.rotation;

        overText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        overText.transform.rotation = Camera.main.transform.rotation;

        failText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        failText.transform.rotation = Camera.main.transform.rotation;
    }
    
    public void RoofTextSign()
    {
        transform.GetChild(0).transform.position = ARAVRInput.RHand.position + Vector3.up * 0.025f;
        transform.GetChild(0).transform.rotation = ARAVRInput.RHand.rotation;

        overText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        overText.transform.rotation = Camera.main.transform.rotation;

        failText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        failText.transform.rotation = Camera.main.transform.rotation;
    }
}
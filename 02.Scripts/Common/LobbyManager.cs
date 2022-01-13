using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    Button parkGo;
    Button roofGo;
    Button skill;
    Button up;
    Button parkManual;
    Button roofManual;
    Button exit;

    TextMeshProUGUI quantity;
    TextMeshProUGUI upGrade;

    bool skillONOFF = false;
    bool parkManualONOFF = false;
    bool roofManualONOFF = false;

    void Start()
    {
        parkGo = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        roofGo = transform.GetChild(1).GetChild(1).GetComponent<Button>();
        parkManual = transform.GetChild(2).GetComponent<Button>();
        roofManual = transform.GetChild(3).GetComponent<Button>();
        skill = transform.GetChild(4).GetComponent<Button>();
        exit = transform.GetChild(6).GetComponent<Button>();
        up = transform.GetChild(4).GetChild(1).GetChild(3).GetComponent<Button>();
        quantity = transform.GetChild(4).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        upGrade = transform.GetChild(4).GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();

        quantity.text = "X  " + GameManager.instacne.dreamCatcherCount.ToString();

        parkGo.onClick.AddListener(() => SceneManager.LoadScene(1));

        if (GameManager.instacne.parkClear)
        {
            roofGo.transform.GetChild(0).GetComponent<Text>().text = "GO";
            roofGo.onClick.AddListener(() => SceneManager.LoadScene(2));
        }
        else roofGo.onClick.AddListener(() => StartCoroutine(Lock()));

        skill.onClick.AddListener(() =>
        {
            skillONOFF = !skillONOFF;
            skill.transform.GetChild(1).gameObject.SetActive(skillONOFF);
        }
        );

        parkManual.onClick.AddListener(() =>
        {
            parkManualONOFF = !parkManualONOFF;
            parkManual.transform.GetChild(1).gameObject.SetActive(parkManualONOFF);
        }
        );

        roofManual.onClick.AddListener(() =>
        {
            roofManualONOFF = !roofManualONOFF;
            roofManual.transform.GetChild(1).gameObject.SetActive(roofManualONOFF);
        }
        );

        up.onClick.AddListener(() =>
        {
            if (GameManager.instacne.dreamCatcherCount > 0)
            {
                if (GameManager.instacne.speedUp < GameManager.instacne.speedUpMax)
                {
                    GameManager.instacne.speedUp++;
                    GameManager.instacne.dreamCatcherCount--;
                }
            }
            else return;
        }
        );

        exit.onClick.AddListener(() =>
        {
            exit.transform.GetChild(1).gameObject.SetActive(true);
            exit.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => exit.transform.GetChild(1).gameObject.SetActive(false));
            exit.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Application.Quit());    
        }
        );

        if (GameManager.instacne.parkClear) transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().enabled = true;
        if (GameManager.instacne.roofClear) transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().enabled = true;
    }

    IEnumerator Lock()
    {
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(1);
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().enabled = false;
    }

    void Update()
    {
        if (GameManager.instacne.speedUp == GameManager.instacne.speedUpMax)
        {
            upGrade.text = "MAX";
            upGrade.color = Color.red;
        }
        else upGrade.text = "+" + GameManager.instacne.speedUp.ToString();

        quantity.text = "X  " + GameManager.instacne.dreamCatcherCount.ToString();
    }
}
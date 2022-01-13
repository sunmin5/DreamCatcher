using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamCatcher : MonoBehaviour
{
    [SerializeField] GameObject dreamCatcher;

    float timer = 11;

    SunMove sun;

    void Start()
    {
        sun = FindObjectOfType<SunMove>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        TextObject.instance.timer.text = ((int)timer).ToString();

        if (timer <= 0)
        {
            TextObject.instance.timer.enabled = false;
            dreamCatcher.SetActive(true);
            sun.speed = 5;
            gameObject.SetActive(false);
        }
    }
}
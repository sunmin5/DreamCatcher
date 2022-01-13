using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (TextObject.instance.overText.enabled || TextObject.instance.failText.enabled)
            {
                TextObject.instance.timer.enabled = false;
                StartCoroutine(EndGame());
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (TextObject.instance.overText.enabled || TextObject.instance.failText.enabled) StartCoroutine(EndGame());
        }

    }

    IEnumerator EndGame() 
    {
        yield return new WaitForSeconds(3.6f);
        SceneManager.LoadScene(0);
    }
}
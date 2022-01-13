using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instacne;

    public bool parkClear = false;

    public bool roofClear = false;

    public int dreamCatcherCount = 0;
    public int speedUp = 0;
    public int speedUpMax = 5;
    public int acceleration = 0;

    void Awake()
    { 
        if (instacne == null)
        {
            instacne = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
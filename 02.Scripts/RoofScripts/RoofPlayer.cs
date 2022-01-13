using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoofPlayer : PlayerMove
{
    public int hp;
    int startHp = 3;

    public GameObject note;
    public GameObject dreamCatcher;

    public int noteCount = 0;

    public bool setDreamCatcher = false;

    float attackDelay = 1.4f;

    public bool isDead = false;
    public bool safe = false;

    void Start()
    {
        hp = startHp;
        TextObject.instance.hpText.text = "X " + hp.ToString();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        TextObject.instance.RoofTextSign();
    }    

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Note")
        {
            noteCount++;
            if (noteCount == 2) note.SetActive(true);
        }
        else if (other.name == "NoteDoubleGold")
        {
            setDreamCatcher = true;
            dreamCatcher.SetActive(true);
        }
        else if (other.tag == "DreamCatcher")
        {
            other.gameObject.SetActive(false);
            TextObject.instance.overText.enabled = true;
            EnemySpawn.FindObjectOfType<EnemySpawn>().gameObject.SetActive(false);
            GameManager.instacne.roofClear = true;
            GameManager.instacne.dreamCatcherCount++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "SafeZone") safe = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "SafeZone") safe = false;
    }

    public IEnumerator OnDamage(int enemyDamage)
    {
        yield return new WaitForSeconds(0.8f);
        if (hp > 0) hp -= enemyDamage;
        TextObject.instance.hpText.text = "X " + hp.ToString();
        Die();
        ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
        ARAVRInput.PlayVibration(ARAVRInput.Controller.LTouch);
        GameManager.instacne.acceleration = 1;
        yield return new WaitForSeconds(attackDelay);
        GameManager.instacne.acceleration = 0;
    }

    void Die()
    {
        if (hp <= 0)
        {
            TextObject.instance.failText.enabled = true;
            speed = 0;
            isDead = true;
            //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), false);
        }
    }
}
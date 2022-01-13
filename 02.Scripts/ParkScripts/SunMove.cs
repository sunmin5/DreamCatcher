using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SunMove : MonoBehaviour
{
    [SerializeField] GameObject targetPos;

    public float speed = 5;

    Rigidbody rg;

    ParkPlayer parkPlayer;


    void Start()
    {
        rg = GetComponent<Rigidbody>();
        parkPlayer = FindObjectOfType<ParkPlayer>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos.transform.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("RealGround"))
        {
            collision.gameObject.SetActive(false);
            TextObject.instance.failText.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Barrier")
        {
            speed = 0;
            TextObject.instance.protectText.enabled = false;
        }
        else if (other.tag == "Sign") TextObject.instance.protectText.enabled = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Barrier")
        {
            TextObject.instance.protectText.enabled = false;
        }
        else if (other.tag == "Sign")
        {
            if (!parkPlayer.isBarrier) ARAVRInput.PlayVibration(ARAVRInput.Controller.LTouch);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sign") TextObject.instance.protectText.enabled = false;
    }
}
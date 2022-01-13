using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    protected CharacterController cc;

    protected Vector3 dir;

    protected float gravity = -5f;
    protected float h, v;
    protected float yVelocity;
    protected float speed;

    public void Move()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        h = ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.RTouch);
        v = ARAVRInput.GetAxis("Vertical", ARAVRInput.Controller.RTouch);
        
        dir = new Vector3(h, 0, v);

        yVelocity += gravity * Time.deltaTime;
        
        if (cc.isGrounded)
        {
            yVelocity = 0;

            if (ARAVRInput.GetDown(ARAVRInput.Button.Two))
            {
                yVelocity = 2.5f;
            }
        }

        dir.y = yVelocity;

        dir = Camera.main.transform.TransformDirection(dir);
        
        speed = 0.6f + GameManager.instacne.speedUp * 0.1f + GameManager.instacne.acceleration;
        cc.Move(dir * speed * Time.deltaTime);

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch)) ARAVRInput.Recenter(transform, Vector3.back);
    }
}

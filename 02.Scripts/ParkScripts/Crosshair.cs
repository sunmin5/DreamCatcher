using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Transform crossHair;
    [SerializeField] Image cHImage;

    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject particleAngle;

    void Update()
    {
        ARAVRInput.DrawCrosshair(crossHair);

        Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1 << 7))
        {
            cHImage.color = Color.red;

            //if (Input.GetMouseButtonDown(0))
            if(ARAVRInput.GetDown(ARAVRInput.Button.One))
            {
                particle.Play();
            }
            //else if (Input.GetMouseButton(0))
            else if (ARAVRInput.Get(ARAVRInput.Button.One))
            {
                crossHair.localScale = crossHair.localScale * 0.7f;

                particleAngle.transform.position = hit.point;
                particleAngle.transform.rotation = Quaternion.LookRotation(transform.position - hit.point) * Quaternion.AngleAxis(90, Vector3.right);
            }
            //else if (Input.GetMouseButtonUp(0))
            else if (ARAVRInput.GetUp(ARAVRInput.Button.One))
            {
                particle.Stop();
            }
            
        }
        else cHImage.color = Color.white;
    }
}
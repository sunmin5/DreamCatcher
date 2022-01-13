using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainCross : MonoBehaviour
{
    [SerializeField] Transform cross;
    [SerializeField] Image crossImage;

    void Update()
    {
        ARAVRInput.DrawCrosshair(cross);

        Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << 5))
        {
            crossImage.color = Color.red;
            if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch)) ExecuteEvents.Execute(hit.transform.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
        else crossImage.color = Color.white;
    }
}
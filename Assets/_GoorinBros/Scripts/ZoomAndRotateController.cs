using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomAndRotateController : MonoBehaviour
{
    public GameObject selectedObject;
    
    [HideInInspector]
    public static bool IsScale;

    private bool zoomActivated;
    
    void Update()
    {

        if (zoomActivated)
        {

            if (selectedObject != null)
            {
                ZoomPlayer();
                // RotationPlayer();

            }
        }

    }

    public void SetTexture(Sprite img)
    {
        selectedObject.GetComponent<Image>().sprite = img;
        zoomActivated = true;
    }

    public void OnReset()
    {
        selectedObject.transform.localEulerAngles = Vector3.one;
        zoomActivated = false;
    }

    public void ZoomPlayer()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;



            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float Scale = -deltaMagnitudeDiff * 0.01f;
            selectedObject.transform.localScale += new Vector3(Scale, Scale, Scale);
            selectedObject.transform.localScale = new Vector3(Mathf.Clamp(selectedObject.transform.localScale.x, 1f, 2.5f), Mathf.Clamp(selectedObject.transform.localScale.y, 1f, 2.5f), Mathf.Clamp(selectedObject.transform.localScale.z, 1f, 2.5f));
            IsScale = true;
        }
        else
        {
            IsScale = false;
        }
    }


#if UNITY_EDITOR
    private Vector3 previousMousePos;
#endif

    public void RotationPlayer()
    {
        if (IsScale == false)
        {
#if UNITY_EDITOR //codigo para poder rotar el carro con el mouse ... solo funciona dentro de editor
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosDelta = Input.mousePosition - previousMousePos;
                selectedObject.transform.Rotate(0, -0.3f * mousePosDelta.x, 0);
            }
            previousMousePos = Input.mousePosition;
#endif

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {

                selectedObject.transform.Rotate(0, -0.3f * Input.GetTouch(0).deltaPosition.x, 0);
            }
        }
    }
}

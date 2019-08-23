using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomAndRotateController : MonoBehaviour
{
    public GameObject selectedObject;
    
   
    public  bool IsScale;

    public bool zoomActivated;

    [Header("Movement")]
    public Vector3 direction;
    public Vector2 StartPosition;

    [Header("Clamping")]
    public float scale;
    public Vector2 ClampPosition;

    void FixedUpdate()
    {

        //MovementPlayer();
      //  ZoomPlayer();


        if (zoomActivated)
        {

            if (selectedObject != null)
            {
                ZoomPlayer();
                MovementPlayer();
                //  RotationPlayer();

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
        selectedObject.transform.localScale = Vector3.one;
        selectedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        zoomActivated = false;
    }


   
    public void MovementPlayer()
    {
        if (IsScale == false)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Began)
                {
                    StartPosition = Input.GetTouch(0).position;
                }

                
                //if (touch.phase == TouchPhase.Moved)
                //{
                //    Debug.Log("GGG");
                //    direction = StartPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //    Vector3 value = direction;

                //    selectedObject.transform.position -= value;

                //    //float PosclampX = Mathf.Clamp(selectedObject.GetComponent<RectTransform>().anchoredPosition.x, -ClampPosition.x, ClampPosition.x);
                //    //float PosclampY = Mathf.Clamp(selectedObject.GetComponent<RectTransform>().anchoredPosition.y, -ClampPosition.y, ClampPosition.y);

                //    //selectedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(PosclampX, PosclampY);

                //}

                if(touch.phase == TouchPhase.Moved)
                {
                    direction = StartPosition - Input.GetTouch(0).position;

                    float distance = Vector3.Distance(StartPosition, Input.GetTouch(0).position);
                    Debug.Log(distance);
                    Vector3 value = direction*0.1f;

                    selectedObject.transform.position -= value;

                    float PosclampX = Mathf.Clamp(selectedObject.GetComponent<RectTransform>().anchoredPosition.x, -ClampPosition.x, ClampPosition.x);
                    float PosclampY = Mathf.Clamp(selectedObject.GetComponent<RectTransform>().anchoredPosition.y, -ClampPosition.y, ClampPosition.y);

                    selectedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(PosclampX, PosclampY);
                }
            }

            
            
        }
    }

    public void ClampMovement()
    {
        if(selectedObject != null)
        {
            scale = selectedObject.transform.localScale.x;

            float ClampX = 315;
            float ClampY = 360;
            float maxScale = 1.5f;

            //Clamping in X
            float resX = scale - 1;
            float clampAspectX = (ClampX * resX) / maxScale;
            //Clamping in Y
            float resY = scale - 1;
            float clampAspectY = (ClampY * resY) / maxScale;

            ClampPosition = new Vector2(clampAspectX, clampAspectY);
        }
    }

    public void ZoomPlayer()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            ClampMovement();

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

                Debug.Log("FFF");
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

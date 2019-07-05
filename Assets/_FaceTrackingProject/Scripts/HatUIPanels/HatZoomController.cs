using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatZoomController : MonoBehaviour
{
    private Vector3 m_MousePosition;
    private bool m_IsMovementActive = false;

    [Header("Zoom")]
    public float m_ZoomFix;
    [Range(0.0f, 1.0f)]
    public float m_Zoom;

    public float m_ZoomSpeed = 1.5f;
    private Vector3 m_MousePos;

    [Header("Hat Reference")]
    public GameObject m_HatPosition;
    public GameObject m_HatHead;
    public GameObject m_HatHead2;
    public GameObject m_HatRotationReference;

    [Header("Front Rotation Limits")]
    [Range(9.0f, 10.0f)]
    public float m_FrontHatMaxRotation;    
    [Range(-5.0f, -2.0f)]
    public float m_FrontHatMinRotation;
    private float m_FrontMovement = 0.0f;

    [Range(5.0f, 8.0f)]
    public float m_YawHatMaxRotation;
    [Range(-5.0f, -8.0f)]
    public float m_YawHatMinRotation;
    private float m_YawMovement = 0.0f;
    private Vector3 m_CurrenteReferenceEulerAnglesX;

    private bool m_HatDectected = false;
    private Vector3 m_CurrentHatScale;
    private Vector3 m_CurrentHatHeadScale;
    private Vector3 m_CurrentHatHeadScale2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HatPosition.transform.childCount > 0 && m_HatDectected == false)
        {
            m_HatDectected = true;

            if (m_HatDectected)
            {
                //m_CurrentHatScale = m_HatPosition.transform.GetChild(0).transform.localScale;
                m_CurrentHatScale = m_HatPosition.transform.localScale;
                m_CurrentHatHeadScale = m_HatHead.transform.localScale;
                m_CurrentHatHeadScale2 = m_HatHead2.transform.localScale;
                m_CurrenteReferenceEulerAnglesX = m_HatRotationReference.transform.localEulerAngles;
                Debug.Log(m_CurrentHatScale);
            }
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            m_MousePosition = Input.mousePosition;
        }

        Dragging();
        
    }

    public void OnStartDrag()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                m_MousePosition = Input.GetTouch(0).position;
                m_IsMovementActive = true;
            }
        }

        if (Input.mousePresent)
        {
            m_MousePosition = Input.mousePosition;
            m_IsMovementActive = true;
        }
    }

    public void Dragging()
    {
        if (Input.touchSupported)
        {            
            int touchCount = Input.touchCount;
            if (touchCount > 0)
            {
                Touch t0 = Input.GetTouch(0);
                if (touchCount == 2)
                {
                    if (m_HatPosition.transform.childCount > 0)
                    {
                        Touch t1 = Input.GetTouch(1);
                        float d1 = (t1.position - t0.position).sqrMagnitude;
                        float d0 = ((t1.position - t1.deltaPosition) - (t0.position - t0.deltaPosition)).sqrMagnitude;
                        m_Zoom = Mathf.Clamp(m_Zoom + ((d1 - d0) * m_ZoomSpeed / d1), -1.0f, 1.0f);

                        //Zoom
                        if (m_HatHead.transform.localScale.x > 0.75f || m_HatHead2.transform.localScale.x > 0.75f ||
                            m_HatHead.transform.localScale.x < 1.25f || m_HatHead2.transform.localScale.x < 1.25f)
                        {
                            m_HatPosition.transform.localScale = m_CurrentHatScale * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                            m_HatHead.transform.localScale = m_CurrentHatHeadScale * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                            m_HatHead2.transform.localScale = m_CurrentHatHeadScale2 * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                        }

                        //Rotation
                        //if (t0.position.x < t1.position.x)
                        //{
                        //    if (t0.deltaPosition.y > 0 && t1.deltaPosition.y < 0 &&
                        //       t0.deltaPosition.x >= 0 && t1.deltaPosition.x <= 0)
                        //    {
                        //        m_HatRotationReference.transform.eulerAngles += new Vector3(0, 0, 1.0f);
                        //    }

                        //    if (t0.deltaPosition.y < 0 && t1.deltaPosition.y > 0 &&
                        //        t0.deltaPosition.x >= 0 && t1.deltaPosition.x <= 0)
                        //    {
                        //        m_HatRotationReference.transform.eulerAngles -= new Vector3(0, 0, 1.0f);
                        //    }
                        //}
                        //else
                        //{
                        //    if (t1.deltaPosition.y > 0 && t0.deltaPosition.y < 0 &&
                        //       t1.deltaPosition.x >= 0 && t0.deltaPosition.x <= 0)
                        //    {
                        //        m_HatRotationReference.transform.eulerAngles += new Vector3(0, 0, 1.0f);
                        //    }

                        //    if (t1.deltaPosition.y < 0 && t0.deltaPosition.y > 0 &&
                        //        t1.deltaPosition.x >= 0 && t0.deltaPosition.x <= 0)
                        //    {
                        //        m_HatRotationReference.transform.eulerAngles -= new Vector3(0, 0, 1.0f);
                        //    }
                        //}
                    }
                }

                if (touchCount == 1)
                {
                    if (t0.deltaPosition.y > 0 && m_FrontMovement > m_FrontHatMinRotation)
                    {
                        m_HatRotationReference.transform.localEulerAngles -= new Vector3(1.0f, 0, 0);
                        m_FrontMovement -= 1.0f;
                    }
                    else if (t0.deltaPosition.y < 0 && m_FrontMovement < m_FrontHatMaxRotation)
                    {
                        m_HatRotationReference.transform.localEulerAngles += new Vector3(1.0f, 0, 0);
                        m_FrontMovement += 1.0f;
                    }

                    if (t0.deltaPosition.x < 0 && m_YawMovement > m_YawHatMinRotation)
                    {
                        m_HatRotationReference.transform.localEulerAngles -= new Vector3(0.0f, 0, 1.0f);
                        m_YawMovement -= 1.0f;
                    }
                    else if (t0.deltaPosition.x > 0 && m_YawMovement < m_YawHatMaxRotation)
                    {
                        m_HatRotationReference.transform.localEulerAngles += new Vector3(0.0f, 0, 1.0f);
                        m_YawMovement += 1.0f;
                    }
                }
            }

            //Rotation


        }

        if (Input.mousePresent )
        {
            if(Input.mouseScrollDelta.y != 0)
            {
                m_Zoom = Mathf.Clamp(m_Zoom + Input.mouseScrollDelta.y * 0.05f * m_ZoomSpeed, -1.0f, 1.0f);
                //if (m_Zoom != 0.0f)
                {
                    if (m_HatPosition.transform.childCount > 0)
                    {
                        //m_HatPosition.transform.GetChild(0).transform.localScale = m_CurrentHatScale * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                        if (m_HatHead.transform.localScale.x > 0.75f || m_HatHead2.transform.localScale.x > 0.75f ||
                           m_HatHead.transform.localScale.x < 1.25f || m_HatHead2.transform.localScale.x < 1.25f)
                        {
                            m_HatPosition.transform.localScale = m_CurrentHatScale * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                            m_HatHead.transform.localScale = m_CurrentHatHeadScale * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                            m_HatHead2.transform.localScale = m_CurrentHatHeadScale2 * (((m_Zoom + 2.0f) / 4.0f) + 0.5f);
                        }

                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                float deltaMouseY = Input.mousePosition.y - m_MousePosition.y;
                float deltaMouseX = Input.mousePosition.x - m_MousePosition.x;

                if (deltaMouseY > 0 && m_FrontMovement > m_FrontHatMinRotation)
                {
                    m_HatRotationReference.transform.localEulerAngles -= new Vector3(1.0f, 0, 0);
                    m_FrontMovement -= 1.0f;
                }
                else if (deltaMouseY < 0 && m_FrontMovement < m_FrontHatMaxRotation)
                {
                    m_HatRotationReference.transform.localEulerAngles += new Vector3(1.0f, 0, 0);
                    m_FrontMovement += 1.0f;
                }

                if (deltaMouseX < 0 && m_YawMovement > m_YawHatMinRotation)
                {
                    m_HatRotationReference.transform.localEulerAngles -= new Vector3(0.0f, 0, 1.0f);
                    m_YawMovement -= 1.0f;
                }
                else if (deltaMouseX > 0 && m_YawMovement < m_YawHatMaxRotation)
                {
                    m_HatRotationReference.transform.localEulerAngles += new Vector3(0.0f, 0, 1.0f);
                    m_YawMovement += 1.0f;
                }

                m_MousePosition = Input.mousePosition;
            }
        }

        
    }

    public void FinishDrag()
    {
        if (m_IsMovementActive == true)
        {
            m_IsMovementActive = false;
        }
    }
}

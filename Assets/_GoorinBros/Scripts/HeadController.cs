using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ULSTrackerForUnity;

public class HeadController : MonoBehaviour
{
    [SerializeField]
    private float followSpeed;
    [SerializeField]
    private Transform Anchor3D;
    [SerializeField]
    private Transform faceTarget;
    [SerializeField]
    FaceTrackerController2 m_faceTrackerController2;

   
  //  public Transform pointerCenter;


    public void Update()
    {
        faceTarget.position = Vector3.Lerp(faceTarget.position, Anchor3D.position, followSpeed);
        faceTarget.rotation = Quaternion.Lerp(faceTarget.rotation, Anchor3D.rotation, followSpeed);


        if (m_faceTrackerController2._marker2d.Count > 0) {

            ////punto medio x
            //float PosX = (m_faceTrackerController2._marker2d[0].transform.position.x + m_faceTrackerController2._marker2d[4].transform.position.x)/2;

            ////punto medio Y
            //float PosY = (m_faceTrackerController2._marker2d[9].transform.position.y + m_faceTrackerController2._marker2d[10].transform.position.y) / 2;

            //pointerCenter.position = new Vector3(PosX, PosY, 0);

            //Distancia Ancho
            float disAn = Vector3.Distance (m_faceTrackerController2._marker2d[0].transform.position , m_faceTrackerController2._marker2d[4].transform.position);
            //Distancia Alto
            float disAl = Vector3.Distance(m_faceTrackerController2._marker2d[9].transform.position, m_faceTrackerController2._marker2d[10].transform.position);

            float d = disAn / disAl;
            Debug.Log(d);
        }


    }

    //private void OnDrawGizmos()
    //{
    //    if (m_faceTrackerController2._marker2d.Count > 0)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(m_faceTrackerController2._marker2d[0].transform.position, m_faceTrackerController2._marker2d[4].transform.position);
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawLine(m_faceTrackerController2._marker2d[9].transform.position, m_faceTrackerController2._marker2d[10].transform.position);
    //    }
    //}
}

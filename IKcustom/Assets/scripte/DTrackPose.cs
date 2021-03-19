using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTrackPose : MonoBehaviour
{
    public int id;
    //private int count;
    DTrack manager;
    public DTrack.BodyDesc body_desc;
    
 
    void Start()
    {
        manager = FindObjectOfType<DTrack>();
        //count = 0;
        if (manager == null)
        {
            GameObject obj = new GameObject("DTrack manager");
            manager = obj.AddComponent<DTrack>();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //DTrack.BodyDesc body_desc;

        manager.GetBody(id, out body_desc);

        
        /*if (transform.parent != null)
        {
            Transform parent = transform.parent;
            Vector3 localUp = parent.InverseTransformDirection(Vector3.up);
            Vector3 localFor = parent.InverseTransformDirection(Vector3.forward);
            Vector3 localLast = Vector3.Cross(localUp, localFor).normalized;
            localLast = Vector3.ClampMagnitude(localLast, localFor.magnitude);
            Vector3 pos = parent.position;
            if (count % 60 == 0)
            {
                Debug.DrawLine(pos, pos + localUp, Color.green, 1);
                Debug.DrawLine(pos, pos + localFor, Color.blue, 1);
                Debug.DrawLine(pos, pos + localLast, Color.red, 1);
   
            }
        }*/

        //these two lines have to be commented when you playe in mode direct kinematics
        transform.position = body_desc.position;
        transform.rotation = body_desc.orientation;

        //count++;
    }
}

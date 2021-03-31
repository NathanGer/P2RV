using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTrackPose : MonoBehaviour
{
    public int id;
    private int count;
    DTrack manager;
    public DTrack.BodyDesc body_desc;
    public Matrix4x4 p, inv_p; // matrices pour le changement de base spécifique à chaque part

    // Use this for initialization
    void Start()
    {
        manager = FindObjectOfType<DTrack>();
        count = 0;
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

        // ces 2 lignes sont a commenter lorsque vous etes en mode cinemaique direct
       transform.position = body_desc.position;
       transform.rotation = body_desc.orientation;


        count++;
    }
}

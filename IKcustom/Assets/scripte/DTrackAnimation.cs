using UnityEngine;
using System.Collections.Generic;

public class DTrackAnimation : MonoBehaviour
{

    private List<Transform> bones = new List<Transform>(); // liste des bones possédant le script DTrackPose
    //private List<Quaternion> rotationOffsets = new List<Quaternion>(); // liste des quaternions offsets pour chaque bone
    //private List<Vector3> positionOffsets = new List<Vector3>(); // liste des positions offsets pour chaque bone
    private Transform currentChild;
    public int firstFrame = 10; // frame à laquelle on mesure les offsets
    private float eps = 0.0001f;
    private List<Quaternion> rotationStandards = new List<Quaternion>();
    //private List<Vector3> positionStandards = new List<Vector3>();

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Time.frameCount == firstFrame)
        {
            AddChildrenToList(this.gameObject.transform);
        }
        // Parcourt tous les membres du squelette ayant un script DTrackPose
        // Et effectue la mise à jour des positions à chaque frame
        if (Time.frameCount > firstFrame)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                DTrackPose pose = bones[i].GetComponent<DTrackPose>();
                //Quaternion orientation = pose.body_desc.orientation;
                Vector3 position_world = pose.body_desc.position;

                Quaternion quat = rotationStandards[i];

                if (i == 0)
                 {
                    bones[i].position = position_world;
                    bones[i].transform.rotation = pose.body_desc.orientation * quat;

                }
                 else
                 {
                    bones[i].transform.rotation = pose.body_desc.orientation * quat;
                }
                              
            }
        }
    }

    /// <summary>
    /// Recursive function to store all children having a DTrackPose component in the right order
    /// </summary>
    /// <param name="parent">Parent.</param>
    private void AddChildrenToList(Transform parent)
    {
        int n = parent.childCount;
        if (n == 0)
        {
            return;
        }
        for (int i = 0; i < n; i++)
        {
            currentChild = parent.GetChild(i);
            DTrackPose pose = currentChild.GetComponent<DTrackPose>();
            if (pose != null)
            {
                
                bones.Add(currentChild); // on ajoute à la table le transform de l'enfant
               // rotationOffsets.Add(pose.body_desc.orientation); // on ajoute la première rotation envoyée par DTrack : ceci sera notre offset pour la suite
                rotationStandards.Add(currentChild.rotation);
              // positionOffsets.Add(pose.body_desc.position);
               // positionStandards.Add(currentChild.localPosition);// vector3  local position  ce sera pas necessaire je dirais
            }
            AddChildrenToList(currentChild); // on traite les petits enfants
        }
    }


}

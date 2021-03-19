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
       /* p = Matrix4x4.identity;
       if (id == 14)
        { // hand
            p.m00 = 0f;
            p.m11 = 0f;
            p.m22 = 0f;
            p.m01 = Mathf.Sqrt(2f) / 2f;
            p.m02 = -Mathf.Sqrt(2f) / 2f;
            p.m12 = -1f;
            p.m20 = -Mathf.Sqrt(2f) / 2f;
            p.m21 = -Mathf.Sqrt(2f) / 2f;
        }
        if (id == 10)
        { // forearm
            p.m00 = 0f;
            p.m22 = 0f;
            p.m02 = 1f;
            p.m20 = 1f;
        }
        if (id == 2)
        { // upper arm
            p.m22 = -1f;
        }
        
        inv_p = p.inverse;
        */
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

        //this
        transform.position = body_desc.position;
        transform.rotation = body_desc.orientation;


        //à mettre en commentaire

       // transform.position = body_desc.position;
        /*if (transform.parent != null ) {
			Transform parent = transform.parent;
			// quaternion global parent
			Quaternion parentQuat = transform.parent.rotation;
			// transforme en euler angle
			Vector3 eulerParent = new Vector3(parentQuat.eulerAngles.x,parentQuat.eulerAngles.y,parentQuat.eulerAngles.z);
			// transforme en local euler angle 
			Vector3 localEulerParent = parent.InverseTransformPoint(eulerParent);
			// transforme en local quaternion
			Quaternion localParentQuat = Quaternion.Euler(localEulerParent.x,localEulerParent.y,localEulerParent.z);
			// on prend l'inverse
			Quaternion inv = Quaternion.Inverse (localParentQuat);
			// on applique la meme transformation au quaternion global extrait de Dtrack
			// transforme en euler angle
			Vector3 eulerGlobal = new Vector3(body_desc.orientation.eulerAngles.x,body_desc.orientation.eulerAngles.y,body_desc.orientation.eulerAngles.z);
			// transforme en local euler angle
			Vector3 eulerLocal = parent.InverseTransformPoint(eulerGlobal);
			// transforme en local quaternion
			Quaternion localQuat = Quaternion.Euler(eulerLocal.x,eulerLocal.y,eulerLocal.z);
			// set la rotation du current transform
			transform.localRotation = inv * localQuat;

		} else {
			//Debug.DrawLine(transform.parent.position,transform.parent.position + transform.parent.)
			//Debug.Log(transform.parent.position);
			//Debug.Log("Sans parent : " + transform.name);
			transform.rotation = body_desc.orientation; // pas de localRotation car pas de parent
		}
        
        // jusquici

        //============== NOUVELLE METHODE =====================
        /*
		// on applique la position globale (peut-etre a modifier en utilisant les methodes de l'API ??)
		transform.position = body_desc.position;
		// on recupere la matrice de rotation globale associee a la rotation fournie par DTrack
		Matrix4x4 R = body_desc.rotationMatrix;
		// on recupere la matrice de passage pour passer dans la base propre au parent
		if (transform.parent != null) {
			Matrix4x4 M = transform.localToWorldMatrix; // on recupere la matrice monde de la part concernee
			//Matrix4x4 R_local = M.inverse * R * M; // matrice de rotation dans le repere du parent
			//transform.localRotation = Quaternion.Inverse(transform.parent.rotation) * R_local.ExtractQuaternion(); // on applique le quaternion local a la piece concernee
			M = R * M;
			Extensions.SetWorldMatrix(transform, M);
		}
		// on applique la rotation donnee par DTrack dans ce repere propre
		*/

        count++;
    }
}
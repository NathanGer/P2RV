using UnityEngine;
using System.Collections.Generic;

public class DTrackAnimation : MonoBehaviour {

	private List<Transform> bones = new List<Transform>(); // liste des bones possédant le script DTrackPose
	private List<Quaternion> rotationOffsets = new List<Quaternion> (); // liste des quaternions offsets pour chaque bone
	private List<Vector3> positionOffsets = new List<Vector3> (); // liste des positions offsets pour chaque bone
	private Transform currentChild;
	public int firstFrame = 10; // frame à laquelle on mesure les offsets
	private float eps = 0.0001f;
	private List<Quaternion> rotationStandards = new List<Quaternion> (); 

	// Use this for initialization
	void Start () {
		//AddChildrenToList (this.gameObject.transform);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Time.frameCount == firstFrame) {
			AddChildrenToList(this.gameObject.transform);
		}
		// Parcourt tous les membres du squelette ayant un script DTrackPose
		// Et effectue la mise à jour des positions à chaque frame
		if (Time.frameCount > firstFrame) {
			for (int i = 0; i < bones.Count; i++) {
				SetTransform (bones [i],rotationStandards[i], rotationOffsets [i], positionOffsets[i]);
			}
		}
	}

	/// <summary>
	/// Recursive function to store all children having a DTrackPose component in the right order
	/// </summary>
	/// <param name="parent">Parent.</param>
	private void AddChildrenToList(Transform parent) {
		int n = parent.childCount;
		if (n == 0) {
			return;
		}
		for (int i = 0; i < n; i++) {
			currentChild = parent.GetChild (i);
			DTrackPose pose = currentChild.GetComponent<DTrackPose>();
			if (pose != null) {
				bones.Add (currentChild); // on ajoute à la table le transform de l'enfant
				rotationOffsets.Add (pose.body_desc.orientation); // on ajoute la première rotation envoyée par DTrack : ceci sera notre offset pour la suite
				rotationStandards.Add (currentChild.localRotation);
				//Debug.Log(pose.body_desc.orientation);
				positionOffsets.Add (pose.body_desc.position);

			}
			AddChildrenToList(currentChild); // on traite les petits enfants
		}
	}

	private void SetTransform(Transform bone, Quaternion Rstandard, Quaternion Roffset, Vector3 Poffset) {
		// On récupère le composant DTrackPose
		DTrackPose pose = bone.GetComponent<DTrackPose>();
		// On applique le changement de repère spécifique à la pièce traquée
		Matrix4x4 newRotationMatrix = pose.inv_p * pose.body_desc.rotationMatrix * pose.p;
		// nouveau Quaternion
		//Quaternion newOrientation = newRotationMatrix.ExtractQuaternion();
        
        
            Vector3 forward;
            forward.x = newRotationMatrix.m02;
            forward.y = newRotationMatrix.m12;
            forward.z = newRotationMatrix.m22;

            Vector3 upwards;
            upwards.x = newRotationMatrix.m01;
            upwards.y = newRotationMatrix.m11;
            upwards.z = newRotationMatrix.m21;

        Quaternion newOrientation =  Quaternion.LookRotation(forward, upwards);
        
        //Quaternion quat = Quaternion.Inverse (Roffset) * pose.body_desc.orientation;
        Quaternion quat = Quaternion.Inverse (Roffset) * newOrientation;
		//bone.localRotation = Rstandard*quat;
		if (!(Mathf.Abs(quat.x) < eps && Mathf.Abs(quat.y) < eps && Mathf.Abs(quat.z) < eps && Mathf.Abs(quat.w) < eps)) {
			bone.localRotation = Rstandard * Quaternion.Inverse (bone.parent.rotation) * quat;
			//bone.localRotation = Rstandard * quat;
		}
	}
}

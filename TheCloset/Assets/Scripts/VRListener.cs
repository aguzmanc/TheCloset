using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRListener : MonoBehaviour 
{
	public AudioSource TVAudio;

	public float MaxDistance;
	public float MaxVolume;


	public float dist;
	public float p;

	void Update () 
	{
		TVAudio.panStereo = -1f * Vector3.Dot (transform.right, Vector3.forward);

		dist = (transform.position - TVAudio.transform.position).z;

		float p = 1f - dist / MaxDistance;

		TVAudio.volume = p * MaxVolume;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRListener : MonoBehaviour 
{
	AudioSource _src;

	public float MaxDistance;
	public float MaxVolume;

	void Start () 
	{
		_src = GameObject.FindObjectOfType<AudioSource> ();
		
	}

	public float dist;
	public float p;

	void Update () 
	{
		_src.panStereo = -1f * Vector3.Dot (transform.right, Vector3.forward);

		dist = (transform.position - _src.transform.position).z;

		float p = 1f - dist / MaxDistance;

		_src.volume = p * MaxVolume;
	}
}

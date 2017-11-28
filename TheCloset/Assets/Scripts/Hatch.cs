using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour 
{
	public void Start()
	{
		StartCoroutine (StartGas ());
	}


	IEnumerator StartGas()
	{
		yield return new WaitForSeconds (4 * 60); // 4 minutes

		ParticleSystem [] ps = GetComponentsInChildren<ParticleSystem> ();
		ps [0].Play ();
		ps [1].Play ();

		GetComponentInChildren<AudioSource> ().Play ();

	}
}

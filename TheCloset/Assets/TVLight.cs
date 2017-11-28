using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVLight : MonoBehaviour 
{
	public float Velocity=1;
	public float MaxIntensity = 6;
	public float MinIntensity = 5;

	float t;
	Light _light;

	void Start () 
	{
		t = 0;
		_light = GetComponent<Light> ();
	}
	
	void Update () 
	{
		float p = Mathf.PerlinNoise (t, 0);	
		t += Time.deltaTime * Velocity;

		_light.intensity = MinIntensity + (MaxIntensity-MinIntensity) * p;
	}
}

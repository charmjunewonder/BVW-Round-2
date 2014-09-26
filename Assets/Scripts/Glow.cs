using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {

	float pulseSpeed;
	float phase;
	public Color color;
	Vector3 Vec3(float x)
	{
		return new Vector3(x, x, x);
	}
	
	void Start() 
	{
		pulseSpeed = Random.Range(4.0f, 8.0f);
		phase = Random.Range(0.0f, Mathf.PI * 2.0f);
	}
	
	void Update() 
	{
		color.r = 1.0f - color.r;
		color.g = 1.0f - color.g;
		color.b = 1.0f - color.b;
		color = Color.Lerp(color, Color.white, 0.1f);
		
		
		color *= Mathf.Pow(Mathf.Sin(Time.time * pulseSpeed + phase) * 0.49f + 0.51f, 2.0f);
		
		
		renderer.material.SetColor("_EmissionColor", color);
		light.color = color;
	}
}

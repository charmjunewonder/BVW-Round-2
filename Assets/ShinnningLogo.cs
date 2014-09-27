using UnityEngine;
using System.Collections;

public class ShinnningLogo : MonoBehaviour {

	public Material[] materials;
	private float originalEmissionGain;
	public void shineLogo(int index){
		originalEmissionGain = materials [index].GetFloat ("_EmissionGain");
		materials[index].SetFloat("_EmissionGain", 0.2f);
		StartCoroutine (flashLight (index));
	}

	IEnumerator flashLight(int index){
		float pulseSpeed = 10.0f;
		float phase = 5.0f;
		Color color = materials [index].GetColor("_EmissionColor");
		Color originalColor = color;

		for(int i = 0; i < 100; ++i){
			color.r = 1.0f - color.r;
			color.g = 1.0f - color.g;
			color.b = 1.0f - color.b;
			color = Color.Lerp(color, Color.white, 0.1f);
			color *= Mathf.Pow(Mathf.Sin(Time.time * pulseSpeed + phase) * 0.49f + 0.51f, 2.0f);
			materials [index].SetColor("_EmissionColor", color);
			yield return new WaitForSeconds(0.02f);
		}
		materials[index].SetFloat("_EmissionGain", 0.0f);
		materials [index].SetColor("_EmissionColor", originalColor);
		originalEmissionGain = 0.0f;
	}

	public void unshineLogo(int index){
		materials[index].SetFloat("_EmissionGain", 0f);
		//yield return null;
	}
}

using UnityEngine;
using System.Collections;

public class locationScript : MonoBehaviour {
	public Texture2D[] locationtex;
	public AudioClip[] clips;


	// Use this for initialization
	void Start () {
		Vector3 pos = this.transform.localPosition;
		pos.z = -8.0f;
		this.transform.localPosition = pos;
	}

	public void SendText(int texnum)
	{
		StartCoroutine (WaitAndSend(texnum));
	}

	IEnumerator FadeIn()
	{
		for (float f=0; f<1.0f; f+= Time.deltaTime) 
		{
			yield return null;
			Vector3 pos = this.transform.localPosition;
			pos.z += 4.0f * Time.deltaTime;
			this.transform.localPosition = pos;
			//Color c = this.renderer.material.color;
			//c.a = Mathf.Clamp01(f);
			//this.renderer.material.color = c;
			//this.transform.localPosition;

		}

		Vector3 pos2 = this.transform.localPosition;
		pos2.z = -4.0f;
		this.transform.localPosition = pos2;
	}

	IEnumerator FadeOut()
	{
		for (float f=0; f<1.0f; f+= Time.deltaTime) 
		{
			yield return null;
			Vector3 pos = this.transform.localPosition;
			pos.z -= 4.0f * Time.deltaTime;
			this.transform.localPosition = pos;
			//Color c = this.renderer.material.color;
			//c.a = Mathf.Clamp01(f);
			//this.renderer.material.color = c;
			//this.transform.localPosition;
			
		}

		Vector3 pos2 = this.transform.localPosition;
		pos2.z = -8.0f;
		this.transform.localPosition = pos2;
	}


	IEnumerator WaitAndSend(int texnum)
	{
		yield return new WaitForSeconds(2.0f);
		this.renderer.material.mainTexture = locationtex [texnum];
		StartCoroutine (FadeIn ());
		audio.clip = clips [texnum];
		audio.Play ();
		yield return new WaitForSeconds(5.0f);
		StartCoroutine (FadeOut ());
	}

	// Update is called once per frame
	void Update () {
	
	}
}

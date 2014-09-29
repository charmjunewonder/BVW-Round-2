using UnityEngine;
using System.Collections;

public class locationScript : MonoBehaviour {
	public Texture2D[] locationtex;
	public AudioClip[] clips;
	public GameObject shinningLogo;
	public TextMesh clues;
	string[] locationstrings = {"Christmas Tree","Mapple Store","Wine Store","Cafe", "Whole Foods", "Church", "Nail Salon"};
	float xaspect;
	float hidepos;
	float showpos;


	// Use this for initialization
	void Start () {
		Vector3 pos = this.transform.localPosition;

		float aspect = (float)Screen.width / (float)Screen.height;


		hidepos =  4.0f - (2.0f - 2.0f/aspect)/2.0f + 2.0f - 0.2f;
		showpos = 4.0f + (2.0f - 2.0f/aspect)/2.0f - 0.3f;
		xaspect = hidepos - showpos;
		pos.x = hidepos;

		this.transform.localScale = new Vector3 (2.0f / aspect, 2.0f, 2.0f);
		this.transform.localPosition = pos;
	}

	public void SendText(int texnum)
	{
		StartCoroutine (WaitAndSend(texnum));
	}

	IEnumerator FadeIn()
	{
		Vector3 pos3 = this.transform.localPosition;
		pos3.x = hidepos;
		this.transform.localPosition = pos3;

		for (float f=0; f<1.0f; f+= Time.deltaTime) 
		{
			yield return null;
			Vector3 pos = this.transform.localPosition;
			pos.x -= xaspect * Time.deltaTime;
			this.transform.localPosition = pos;
			//Color c = this.renderer.material.color;
			//c.a = Mathf.Clamp01(f);
			//this.renderer.material.color = c;
			//this.transform.localPosition;

		}

		Vector3 pos2 = this.transform.localPosition;
		pos2.x = showpos;
		this.transform.localPosition = pos2;
		yield return new WaitForSeconds(3.0f);
		StartCoroutine (FadeOut ());
	}

	IEnumerator FadeOut()
	{
		Vector3 pos3 = this.transform.localPosition;
		pos3.x = showpos;
		this.transform.localPosition = pos3;

		for (float f=0; f<1.0f; f+= Time.deltaTime) 
		{
			yield return null;
			Vector3 pos = this.transform.localPosition;
			pos.x += xaspect * Time.deltaTime;
			this.transform.localPosition = pos;
			//Color c = this.renderer.material.color;
			//c.a = Mathf.Clamp01(f);
			//this.renderer.material.color = c;
			//this.transform.localPosition;
			
		}

		Vector3 pos2 = this.transform.localPosition;
		pos2.x = hidepos;
		this.transform.localPosition = pos2;
	}


	IEnumerator WaitAndSend(int texnum)
	{
		yield return new WaitForSeconds(1.0f);
		audio.clip = clips [7];
		audio.Play ();
		yield return new WaitForSeconds(1.0f);
		this.renderer.material.mainTexture = locationtex [texnum];
		StartCoroutine (FadeIn ());
		audio.clip = clips [texnum];
		audio.Play ();
		shinningLogo.GetComponent<ShinnningLogo> ().shineLogo (texnum);
		clues.text += "\n" + locationstrings [texnum];

	}

	// Update is called once per frame
	void Update () {
	
	}
}

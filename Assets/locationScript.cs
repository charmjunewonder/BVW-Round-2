using UnityEngine;
using System.Collections;

public class locationScript : MonoBehaviour {
	public Texture2D[] locationtex;
	public AudioClip[] clips;
	public GameObject shinningLogo;


	// Use this for initialization
	void Start () {
	
	}

	public void SendText(int texnum)
	{
		StartCoroutine (WaitAndSend(texnum));
	}

	IEnumerator WaitAndSend(int texnum)
	{
		yield return new WaitForSeconds(2.0f);
		this.renderer.material.mainTexture = locationtex [texnum];
		audio.clip = clips [texnum];
		audio.Play ();
		shinningLogo.GetComponent<ShinnningLogo>().shineLogo(texnum);

	}

	// Update is called once per frame
	void Update () {
	
	}
}

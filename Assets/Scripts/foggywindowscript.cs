using UnityEngine;
using System.Collections;

public class foggywindowscript : MonoBehaviour {
	bool shot;
	public GameObject[] girls;
	public GameObject window;

	Texture2D tex;
	Texture2D smalltex;

	void OnPostRender()
	{

		if (shot)
			return;
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		smalltex = new Texture2D (1024, 1024,TextureFormat.ARGB32,false);

		for (int i=0; i<1024; i++)
						for (int j=0; j<1024; j++) {
								smalltex.SetPixel (i, j, tex.GetPixelBilinear (i / 1024.0f, j / 1024.0f) + new Color(.3f,.3f,.3f,1.0f));
						}

//		Color[] pixels = smalltex.GetPixels ();
//		for (int i=0; i<pixels.Length; i++) 
//		{
//			pixels[i].r=Mathf.Clamp01(pixels[i].r+0.3f);
//			pixels[i].g=Mathf.Clamp01(pixels[i].g+0.3f);
//			pixels[i].b=Mathf.Clamp01(pixels[i].b+0.3f);
//			pixels[i].a = 1.0f;
//
//		}
//		smalltex.SetPixels (pixels);


		smalltex.Apply ();
		window.renderer.material.mainTexture = smalltex;
		//tex.filterMode = FilterMode.Trilinear;

		Debug.Log ("hello");
		shot = true;
		//Camera.main.gameObject.GetComponent<BlurEffect> ().enabled = false;

	}
	// Use this for initialization
	void Start () {
		shot = false;

		for (int i=0; i<girls.Length; i++)
						setVisible (girls [i], false);
		setVisible (window, false);
		tex = new Texture2D (Screen.width, Screen.height,TextureFormat.ARGB32,false);

		//Camera.main.gameObject.GetComponent<BlurEffect> ().enabled = true;
		Camera.main.transform.localRotation = Quaternion.Euler(0,0,180);
		Camera.main.Render ();
		Camera.main.transform.localRotation = Quaternion.identity;
		for (int i=0; i<girls.Length; i++)
			setVisible (girls [i], true);
		setVisible (window, true);

	}
	
	// Update is called once per frame
	void Update () {
		//if(shot)
		//	Camera.main.gameObject.GetComponent<BlurEffect> ().enabled = false;
	}

	void setVisible(GameObject g, bool visible)
	{
		MeshRenderer m = g.GetComponent<MeshRenderer> ();
		if(m != null) m.enabled = visible;
		
		MeshRenderer[] mchild = g.GetComponentsInChildren<MeshRenderer> ();
		if (mchild != null)
			for (int i=0; i<mchild.Length; i++)
				mchild [i].enabled = visible;
		
		
	}
}

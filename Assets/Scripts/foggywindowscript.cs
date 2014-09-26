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
		smalltex = (Texture2D)Instantiate(window.renderer.material.mainTexture);
		Texture2D frosty = (Texture2D)window.renderer.material.mainTexture;


		for (int i=0; i<smalltex.width; i++)
			for (int j=0; j<smalltex.height; j++) {
			Color scene = tex.GetPixelBilinear ((float)i / smalltex.width, (float)j / smalltex.height);
			Color frost = frosty.GetPixelBilinear((float)i / smalltex.width, (float)j / smalltex.height);


			smalltex.SetPixel (i, j, (Color.white - scene) * frost * 0.75f + scene + Color.cyan * 0.1f);
						}
		//tex.filterMode = FilterMode.Trilinear;

		smalltex.Apply ();
		window.renderer.material.mainTexture = smalltex;

		Debug.Log ("hello");
		shot = true;


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

	public bool HasShot()
	{
		return shot;
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

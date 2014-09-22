using UnityEngine;
using System.Collections;

public class loadingscript : MonoBehaviour {

	public GameObject[] checkSpheres;
	public GameObject window;
	public GameObject main_cam;
	// Use this for initialization
	Vector2[] spherescreenpos;
	bool[] flagarray;


	void Start () {
		Texture2D tex = (Texture2D)window.renderer.material.mainTexture;
		spherescreenpos = new Vector2[checkSpheres.Length];
		flagarray = new bool[checkSpheres.Length];
		for (int i=0; i<checkSpheres.Length; i++) {
			Vector3 p = main_cam.camera.WorldToScreenPoint(checkSpheres[i].transform.position);
			spherescreenpos[i].x = Screen.width - p.x;
			spherescreenpos[i].y = Screen.height - p.y;

			spherescreenpos[i].x = Mathf.FloorToInt((float)spherescreenpos[i].x * (float)tex.width / (float)Screen.width);
			spherescreenpos[i].y = Mathf.FloorToInt ((float)spherescreenpos[i].y * (float)tex.height / (float)Screen.height);

			flagarray [i] = false;
				}


	}
	
	// Update is called once per frame
	void Update () {
		Texture2D tex = (Texture2D)window.renderer.material.mainTexture;

		for (int i=0; i<checkSpheres.Length; i++) 
		{
			Color col = tex.GetPixel(Mathf.FloorToInt(spherescreenpos[i].x),Mathf.FloorToInt(spherescreenpos[i].y));
			if(col.a < 0.5f)
				flagarray[i] = true;
		}

		for (int i=0; i<checkSpheres.Length; i++)
						if (flagarray [i] == false)
								return;

		Application.LoadLevel ("GamePlay");
	}
}

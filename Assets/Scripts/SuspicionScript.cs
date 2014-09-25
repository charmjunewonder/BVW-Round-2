using UnityEngine;
using System.Collections;

public class SuspicionScript : MonoBehaviour {
	public GameObject window;
	public GameObject[] girls;

	// Use this for initialization
	void Start () {

		for (int i=0; i<girls.Length; i++) 
		{
			//girls[i].GetComponentInChildren<lifebarwrapper>().reset();
			//setVisible (ref girls [i], false);
			//girls[i].GetComponentInChildren<lifebarwrapper>().gameObject.SetActive(false);
		}
	}

	bool checkIfSeen(int girl_index)
	{
				Texture2D tex = (Texture2D)window.renderer.material.mainTexture;
		
		
				//girl4 is the red girl
				Vector3 pos = girls [girl_index].transform.position;
				Vector3 p = Camera.main.WorldToScreenPoint (pos);
				Vector2 screenpos = new Vector2 ();
				screenpos.x = Screen.width - p.x;
				screenpos.y = Screen.height - p.y;
				screenpos.x = Mathf.FloorToInt ((float)screenpos.x * (float)tex.width / (float)Screen.width);
				screenpos.y = Mathf.FloorToInt ((float)screenpos.y * (float)tex.height / (float)Screen.height);
				Color col = tex.GetPixel (Mathf.FloorToInt (screenpos.x), Mathf.FloorToInt (screenpos.y));
		
				if (col.a < 0.01f) {
						Debug.Log ("WIN");
						return true;
				}
		
		
				return false;
	}


	// Update is called once per frame
	void Update () {
		for(int i=0;i<girls.Length;i++)
		{
			lifebarwrapper lifebar = girls[i].GetComponentInChildren<lifebarwrapper>();
			if(checkIfSeen(i))
			{
				//lifebar.gameObject.SetActive(true);
				lifebar.addLife();
			}
			else
			{
				//lifebar.gameObject.SetActive(false);
				lifebar.minusLife();
			}
		}
	}


	void setVisible(ref GameObject g, bool visible)
	{
		MeshRenderer m = g.GetComponent<MeshRenderer> ();
		if(m != null) m.enabled = visible;

		MeshRenderer[] mchild = g.GetComponentsInChildren<MeshRenderer> ();
		if (mchild != null)
						for (int i=0; i<mchild.Length; i++)
								mchild [i].enabled = visible;


	}
}

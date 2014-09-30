using UnityEngine;
using System.Collections;

public class StoryScript : MonoBehaviour {
	MovieTexture mov;

	// Use this for initialization
	void Start () {
		resize_window_plane ();
		mov = (MovieTexture)renderer.material.mainTexture;
		mov.Play ();
	}

	void resize_window_plane()
	{
		float distance = (Camera.main.transform.position - transform.position).magnitude;
		float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
		float width = height * Screen.width / Screen.height;
		
		transform.localScale = new Vector3 (width * 0.1f, 1, height * 0.1f);
	}

	
	// Update is called once per frame
	void Update () {
		if (!mov.isPlaying)
			Application.LoadLevel ("GamePlay");
	}

}

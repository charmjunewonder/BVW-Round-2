using UnityEngine;
using System.Collections;

public class GlassMoisture : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D texture = new Texture2D(Screen.width, Screen.height);
		Color[] colors = new Color[Screen.width*Screen.height];
		// tint each mip level
		for( int x = 0; x < Screen.width; ++x ) {
			for( int y = 0; y < Screen.height; ++y ) {
				colors[x*Screen.height+y] = Color.white;
			}
		}
		texture.SetPixels(colors);

		// actually apply all SetPixels, don't recalculate mip levels
		guiTexture.texture = texture;

		texture.Apply( false );
		//GetComponent<GUITexture> ().guiTexture.texture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		Texture2D texture = (Texture2D)guiTexture.texture;
		Color[] colors = new Color[64];
		for( int x = 0; x < 8; ++x ) {
			for( int y = 0; y < 8; ++y ) {
				colors[x*8+y] = new Color(1,1,1,0.5f);
			}
		}
		//texture.SetPixels ((int)Input.mousePosition.x+4, (int)Input.mousePosition.y+4, 8, 8, colors);
		texture.SetPixel ((int)Input.mousePosition.x, (int)Input.mousePosition.y, new Color(1,1,1,0.5f));

		//Debug.Log (Input.mousePosition + :);
		texture.Apply( false );

		//Input.mousePosition
	}
}

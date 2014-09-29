using UnityEngine;
using System.Collections;

public class HEStory : MonoBehaviour {
	public Texture[] texs;
	// Use this for initialization
	void Start () {
		StartCoroutine (changeScene ());
	}
	
	IEnumerator changeScene(){
		for(int i = 0; i < 5; ++i){
			guiTexture.texture = texs[i];
			yield return new WaitForSeconds(1.0f);
		}
	}
}

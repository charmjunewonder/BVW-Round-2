using UnityEngine;
using System.Collections;

public class StoryController : MonoBehaviour {
	public GameObject storyBoard;
	public Texture[] storyPics;
	// Use this for initialization
	void Start () {

		//StartCoroutine("changeStoryPic");
	}
	
	IEnumerator changeStoryPic(){
		for(int i = 0; i < 9; ++i){
			storyBoard.guiTexture.texture = storyPics [i];
			yield return new WaitForSeconds(2.0f);
		}
	}
}

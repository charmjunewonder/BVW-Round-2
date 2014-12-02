using UnityEngine;
using System.Collections;

public class RestartController : MonoBehaviour {
	public Texture[] backgroundTexture;
	public Texture[] buttonTexture;
	private int restartButtonIndex = 0;
	private Rect restartButtonRect;
	private int storyButtonIndex = 2;
	private Rect storyButtonRect;
	private int quitButtonIndex = 4;
	private Rect quitButtonRect;
	private float widthRatio;
	private int backgroundIndex = 0;

	// Use this for initialization
	void Start () {
		int defaultWidth = 1600;
		widthRatio = Screen.width * 1.0f/ defaultWidth;
		//restart
		float width = buttonTexture[restartButtonIndex].width*0.6f*widthRatio, height = buttonTexture[restartButtonIndex].height*0.6f*widthRatio;
		restartButtonRect = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, width, height);
		//story
		width = buttonTexture[storyButtonIndex].width*0.6f*widthRatio;
		height = buttonTexture[storyButtonIndex].height*0.6f*widthRatio;
		storyButtonRect = new Rect(Screen.width * 0.5f, Screen.height * 0.6f, width, height);
		//quit
		width = buttonTexture[quitButtonIndex].width*0.6f*widthRatio;
		height = buttonTexture[quitButtonIndex].height*0.6f*widthRatio;
		quitButtonRect = new Rect(Screen.width * 0.5f, Screen.height * 0.7f, width, height);

		backgroundIndex = PlayerPrefs.GetInt("SlayCount", 0);
	}
	
	// Update is called once per frame
	void Update () {
		bool clickRestart = false;
		bool clickStory = false;
		bool clickQuit = false;
		float mouseX = Input.mousePosition.x;
		float mouseY = Screen.height - Input.mousePosition.y;	
		Vector2 mousePos = new Vector2(mouseX, mouseY);
		if(restartButtonRect.Contains(mousePos)){
			clickRestart = true;
			restartButtonIndex = 1;
		} else{
			restartButtonIndex = 0;
		}
		if(storyButtonRect.Contains(mousePos)){
			clickStory = true;
			storyButtonIndex = 3;
		} else{
			storyButtonIndex = 2;
		}
		if(quitButtonRect.Contains(mousePos)){
			clickQuit = true;
			quitButtonIndex = 5;
		} else{
			quitButtonIndex = 4;
		}
		if(Input.GetButtonDown("Fire1")) {
			if(clickRestart){
				Debug.Log("Restart");
				//Application.LoadLevel("GamePlay");
			} else if(clickStory){
				Debug.Log("Story");
				//Application.LoadLevel("scene_game");
				//Application.LoadLevel("Credit");
			}
			else if(clickQuit){
				Debug.Log("Quit");
				//Application.Quit();
			}				
		}
	}

	void OnGUI() {
		
		guiTexture.texture = backgroundTexture[backgroundIndex];

		//restart
		GUI.DrawTexture(restartButtonRect, buttonTexture[restartButtonIndex]);
		GUI.DrawTexture(storyButtonRect, buttonTexture[storyButtonIndex]);
		GUI.DrawTexture(quitButtonRect, buttonTexture[quitButtonIndex]);

    }
}

using UnityEngine;
using System.Collections;

public class phoneDisplay : MonoBehaviour {
	//public GUIText timer;
	public GUITexture text1;
	public GUITexture text2;
	public GUITexture text3;
	public GUITexture text4;
	
	public float textTime1 = 4.0f;
	public float textTime2 = 5.0f;
	public float textTime3 = 5.0f;
	public float textTime4 = 5.0f;

	GameObject[] prevTexts;

	float myTimer = 0.0f;

	// Maximum text y position
	float ceiling;

	// variable that stores current text status
	int textState = 0;

	float textTranslate = 0.099f;

	float phoneHeight;
	float phoneWidth ;
	
	float phoneLeftMargin;
	float phoneBottomMargin;
	
	float textWidth;
	float textHeight;
	
	float textLeftMargin;
	float textBottomMargin ;
	
	float textMargin;

	Rect text0_1Rect;
	Rect text0_2Rect;
	Rect text0_3Rect;
	Rect textNewRect;

	float textJumpAmount;

	// Use this for initialization
	void Start () {

		// inits text number state
		textState = 1;

		phoneHeight = Screen.height * 0.75f;
		phoneWidth = phoneHeight * 0.5f;
		
		phoneLeftMargin = Screen.width * 0.01f;
		phoneBottomMargin = phoneHeight * 0.4f;

		textWidth = phoneWidth * 0.78f;
		textHeight = phoneHeight * 0.1f;

		textLeftMargin = phoneWidth * 0.15f * 0.75f;
		textBottomMargin = phoneHeight * 0.5f;

		textMargin = phoneHeight * 0.035f;

		ceiling = 0.3489f;

		textJumpAmount = textMargin + textHeight;


		text0_1Rect = new Rect (Screen.width - phoneWidth - phoneLeftMargin + textLeftMargin, 
		                           Screen.height - phoneHeight - phoneBottomMargin + textBottomMargin, 
		                           textWidth * 0.5f, 
		                           textHeight * 0.65f);

		text0_2Rect = new Rect (Screen.width - phoneWidth - phoneLeftMargin + textLeftMargin, 
		                           Screen.height - phoneHeight - phoneBottomMargin + textBottomMargin - textHeight - textMargin, 
		                           textWidth, 
		                           textHeight);

		text0_3Rect = new Rect (Screen.width - phoneWidth - phoneLeftMargin + textLeftMargin, 
		                           Screen.height - phoneHeight - phoneBottomMargin + textBottomMargin - (textHeight + textMargin) * 2, 
		                           textWidth * 0.8f, 
		                           textHeight);

		textNewRect = new Rect (Screen.width - phoneWidth - phoneLeftMargin + textLeftMargin, 
		                        Screen.height - phoneHeight - phoneBottomMargin + textBottomMargin - (textHeight + textMargin) * 2, 
		                        textWidth, 
		                        textHeight);


		// place phone textures
		this.guiTexture.pixelInset = new Rect (Screen.width - phoneWidth - phoneLeftMargin, Screen.height - phoneHeight - phoneBottomMargin, phoneWidth, phoneHeight);
		this.transform.FindChild("phone-foreground").guiTexture.pixelInset = new Rect (Screen.width - phoneWidth - phoneLeftMargin, Screen.height - phoneHeight - phoneBottomMargin, phoneWidth, phoneHeight);

		this.transform.FindChild ("text0_1").guiTexture.pixelInset = text0_1Rect;

		this.transform.FindChild ("text0_2").guiTexture.pixelInset = text0_2Rect;

		this.transform.FindChild ("text0_3").guiTexture.pixelInset = text0_3Rect;
	}

	// Update is called once per frame
	void Update () 
	{
		/*myTimer+=Time.deltaTime;
		//timer.text = "Time :" + (int)myTimer;

		if (myTimer >= textTime1)
		{
			sendText(1);
		}

		if (myTimer >= textTime1 + textTime2) 
		{
			sendText(2);
		}

		if (myTimer >= textTime1 + textTime2 + textTime3) 
		{
			sendText(3);
		}

		if (myTimer >= textTime1 + textTime2 + textTime3 + textTime4) 
		{
			sendText(4);
		}*/
	}

	public void sendText(int textNum)
	{
		if (textState == textNum){ // this line can be replaced with messaging system

			// play the sound
			audio.Play();
			
			// move every texts upward
			prevTexts = GameObject.FindGameObjectsWithTag("text");

			foreach (GameObject text_iter in prevTexts) {
				text_iter.transform.Translate (new Vector3(0, textTranslate, 0), Space.World);

//				text_iter.guiTexture.pixelInset = new Rect (Screen.width - phoneWidth - phoneLeftMargin + textLeftMargin, 
//				                        Screen.height - phoneHeight - phoneBottomMargin + textBottomMargin + textJumpAmount, 
//				                        textWidth,
//				                        textHeight);
//				textJumpAmount *= 2;

				// remove text exceeds the smartphone screen
                if(text_iter.transform.position.y >= ceiling)
				{
					Destroy(text_iter);
				}
			}

			GameObject curText;

			// display new text
            switch (textNum)
			{
			case 1:
//				curText = Instantiate(text1) as GameObject;
//				curText.transform.parent = transform;
				text1.gameObject.SetActive(true);

				this.transform.FindChild("text1").guiTexture.pixelInset = textNewRect;
				break;
			case 2:
//				curText = Instantiate(text2) as GameObject;
//				curText.transform.parent = transform;
				text2.gameObject.SetActive(true);
				this.transform.FindChild("text2").guiTexture.pixelInset = textNewRect;
				break;
			case 3:
//				curText = Instantiate(text3) as GameObject;
//				curText.transform.parent = transform;
				text3.gameObject.SetActive(true);
				this.transform.FindChild("text3").guiTexture.pixelInset = textNewRect;
				break;
			case 4:
//				curText = Instantiate(text4) as GameObject;
//				curText.transform.parent = transform;
				text4.gameObject.SetActive(true);
				this.transform.FindChild("text4").guiTexture.pixelInset = textNewRect;
				break;
			}
            textState++;
        }
    }
    
}

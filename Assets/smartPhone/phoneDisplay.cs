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

	// Margin between texts
	float textMargin = 0.0798f;

	// Maximum text y position
	float ceiling = 0.76f;

	// variable that stores current text status
	int textState = 0;
    
	// Use this for initialization
	void Start () {
		textState = 1;
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
				text_iter.transform.Translate (new Vector3(0, textMargin, 0), Space.World);

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
				break;
			case 2:
//				curText = Instantiate(text2) as GameObject;
//				curText.transform.parent = transform;
				text2.gameObject.SetActive(true);
				break;
			case 3:
//				curText = Instantiate(text3) as GameObject;
//				curText.transform.parent = transform;
				text3.gameObject.SetActive(true);
				break;
			case 4:
//				curText = Instantiate(text4) as GameObject;
//				curText.transform.parent = transform;
				text4.gameObject.SetActive(true);
				break;
			}
            textState++;
        }
    }
    
}

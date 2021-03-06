﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject[] girls;
	public GameObject map;
	public MapRepresentation mapRepresentation;
	public GameObject trackingBall;
	public const int TRACKING_RADIUS = 3;
	public GameObject phoneGUI;
	public bool useKenect;
	public bool useMouse;
	public KinectPointController pointskel;
	public GameObject window;
	public locationScript locbox;
	public AudioClip[] audioClips;
	public GameObject passenger;
	public AudioController audioController;
	public GameObject storyPlane;
	int failures;
	int cheatingGirlIndex = -1;
	// Use this for initialization
	void Start () {
		mapRepresentation = map.GetComponent<MapRepresentation> ();
		failures = 0;
		StartCoroutine ("startGame");
		//StartCoroutine ("checkIfSuspendFinishedMoving");
		StartCoroutine (checkIfAllSuspendsFinishedMoving ());
		StartCoroutine (checkIfLifeBarIsFull ());
		//StartCoroutine (inGameVoice ());
	}

	IEnumerator inGameVoice(){
		yield return new WaitForSeconds (15.0f);

		while(true){
			if(audio.isPlaying){
				yield return new WaitForSeconds (1f);
				continue;
			}
			audio.clip = audioClips[8];
			audio.Play();
			yield return new WaitForSeconds (1.5f);
			audio.clip = audioClips[9];
			audio.Play();

			yield return new WaitForSeconds (10.0f);
		}
	}

	// Update is called once per frame
	void Update () {
		/*Vector2 mouse = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(mouse.x, mouse.y, 0));
		RaycastHit hit;
		int layerMaskOfMap = 1 << 8;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskOfMap)){
			trackingBall.transform.position = hit.point;
			//ArrayList girls = getGirlsFromPosition((Vector2)hit.point);
			ArrayList girlsBeingWatched = getGirlsFromPosition(new Vector2(hit.point.x, hit.point.z));

			if(girlsBeingWatched.Count > 0){
				Debug.Log("get girls " + girlsBeingWatched.Count);
			}
			girlsBeingWatched.Clear();
		}*/
	}

	IEnumerator checkIfLifeBarIsFull(){
		int[] sortedList = {0, 1, 2, 3, 4};
		while (true) {
			bool ifFull = false;
			int focusCount = 0;
			for(int n = 0; n < 5; ++n){
				ifFull |= girls[n].GetComponent<Suspect>().isLifeBarFull();
			}
			if(ifFull){

				int i, j, temp;
				for (i = 0; i < sortedList.Length - 1; i++)
				{
					for (j = i + 1; j < sortedList.Length; j++)
					{
						if (girls[sortedList[i]].GetComponent<Suspect>().getLife() 
						    > girls[sortedList[j]].GetComponent<Suspect>().getLife())
						{
							temp = sortedList[i];
							sortedList[i] = sortedList[j];
							sortedList[j] = temp;
						}
					}
				}
				for (i = 0; i < sortedList.Length; i++){
					Debug.Log(sortedList[i] + " " + girls[sortedList[i]].GetComponent<Suspect>().getLife());
				}
				bool isBothFull = girls[sortedList[0]].GetComponent<Suspect>().isLifeBarFull()
					& girls[sortedList[1]].GetComponent<Suspect>().isLifeBarFull();
				if(isBothFull){
					Debug.Log("Those girls looks similar. I am confused.");

				} else{
					girls[sortedList[0]].GetComponent<HighlightableObject> ().FlashingOn ();

					//for(int a=0;a<girls.Length;a++)
					//	if(a!=sortedList[0])
					//		girls[a].transform.GetChild(1).GetComponentInChildren<MeshRenderer>().enabled = false;

					audio.clip = audioClips[1];
					audio.Play();
					girls[sortedList[0]].GetComponentInChildren<TextMesh>().text = "5";
					yield return new WaitForSeconds(3.0f);
					
					for(int m = 0; m < 4; ++m){
						audio.clip = audioClips[m+2];
						audio.Play();	

						girls[sortedList[0]].GetComponentInChildren<TextMesh>().text = (4-m).ToString();

						Debug.Log("Life: " + girls[sortedList[0]].GetComponent<Suspect>().getLife());
						if(girls[sortedList[0]].GetComponent<Suspect>().isLifeBarFull()){
							Debug.Log("stay still");
							focusCount++;
						} else{
							Debug.Log("i change my mind");
							girls[sortedList[0]].GetComponentInChildren<TextMesh>().text = "";
							audio.clip = audioClips[7];
							audio.Play();	
							break;
						}
						yield return new WaitForSeconds(1.0f);
					}

					if(focusCount < 4){
						girls[sortedList[0]].GetComponent<HighlightableObject> ().FlashingOff ();
						//for(int a=0;a<girls.Length;a++)
						//	girls[a].transform.GetChild(1).GetComponentInChildren<MeshRenderer>().enabled = true;

						continue;
					}

					girls[sortedList[0]].GetComponentInChildren<TextMesh>().text = "<color=#ff5555><b>SLAP</b></color>";
					yield return new WaitForSeconds(1.0f);
					girls[sortedList[0]].GetComponentInChildren<TextMesh>().text = "";
					if(girls[sortedList[0]].GetComponent<Suspect>().isCheating){
						Debug.Log ("WIN");
						PlayerPrefs.SetInt("SlayCount", failures);
						Debug.Log ("failures " + failures);
						audio.clip = audioClips[6];
						audio.Play();	
						Application.LoadLevel("HappyEnding");
						break;
					} else{
						Debug.Log ("Loss");
						GameObject fakePassenger = Instantiate(passenger) as GameObject;
						fakePassenger.GetComponent<Passenger>().initialize();
						fakePassenger.GetComponent<Passenger>().setStartPosition(girls[sortedList[0]].GetComponent<Suspect>().currentPosition);
						fakePassenger.GetComponent<Passenger>().moveInRoutine();
						girls[sortedList[0]].GetComponent<Suspect>().resetLife();
						girls[sortedList[0]].GetComponent<Suspect>().isFinishedMoving = true;
						girls[sortedList[0]].GetComponent<HighlightableObject> ().FlashingOff ();
						girls[sortedList[0]].SetActiveRecursively(false);

						failures++;
						audioController.turnOffSound();
						setVisible(window,false);
						storyPlane.SetActive(true);
						storyPlane.GetComponent<BEStoryScript>().playMovie();
						yield return new WaitForSeconds(14.5f);
						setVisible(window,true);
						audioController.turnOnSound();
						storyPlane.GetComponent<BEStoryScript>().stopMovie();
						storyPlane.SetActive(false);

						//Application.LoadLevel("BadEnding");
						//break;
					}


				}

				//StartCoroutine ("checkStatusOfGame");

			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	/*IEnumerator checkIfSuspendFinishedMoving(){
		while (true) {
			if(girls[4].GetComponent<Suspect>().isFinishedMoving){
				//StartCoroutine ("checkStatusOfGame");
				audio.clip = audioClips[0];
				audio.Play();
				yield return new WaitForSeconds(3.0f);

				for(int i = 1; i < 6; ++i){
					audio.clip = audioClips[i];
					audio.Play();
					
					yield return new WaitForSeconds(1.0f);
				}

				if(checkIfWin()){
					Debug.Log ("WIN");
					//Application.LoadLevel("HappyEnding");
					break;
				} else{
					Debug.Log ("Loss");
					//Application.LoadLevel("BadEnding");
					break;
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}*/

	IEnumerator checkIfAllSuspendsFinishedMoving(){
		while (true) {
			if(girls[4].GetComponent<Suspect>().isFinishedMoving &&
			   girls[0].GetComponent<Suspect>().isFinishedMoving &&
			   girls[1].GetComponent<Suspect>().isFinishedMoving &&
			   girls[2].GetComponent<Suspect>().isFinishedMoving &&
			   girls[3].GetComponent<Suspect>().isFinishedMoving){

				suspectStartMoving();
				Debug.Log ("Move Again!!!");
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator checkStatus(){

		yield return new WaitForSeconds(0.1f);
	}

	/*bool checkIfWin()
	{
		Texture2D tex = (Texture2D)window.renderer.material.mainTexture;


		//girl4 is the red girl
			Vector3 pos = girls[4].transform.position;
			Vector3 p = Camera.main.WorldToScreenPoint(pos);
			Vector2 screenpos = new Vector2();
			screenpos.x = Screen.width - p.x;
			screenpos.y = Screen.height - p.y;
			screenpos.x  = Mathf.FloorToInt((float)screenpos.x * (float)tex.width / (float)Screen.width);
			screenpos.y = Mathf.FloorToInt ((float)screenpos.y * (float)tex.height / (float)Screen.height);
			Color col = tex.GetPixel(Mathf.FloorToInt(screenpos.x),Mathf.FloorToInt(screenpos.y));

			if(col.a < 0.5f)
			{
				Debug.Log ("WIN");
				return true;
			}
	

		return false;
	}*/

	/*IEnumerator checkStatusOfGame(){
		bool isWin = false;
		for(int i = 0; i < 50; ++i){
			if(useMouse){
				Vector2 mouse = Input.mousePosition;
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(mouse.x, mouse.y, 0));
				RaycastHit hit;
				int layerMaskOfMap = 1 << 8;
				
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskOfMap)){
					if(Vector2.Distance(new Vector2(hit.point.x, hit.point.z), girls[4].GetComponent<Person>().currentPosition.pointOfGrid) < TRACKING_RADIUS){
						isWin = true;
						Debug.Log ("win");
						break;
					} 
				}
			}
			if(useKenect){
				Vector3 lefthandscreenpos = Camera.main.WorldToScreenPoint (pointskel.Hand_Left.transform.position);
				lefthandscreenpos.y = Screen.height - lefthandscreenpos.y;
				lefthandscreenpos.x = Screen.width - lefthandscreenpos.x;

				Vector3 righthandscreenpos = Camera.main.WorldToScreenPoint (pointskel.Hand_Right.transform.position);
				righthandscreenpos.y = Screen.height - righthandscreenpos.y;
				righthandscreenpos.x = Screen.width - righthandscreenpos.x;

				Ray rayLeft = Camera.main.ScreenPointToRay(lefthandscreenpos);
				RaycastHit hitLeft;
				int layerMaskOfMap = 1 << 8;
				
				if (Physics.Raycast(rayLeft, out hitLeft, Mathf.Infinity, layerMaskOfMap)){
					if(Vector2.Distance(new Vector2(hitLeft.point.x, hitLeft.point.z), 
					                    girls[4].GetComponent<Person>().currentPosition.pointOfGrid) < TRACKING_RADIUS){
						isWin = true;
						Debug.Log ("win");
						break;
					} 
				}

				Ray rayRight = Camera.main.ScreenPointToRay(righthandscreenpos);
				RaycastHit hitRight;
				
				if (Physics.Raycast(rayRight, out hitRight, Mathf.Infinity, layerMaskOfMap)){
					if(Vector2.Distance(new Vector2(hitRight.point.x, hitRight.point.z), 
					                    girls[4].GetComponent<Person>().currentPosition.pointOfGrid) < TRACKING_RADIUS){
						isWin = true;
						Debug.Log ("win");
						break;
					} 
				}
			}
			yield return new WaitForSeconds(0.1f);

		}

		if (!isWin){
			Debug.Log ("lose");
		} 
	}*/
	public ArrayList getGirlsFromPosition(Vector2 position){
		ArrayList girlsBeingWatched = new ArrayList ();
		for (int i = 0; i < girls.Length; ++i) {
			if(Vector2.Distance(position, girls[i].GetComponent<Person>().currentPosition.pointOfGrid) < TRACKING_RADIUS){
				girlsBeingWatched.Add(girls[i]);
			}
		}
		return girlsBeingWatched;
	}

	public ArrayList getGirlsFromTwoHands(Vector2 leftHand, Vector2 rightHand){
		ArrayList girlsBeingWatched = new ArrayList ();
		for (int i = 0; i < girls.Length; ++i) {
			if(Vector2.Distance(leftHand, girls[i].GetComponent<Person>().currentPosition.pointOfGrid) < 4
			   || Vector2.Distance(rightHand, girls[i].GetComponent<Person>().currentPosition.pointOfGrid) < 4){
				girlsBeingWatched.Add(girls[i]);
			}
		}
		return girlsBeingWatched;
	}

	void suspectStartMoving(){
		int[,] locationMatrix = mapRepresentation.generateRandomPlaceMatrix ();

		//random
		List<int> randomNum = new List<int>();
		for (int i = 0; i < girls.Length; i++)
			randomNum.Add (i);
		
		int index, value;
		for(int i = 0; i < 5; ++i){
			if(!girls [i].activeSelf)
				continue;
			Grid[] routine = new Grid[5];
			index = Random.Range (0, randomNum.Count);
			value = randomNum [index];
			randomNum.RemoveAt (index);
			if(value == cheatingGirlIndex)
				value = 6;

			for(int j = 0; j < 5; ++j){
				routine[j] = mapRepresentation.getInterestPlace(locationMatrix[value, j]);
			}
			girls [i].GetComponent<Suspect> ().destinations = routine;
			girls [i].GetComponent<Suspect> ().girlIndex = value;
			girls [i].GetComponent<Suspect> ().nextDestination = 0;
			girls [i].GetComponent<Suspect> ().isFinishedMoving = false;
			girls [i].GetComponent<Suspect> ().moveInRoutine ();

		}
	}

	IEnumerator startGame(){

		Vector2[] offsets = {new Vector2(-2, 1), new Vector2(-3, 5), new Vector2(2, 1), 
			new Vector2(0, -1), new Vector2(3, 5)};
		List<int> randomNumbers = new List<int>();
		for(int i = 0; i < 5; ++i){
			randomNumbers.Add (i);
		}

		//set the cheating girl
		cheatingGirlIndex = Random.Range(0, girls.Length);
		girls[cheatingGirlIndex].GetComponent<Suspect> ().isCheating = true;
		for (int i = 0; i < girls.Length; ++i) {
			//Vector2 offsets = mapRepresentation.christmasTree.pointOfGrid;
			int index = Random.Range(0, randomNumbers.Count);
			int value = randomNumbers [index];
			Grid startPosition = mapRepresentation.getGrid(mapRepresentation.christmasTree.pointOfGrid + offsets[value]);
			randomNumbers.RemoveAt (index);
			/*Grid startPosition = mapRepresentation.christmasTree;
			while(startPosition.gridType != Grid.GridType.NormalGrid){
				startPosition = mapRepresentation.getGrid(mapRepresentation.christmasTree.pointOfGrid
				                                          + new Vector2(Random.Range(-7, 6), Random.Range(-4, 5)));
			}*/
			girls[i].GetComponent<Suspect>().setStartPosition(startPosition);
			girls[i].GetComponent<Suspect>().lookAtChristmasTree();
		}

		yield return new WaitForSeconds(3.0f);
		audio.clip = audioClips[0];
		audio.Play();
		yield return new WaitForSeconds(5.0f);

		//phoneGUI.GetComponent<phoneDisplay> ().sendText (mapRepresentation.locationMatrix [4, 0]);
		locbox.SendText (0);
		yield return new WaitForSeconds(5.0f);

		suspectStartMoving ();


		/*Grid[] g1 = {mapRepresentation.christmasTree, getLocation(0,0),getLocation(0,1),getLocation(0,2),getLocation(0,3)};
		girls [0].GetComponent<Suspect> ().destinations = g1;
		girls [0].GetComponent<Suspect> ().moveInRoutine ();
	

		Grid[] g5 = {mapRepresentation.christmasTree, getLocation(1,0),getLocation(1,1),getLocation(1,2),getLocation(1,3)};
		girls [4].GetComponent<Suspect> ().destinations = g5;
		girls [4].GetComponent<Suspect> ().moveInRoutine ();

		yield return new WaitForSeconds(4.0f);

		Grid[] g2 = {mapRepresentation.christmasTree, getLocation(2,0),getLocation(2,1),getLocation(2,2),getLocation(2,3)};
		girls [1].GetComponent<Suspect> ().destinations = g2;
		girls [1].GetComponent<Suspect> ().moveInRoutine ();
		
		Grid[] g3 = {mapRepresentation.christmasTree, getLocation(3,0),getLocation(3,1),getLocation(3,2),getLocation(3,3)};
		girls [2].GetComponent<Suspect> ().destinations = g3;
		girls [2].GetComponent<Suspect> ().moveInRoutine ();
		
		Grid[] g4 = {mapRepresentation.christmasTree, getLocation(4,0),getLocation(4,1),getLocation(4,2),getLocation(4,3)};
		girls [3].GetComponent<Suspect> ().destinations = g4;
		girls [3].GetComponent<Suspect> ().moveInRoutine ();*/
	}


	/*Grid getLocation(int girl,int locindex)
	{
				int loc = mapRepresentation.locationMatrix [girl, locindex];
				switch (loc) {
				case 0:
					return mapRepresentation.christmasTree;
				case 1:
						return mapRepresentation.mappleStore;
				case 2:
						return mapRepresentation.wineSpirits;
				case 3:
						return mapRepresentation.nailSalon;
				case 4:
						return mapRepresentation.holeFood;
				default:
						return mapRepresentation.christmasTree;
				}
				;
		}*/

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

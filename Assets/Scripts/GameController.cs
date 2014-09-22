using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject[] girls;
	public GameObject map;
	MapRepresentation mapRepresentation;
	public GameObject trackingBall;
	public const int TRACKING_RADIUS = 3;
	public GameObject phoneGUI;
	// Use this for initialization
	void Start () {
		mapRepresentation = map.GetComponent<MapRepresentation> ();

		StartCoroutine ("startGame");
		StartCoroutine ("checkIfSuspendFinishedMoving");

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

	IEnumerator checkIfSuspendFinishedMoving(){
		while (true) {
			if(girls[4].GetComponent<Person>().isFinishedMoving){
				StartCoroutine ("checkStatusOfGame");
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator checkStatusOfGame(){
		bool isWin = false;
		for(int i = 0; i < 50; ++i){
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
			yield return new WaitForSeconds(0.1f);
		}

		if (!isWin){
			Debug.Log ("lose");
		} 
	}
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

	IEnumerator startGame(){
		Vector2[] offsets = {new Vector2(-5, 0), new Vector2(-1, -3), new Vector2(3, 0), 
			new Vector2(-1, 4), new Vector2(-2, 3)};
		List<int> randomNumbers = new List<int>();
		for(int i = 0; i < 5; ++i){
			randomNumbers.Add (i);
		}
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
			girls[i].GetComponent<Person>().setStartPosition(startPosition);
			girls[i].GetComponent<Person>().lookAtChristmasTree();
		}

		yield return new WaitForSeconds(5.0f);
		phoneGUI.GetComponent<phoneDisplay> ().sendText (1);
		yield return new WaitForSeconds(5.0f);

		Grid[] g1 = {mapRepresentation.christmasTree, mapRepresentation.mappleStore, 
			mapRepresentation.wineSpirits, mapRepresentation.nailSalon, mapRepresentation.christmasTree};
		girls [0].GetComponent<Person> ().destinations = g1;
		girls [0].GetComponent<Person> ().moveInRoutine ();
	

		Grid[] g5 = {mapRepresentation.christmasTree, mapRepresentation.mappleStore, 
			mapRepresentation.wineSpirits, mapRepresentation.mappleStore, mapRepresentation.stationery};
		girls [4].GetComponent<Person> ().destinations = g5;
		girls [4].GetComponent<Person> ().moveInRoutine ();

		yield return new WaitForSeconds(4.0f);

		Grid[] g2 = {mapRepresentation.christmasTree, mapRepresentation.wineSpirits, 
			mapRepresentation.mappleStore, mapRepresentation.holeFood, mapRepresentation.holeFood};
		girls [1].GetComponent<Person> ().destinations = g2;
		girls [1].GetComponent<Person> ().moveInRoutine ();
		
		Grid[] g3 = {mapRepresentation.christmasTree, mapRepresentation.holeFood, 
			mapRepresentation.christmasTree, mapRepresentation.mappleStore, mapRepresentation.nailSalon};
		girls [2].GetComponent<Person> ().destinations = g3;
		girls [2].GetComponent<Person> ().moveInRoutine ();
		
		Grid[] g4 = {mapRepresentation.christmasTree, mapRepresentation.christmasTree, 
			mapRepresentation.mappleStore, mapRepresentation.stationery, mapRepresentation.wineSpirits};
		girls [3].GetComponent<Person> ().destinations = g4;
		girls [3].GetComponent<Person> ().moveInRoutine ();
	}
}

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject[] girls;
	public GameObject map;
	MapRepresentation mapRepresentation;

	// Use this for initialization
	void Start () {
		mapRepresentation = map.GetComponent<MapRepresentation> ();

		StartCoroutine ("startGame");
	}
	
	// Update is called once per frame
	void Update () {
		
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
		for (int i = 0; i < girls.Length; ++i) {
			Grid startPosition = mapRepresentation.getGrid((int)mapRepresentation.christmasTree.pointOfGrid.x-2+i, 
			                                               (int)mapRepresentation.christmasTree.pointOfGrid.y);
			girls[i].GetComponent<Person>().setStartPosition(startPosition);
		}

		Grid[] r5 = {mapRepresentation.christmasTree, mapRepresentation.mappleStore, 
			mapRepresentation.wineSpirits, mapRepresentation.nailSalon, mapRepresentation.christmasTree};
		girls [0].GetComponent<Person> ().destinations = r5;
		girls [0].GetComponent<Person> ().moveInRoutine ();

		Grid[] y5 = {mapRepresentation.christmasTree, mapRepresentation.wineSpirits, 
			mapRepresentation.mappleStore, mapRepresentation.holeFood, mapRepresentation.holeFood};
		girls [1].GetComponent<Person> ().destinations = y5;
		girls [1].GetComponent<Person> ().moveInRoutine ();

		Grid[] b5 = {mapRepresentation.christmasTree, mapRepresentation.holeFood, 
			mapRepresentation.christmasTree, mapRepresentation.mappleStore, mapRepresentation.nailSalon};
		girls [2].GetComponent<Person> ().destinations = b5;
		girls [2].GetComponent<Person> ().moveInRoutine ();

		Grid[] g5 = {mapRepresentation.christmasTree, mapRepresentation.christmasTree, 
			mapRepresentation.mappleStore, mapRepresentation.stationery, mapRepresentation.wineSpirits};
		girls [3].GetComponent<Person> ().destinations = g5;
		girls [3].GetComponent<Person> ().moveInRoutine ();

		Grid[] p5 = {mapRepresentation.christmasTree, mapRepresentation.wineSpirits, 
			mapRepresentation.mappleStore, mapRepresentation.nailSalon, mapRepresentation.mappleStore};
		girls [4].GetComponent<Person> ().destinations = p5;
		girls [4].GetComponent<Person> ().moveInRoutine ();
		yield return new WaitForSeconds(1.0f);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Person : MonoBehaviour {
	public Grid lastWaypoint;
	public Grid currentPosition;
	public Stack path;
	public GameObject map;
	MapRepresentation mapRepresentation;
	public int priority;
	public Grid[] destinations;
	int nextDestination = 1;
	public bool isFinishedMoving = false;
	public bool isCheating;
	public GameObject phoneGUI;

	public enum ActionState{
		Walk,
		Wander,
		Idle
	};
	void Start () {
		path = new Stack ();

		mapRepresentation = map.GetComponent<MapRepresentation> ();
	}

	public void setStartPosition(Grid start){
		currentPosition = start;
		start.isOccupied = true;
		transform.position = new Vector3 (currentPosition.pointOfGrid.x, 0.98f, currentPosition.pointOfGrid.y);
	}

	public void moveTo(Grid destination){
		mapRepresentation.findShortestPath (this, destination);
		StartCoroutine ("move");
	}

	public void activate(){
		StartCoroutine ("doAction");
	}
		
	IEnumerator doAction(){
		System.Array values = System.Enum.GetValues(typeof(ActionState));
		System.Random random = new System.Random();

		ActionState randomActionState = (ActionState)values.GetValue(random.Next(values.Length));

		switch (randomActionState)
		{
		case ActionState.Walk:
			moveTo(mapRepresentation.getRandomPlace());
			break;
		case ActionState.Wander:
			StartCoroutine ("wander");
			break;
		case ActionState.Idle:
			StartCoroutine ("idle");
			break;
		default:
			break;
		}
		yield return new WaitForSeconds(0.1f);
	}

	public void moveInRoutine(){
		moveTo(destinations[nextDestination]);	
	}

	IEnumerator move(){
		while(path.Count > 0){
			Grid nextStep = (Grid)path.Pop();
			/*if(!nextStep.isOccupied){
				nextStep.isOccupied = true;
				transform.position = new Vector3 (nextStep.pointOfGrid.x, 0.5f, nextStep.pointOfGrid.y);
				currentPosition.isOccupied = false;
				currentPosition = nextStep;
				yield return new WaitForSeconds(0.1f);
			} else{
				break;
			}*/
			Vector3 nextPosition = new Vector3 (nextStep.pointOfGrid.x, 0.98f, nextStep.pointOfGrid.y);
			transform.rotation = Quaternion.LookRotation(nextPosition-transform.position);
			while(Vector3.Distance(transform.position, nextPosition) > 0.5f){
				//transform.position = Vector3.Lerp (transform.position, nextPosition, 2 * Time.deltaTime);
				transform.position = Vector3.MoveTowards(transform.position, nextPosition, 2 * Time.deltaTime);
				yield return null;
			}
			currentPosition = nextStep;
		}
		//StartCoroutine ("doAction");
		if(++nextDestination < destinations.Length){
			yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));

			moveTo(destinations[nextDestination]);
			if(isCheating){
				switch (nextDestination)
				{
				case 2:
					phoneGUI.GetComponent<phoneDisplay> ().sendText (2);
					break;
				case 3:
					phoneGUI.GetComponent<phoneDisplay> ().sendText (3);
					break;
				case 4:
					phoneGUI.GetComponent<phoneDisplay> ().sendText (4);
					break;
				}
			}
		}else{
			isFinishedMoving = true;
		}
	}

	public void lookAtChristmasTree(){
		Vector3 christmasTreePosition = new Vector3 (mapRepresentation.christmasTree.pointOfGrid.x, 0.98f, 
		                                    mapRepresentation.christmasTree.pointOfGrid.y);
		transform.rotation = Quaternion.LookRotation(christmasTreePosition-transform.position);
	}

	IEnumerator wander(){
		mapRepresentation.getFourAdjacentGrid (currentPosition);
		yield return new WaitForSeconds(0.1f);
		StartCoroutine ("doAction");
	}
	IEnumerator idle(){
		mapRepresentation.getFourAdjacentGrid (currentPosition);
		yield return new WaitForSeconds(0.1f);
		StartCoroutine ("doAction");
	}
}

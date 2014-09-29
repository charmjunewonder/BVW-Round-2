using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Suspect : Person {


	public int priority;
	public Grid[] destinations;
	public int nextDestination = 0;
	public bool isFinishedMoving = false;
	public bool isCheating;
	public GameObject phoneGUI;
	public locationScript locbox;
	//public GameObject shinningLogo;
	public lifebarwrapper lifebar;

	public GameObject cube;
	public GameObject ObstacleGroup;
	GameObject cubeA;

	public enum ActionState{
		Walk,
		Wander,
		Idle
	};

	public void resetLife(){
		lifebar.resetLife();
	}

	public bool isLifeBarFull(){
		return lifebar.getLife () < 0.1f;
	}

	public float getLife(){
		return lifebar.getLife ();
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
			GetComponent<Animator>().SetBool("startWalk",true);

			moveTo(mapRepresentation.getRandomPlace());
			break;
		case ActionState.Wander:
			StartCoroutine ("wander");
			break;
		case ActionState.Idle:
			GetComponent<Animator>().SetBool("startWalk",false);

			StartCoroutine ("idle");
			break;
		default:
			break;
		}
		yield return new WaitForSeconds(0.1f);
	}
	
	public void moveInRoutine(){
		cubeA = Instantiate(cube) as GameObject;
		moveTo(destinations[nextDestination]);	
	}
	
	IEnumerator move(){
		while(path.Count > 0){
			Grid nextStep = (Grid)path.Pop();
			if(!nextStep.isOccupied){
				nextStep.isOccupied = true;
			} else{
				nextStep = mapRepresentation.getRandomNearnby(nextStep);


			}
//			cubeA.transform.position = new Vector3(nextStep.pointOfGrid.x, 0.5f, nextStep.pointOfGrid.y);
//			cubeA.transform.parent = ObstacleGroup.transform;
			Vector3 nextPosition = new Vector3 (nextStep.pointOfGrid.x, 0.98f, nextStep.pointOfGrid.y);
			transform.rotation = Quaternion.LookRotation(nextPosition-transform.position);
			while(Vector3.Distance(transform.position, nextPosition) > 0.5f){
				transform.position = Vector3.MoveTowards(transform.position, nextPosition, 2 * Time.deltaTime);
				yield return null;
			}
			currentPosition.isOccupied = false;
			currentPosition = nextStep;
	
		}
		if(isCheating){
			locbox.SendText(mapRepresentation.locationMatrix [4, nextDestination]);
		}
		if(++nextDestination < destinations.Length){
			yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));



			moveTo(destinations[nextDestination]);
			yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));
			
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

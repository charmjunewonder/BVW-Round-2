using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Passenger : Person {
	


	
	public void moveTo(Grid destination){
		mapRepresentation.findShortestPath (this, destination);
		StartCoroutine ("move");
	}
	
	public void moveInRoutine(){
		moveTo(mapRepresentation.getRandomPlace());	
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
		yield return new WaitForSeconds(Random.Range(3.0f, 4.0f));
		moveTo(mapRepresentation.getRandomPlace());

	}

}

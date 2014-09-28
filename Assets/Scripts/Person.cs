using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Person : MonoBehaviour {
	public Grid currentPosition;
	public Stack path;
	public GameObject map;
	public MapRepresentation mapRepresentation;

	void Start () {
		path = new Stack ();
		mapRepresentation = map.GetComponent<MapRepresentation> ();
	}


	public void setStartPosition(Grid start){
		currentPosition = start;
		start.isOccupied = true;
		transform.position = new Vector3 (currentPosition.pointOfGrid.x, 0.98f, currentPosition.pointOfGrid.y);
	}


}

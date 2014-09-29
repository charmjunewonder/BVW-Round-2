using UnityEngine;
using System.Collections;

public class mittenscript : MonoBehaviour {
	public GameObject hand;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = hand.transform.position;
	}
}

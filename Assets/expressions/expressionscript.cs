using UnityEngine;
using System.Collections;

public class expressionscript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3 (0, 0.0f, 0);
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
		                Camera.main.transform.rotation * Vector3.up);
		transform.localPosition = new Vector3 (0, 1.3f, 0);
	}
}

using UnityEngine;
using System.Collections;

public class lifebarwrapper : MonoBehaviour {
	public lifebarscript lifebar;
	//public GameObject outline;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//outline.renderer.enabled = !isEmpty ();


		transform.localPosition = new Vector3 (0, 0.0f, 0);
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
		                 Camera.main.transform.rotation * Vector3.up);
		transform.localPosition = new Vector3 (0, 0.06f, 0);
	}

	public void reset()
	{
		reset ();
	}

	public void resetLife(){
		lifebar.reset ();
	}

	public float getLife()
	{
		return lifebar.life ();
	}

	public void addLife()
	{
		lifebar.AddLife ();
	}

	public void minusLife()
	{
		lifebar.RemoveLife ();
	}

	public bool isEmpty()
	{
		return lifebar.isEmpty ();
	}

	public bool isFull()
	{
		return lifebar.isFull ();
	}
}

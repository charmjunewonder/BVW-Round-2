using UnityEngine;
using System.Collections;

public class lifebarscript : MonoBehaviour {

	const float BASE_ALPHA = 0.01f;
	const float ALPHA_STEP = 0.25f;
	float alpha;
	// Use this for initialization
	void Start () {

		//alpha = 1.0f;
		//gameObject.renderer.material.SetFloat ("_Cutoff", alpha);
		//StartCoroutine (delayReset ());
		reset ();

		//RemoveLife ();
	}

	IEnumerator delayReset()
	{
		for (float i=1.0f; i>BASE_ALPHA; i-=Time.deltaTime) 
		{
			alpha = i;
			gameObject.renderer.material.SetFloat ("_Cutoff", alpha);
			yield return null;
		}
		yield return new WaitForSeconds (3.0f);
		for (float i=BASE_ALPHA; i<1.0; i+=Time.deltaTime) 
		{
			alpha = i;
			gameObject.renderer.material.SetFloat ("_Cutoff", alpha);
			yield return null;
		}
		reset ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void RemoveLife()
	{
		alpha += ALPHA_STEP * Time.deltaTime;
		alpha = Mathf.Min (alpha, 1.0f);
		gameObject.renderer.material.SetFloat ("_Cutoff", alpha);
	}

	public void AddLife()
	{

		alpha -= ALPHA_STEP * Time.deltaTime;
		alpha = Mathf.Max (BASE_ALPHA, alpha);
		gameObject.renderer.material.SetFloat ("_Cutoff", alpha);
	}

	public void reset()
	{
		alpha = 1.0f;
		gameObject.renderer.material.SetFloat ("_Cutoff", alpha);
	}

	public float life()
	{
		return alpha;
	}

	public bool isEmpty()
	{
		return alpha > 0.9999f;
	}

	public bool isFull()
	{
		return alpha < (BASE_ALPHA + 0.00001);
	}
}

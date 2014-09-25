using UnityEngine;
using System.Collections;

public class lifebarscript : MonoBehaviour {

	const float BASE_ALPHA = 0.01f;
	const float ALPHA_STEP = 0.25f;
	float alpha;
	// Use this for initialization
	void Start () {
		reset ();
		//RemoveLife ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void RemoveLife()
	{
		alpha += ALPHA_STEP * 2 * Time.deltaTime;
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
}

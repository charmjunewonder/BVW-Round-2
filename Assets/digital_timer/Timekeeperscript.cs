using UnityEngine;
using System.Collections;

public class Timekeeperscript : MonoBehaviour {
	public TextMesh textobj;
	const int START_MIN = 5;
	const int START_SEC = 0;
	float internal_time;
	bool running;
	// Use this for initialization
	void Start () {
		reset ();
		Run (300);
	}
	
	// Update is called once per frame
	void Update () {
		if(running)
		{
			internal_time -= Time.deltaTime;
			if(internal_time < 0)
			{
				//reset ();
				internal_time = 0;
				running = false;
			}

			textobj.text = Mathf.FloorToInt(internal_time / 60.0f).ToString() + ":" + Mathf.FloorToInt(internal_time % 60).ToString("D2");
		}
	}

	public void Run(int seconds)
	{
		internal_time = seconds;
		running = true;
	}

	public void Stop()
	{
		running = false;
		textobj.text = Mathf.FloorToInt(internal_time / 60.0f).ToString() + ":" + Mathf.FloorToInt(internal_time % 60).ToString();
	}

	public void reset()
	{
		internal_time = 0;//START_MIN * 60 + START_SEC;
		running = false;
	}
}

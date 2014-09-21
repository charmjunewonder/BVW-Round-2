using UnityEngine;
using System.Collections;
using System;

public class InputController : MonoBehaviour {
	public KinectPointController pointskel;
	public GameObject window;
	public GameObject main_cam;
	public GameObject christmas_tree;


	private Texture2D foggytex;
	private Texture2D initialtex;

	const float HEIGHT_OFFSET = 20;
	public const int WIPE_RADIUS = 15;
	const bool USE_MOUSE = true;
	const bool USE_LEFT = false;
	const bool USE_RIGHT = false;

	const float NO_MIST_TIME = 1.0f;
	const float MIST_UP_TIME = 3.0f;

	int[,] semaphore;
	int semaphore_index;

	Vector2 left_prev_pos;
	Vector2 right_prev_pos;
	Vector2 left_pos;
	Vector2 right_pos;

	Vector2 mouse_pos;
	Vector2 mouse_prev_pos;


	bool scaled;
	bool all_tracked;

	int[] leftlimb = {
		(int)Kinect.NuiSkeletonPositionIndex.HipLeft,
		(int)Kinect.NuiSkeletonPositionIndex.KneeLeft,
		(int)Kinect.NuiSkeletonPositionIndex.AnkleLeft,
		(int)Kinect.NuiSkeletonPositionIndex.FootLeft
	};
	
	int[] rightlimb = {
		(int)Kinect.NuiSkeletonPositionIndex.HipRight,
		(int)Kinect.NuiSkeletonPositionIndex.KneeRight,
		(int)Kinect.NuiSkeletonPositionIndex.AnkleRight,
		(int)Kinect.NuiSkeletonPositionIndex.FootRight
	};
	
	int[] leftarm = {
		(int)Kinect.NuiSkeletonPositionIndex.ShoulderLeft,
		(int)Kinect.NuiSkeletonPositionIndex.ElbowLeft,
		(int)Kinect.NuiSkeletonPositionIndex.WristLeft,
		(int)Kinect.NuiSkeletonPositionIndex.HandLeft
	};
	
	int[] rightarm = {
		(int)Kinect.NuiSkeletonPositionIndex.ShoulderRight,
		(int)Kinect.NuiSkeletonPositionIndex.ElbowRight,
		(int)Kinect.NuiSkeletonPositionIndex.WristRight,
		(int)Kinect.NuiSkeletonPositionIndex.HandRight
	};

	// Use this for initialization
	void Start () {
		//Debug.Log (height());
		//Debug.Log (range ());
		//adjust_skel_to_fit_screen ();
		resize_window_plane ();
		initialtex = (Texture2D)Instantiate(window.renderer.material.mainTexture);
		foggytex = (Texture2D)Instantiate(window.renderer.material.mainTexture);
		window.renderer.material.mainTexture = foggytex;

		semaphore = new int[foggytex.width, foggytex.height];
		for(int i=0;i<foggytex.width;i++)
			for(int j=0;j<foggytex.height;j++)
				semaphore[i,j] = 0;

		semaphore_index = 0;
		scaled = false;

		left_prev_pos = new Vector2 (0, 0);
		right_prev_pos = new Vector2 (0, 0);
		left_pos  = new Vector2 (0, 0);
		right_pos  = new Vector2 (0, 0);
		mouse_pos = new Vector2 (0, 0);
		mouse_prev_pos = new Vector2 (0, 0);
		all_tracked = false;

		Vector3 startwipe = main_cam.camera.WorldToScreenPoint (christmas_tree.transform.position);
		Vector2 sw_pos = new Vector2 ();
		sw_pos.x = Screen.width - startwipe.x;
		sw_pos.y = Screen.height - startwipe.y;
		wipearea (sw_pos, 80, 3.0f, 3.0f);
	}

	bool check_tracked()
	{
		for (int i=0; i<20; i++)
						if (pointskel.sw.boneState [0, i] == Kinect.NuiSkeletonPositionTrackingState.NotTracked)
								return false;
		return true;
	}

	void adjust_skel_to_fit_screen()
	{
		float range_val = range ();
		//Debug.Log (range_val);
		float distance = ( (range_val * 0.5f) / Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) );
		float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;

		float cam_to_skel = (main_cam.transform.position - pointskel.transform.position).magnitude;
		//Debug.Log (distance + "  " + cam_to_skel);
		pointskel.scale = cam_to_skel/distance;
		//Vector3 pos = pointskel.transform.position;
		//pos.z = -distance;
		//pointskel.transform.position = pos;
		scaled = true;
	}

	void resize_window_plane()
	{
		float distance = (main_cam.transform.position - window.transform.position).magnitude;
		float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
		float width = height * Screen.width / Screen.height;

		window.transform.localScale = new Vector3 (width * 0.1f, 1, height * 0.1f);
	}
	
	// Update is called once per frame
	void Update () {

		if (!all_tracked && check_tracked ())
			all_tracked = true;


		if (USE_MOUSE) {
						Vector2 mousey = Input.mousePosition;
						mousey.y = Screen.height - mousey.y;
						mousey.x = Screen.width - mousey.x;

						mouse_prev_pos = mouse_pos;
						mouse_pos = mousey;
						float diff = (mouse_pos - mouse_prev_pos).magnitude;
						Vector2 dir = (mouse_pos - mouse_prev_pos).normalized;
						for (float i=0; i<diff; i+= WIPE_RADIUS) {
								wipearea (mouse_prev_pos + dir * i,WIPE_RADIUS,NO_MIST_TIME,MIST_UP_TIME);
						}
				}

	

		if (all_tracked && USE_LEFT) 
		{
						Vector3 lefthandscreenpos = main_cam.camera.WorldToScreenPoint (pointskel.Hand_Left.transform.position);
						lefthandscreenpos.y = Screen.height - lefthandscreenpos.y;
						lefthandscreenpos.x = Screen.width - lefthandscreenpos.x;


						left_prev_pos = left_pos;
						left_pos.x = lefthandscreenpos.x;
						left_pos.y = lefthandscreenpos.y;

						float diff = (left_pos - left_prev_pos).magnitude;
						Vector2 dir = (left_pos - left_prev_pos).normalized;
						for (float i=0; i<diff; i+= WIPE_RADIUS) {
						wipearea (left_prev_pos + dir * i,WIPE_RADIUS,NO_MIST_TIME,MIST_UP_TIME);
						}
		}

		if (all_tracked && USE_RIGHT) 
		{
			Vector3 righthandscreenpos = main_cam.camera.WorldToScreenPoint (pointskel.Hand_Right.transform.position);
			righthandscreenpos.y = Screen.height - righthandscreenpos.y;
			righthandscreenpos.x = Screen.width - righthandscreenpos.x;
			
			
			right_prev_pos = right_pos;
			right_pos.x = righthandscreenpos.x;
			right_pos.y = righthandscreenpos.y;
			
			float diff = (right_pos - right_prev_pos).magnitude;
			Vector2 dir = (right_pos - right_prev_pos).normalized;
			for (float i=0; i<diff; i+= WIPE_RADIUS) {
				wipearea (right_prev_pos + dir * i,WIPE_RADIUS,NO_MIST_TIME,MIST_UP_TIME);
			}
		}

		//if(!scaled && check_tracked())
		//	adjust_skel_to_fit_screen();

		((Texture2D)window.renderer.material.mainTexture).Apply ();
	}


	void wipearea(Vector2 screenpos, int wipe_radius, float nomist_time, float mistup_time)
	{

		Texture2D tex = (Texture2D)window.renderer.material.mainTexture;

		Vector3 pos = main_cam.camera.ScreenToWorldPoint (new Vector3 (screenpos.x, screenpos.y, main_cam.camera.nearClipPlane));
		//Debug.Log (pos.x.ToString("f6") +  " " + pos.y.ToString("f6") + pos.z.ToString("f6"));

		int texpos_x = Mathf.FloorToInt((float)screenpos.x * (float)tex.width / (float)Screen.width);
		int texpos_y = Mathf.FloorToInt ((float)screenpos.y * (float)tex.height / (float)Screen.height);

		//Debug.Log (screenpos.ToString()+ texpos_x.ToString() + " " + texpos_y.ToString());

		for (int i=texpos_x - wipe_radius; i<texpos_x + wipe_radius; i++)
		for (int j=texpos_y - wipe_radius; j<texpos_y + wipe_radius; j++) {
			Vector2 p = new Vector2(texpos_x - i,texpos_y - j);
			if(p.magnitude < wipe_radius &&
			   (i>0 && j>0 && i<tex.width && j<tex.height))
			{
				tex.SetPixel (i, j, new Color (0.0f, 0.0f, 0.0f, 0f));
				semaphore[i,j] ++;
			}
						}

		tex.Apply ();

		StartCoroutine(recoverMist (screenpos,wipe_radius,nomist_time,mistup_time));
	}

	IEnumerator recoverMist(Vector2 screenpos, int wipe_radius, float nomist_time, float mistup_time)
	{
		//bool[,] map = new bool[WIPE_RADIUS * 2, WIPE_RADIUS * 2];

		Texture2D tex = (Texture2D)window.renderer.material.mainTexture;
		
		int texpos_x = Mathf.FloorToInt((float)screenpos.x * (float)tex.width / (float)Screen.width);
		int texpos_y = Mathf.FloorToInt ((float)screenpos.y * (float)tex.height / (float)Screen.height);

		semaphore_index ++;
		int semaphore_num = semaphore_index;

		for (int i=texpos_x - wipe_radius; i<texpos_x + wipe_radius; i++)
		for (int j=texpos_y - wipe_radius; j<texpos_y + wipe_radius; j++) {
			Vector2 p = new Vector2(texpos_x - i,texpos_y - j);
			if(p.magnitude < wipe_radius &&
			   (i>0 && j>0 && i<tex.width && j<tex.height))
			{
				semaphore [i, j]=semaphore_num;
			}
		}


		for (float  k = 0; k< nomist_time; k+= Time.deltaTime) 
		{
			yield return null;
		}


		for (float  k = 0; k< mistup_time + 0.1f; k+= Time.deltaTime) 
		{			
			for (int i=texpos_x - wipe_radius; i<texpos_x + wipe_radius; i++)
			for (int j=texpos_y - wipe_radius; j<texpos_y + wipe_radius; j++) {
				Vector2 p = new Vector2(texpos_x - i,texpos_y - j);
				if(p.magnitude < wipe_radius &&
				   (i>0 && j>0 && i<tex.width && j<tex.height))
				{
					Color col = initialtex.GetPixel(i,j);

					if(semaphore[i,j] > semaphore_num) continue;

					tex.SetPixel (i, j, new Color (col.r, col.g, col.b, k/mistup_time));
				}
			}
			
			//tex.Apply ();

			yield return null;
		}


	}


	float height()
	{
		Vector3 headpos = pointskel.Head.transform.position;
		Vector3 shoulderpos = pointskel.Shoulder_Center.transform.position;
		Vector3 spinepos = pointskel.Spine.transform.position;
		Vector3 hipcenterpos = pointskel.Hip_Center.transform.position;

		Vector3 hipleftpos = pointskel.Hip_Left.transform.position;
		Vector3 kneeleftpos = pointskel.Knee_Left.transform.position;
		Vector3 ankleleftpos = pointskel.Ankle_Left.transform.position;
		Vector3 footleftpos = pointskel.Foot_Left.transform.position;

		Vector3 hiprightpos = pointskel.Hip_Right.transform.position;
		Vector3 kneerightpos = pointskel.Knee_Right.transform.position;
		Vector3 anklerightpos = pointskel.Ankle_Right.transform.position;
		Vector3 footrightpos = pointskel.Foot_Right.transform.position;


		float h = 0;

		h += (headpos - shoulderpos).magnitude;
		h += (shoulderpos - spinepos).magnitude;
		h += (spinepos - hipcenterpos).magnitude;

		if (count_tracked (leftlimb) > count_tracked (rightlimb)) {
			
						h += (hipleftpos - kneeleftpos).magnitude;
						h += (kneeleftpos - ankleleftpos).magnitude;
						h += (ankleleftpos - footleftpos).magnitude;
		} else {
			h += (hiprightpos - kneerightpos).magnitude;
			h += (kneerightpos - anklerightpos).magnitude;
			h += (anklerightpos - footrightpos).magnitude;
		}

		return h + HEIGHT_OFFSET;

	}

	float range()
	{
		Vector3 headpos = pointskel.Head.transform.position;
		Vector3 shoulderpos = pointskel.Shoulder_Center.transform.position;
		Vector3 spinepos = pointskel.Spine.transform.position;
		Vector3 hipcenterpos = pointskel.Hip_Center.transform.position;
		
		Vector3 hipleftpos = pointskel.Hip_Left.transform.position;
		Vector3 kneeleftpos = pointskel.Knee_Left.transform.position;
		Vector3 ankleleftpos = pointskel.Ankle_Left.transform.position;
		Vector3 footleftpos = pointskel.Foot_Left.transform.position;
		
		Vector3 hiprightpos = pointskel.Hip_Right.transform.position;
		Vector3 kneerightpos = pointskel.Knee_Right.transform.position;
		Vector3 anklerightpos = pointskel.Ankle_Right.transform.position;
		Vector3 footrightpos = pointskel.Foot_Right.transform.position;

		Vector3 shoulderleftpos = pointskel.Shoulder_Left.transform.position;
		Vector3 elbowleftpos = pointskel.Elbow_Left.transform.position;
		Vector3 wristleftpos = pointskel.Wrist_Left.transform.position;
		Vector3 handleftpos = pointskel.Hand_Left.transform.position;

		Vector3 shoulderrightpos = pointskel.Shoulder_Right.transform.position;
		Vector3 elbowrightpos = pointskel.Elbow_Right.transform.position;
		Vector3 wristrightpos = pointskel.Wrist_Right.transform.position;
		Vector3 handrightpos = pointskel.Hand_Right.transform.position;

		
		float h = 0;

		h += (shoulderpos - spinepos).magnitude;
		h += (spinepos - hipcenterpos).magnitude;

		
		if (count_tracked (leftlimb) > count_tracked (rightlimb)) {
			
			h += (hipleftpos - kneeleftpos).magnitude;
			h += (kneeleftpos - ankleleftpos).magnitude;
			h += (ankleleftpos - footleftpos).magnitude;
		} else {
			h += (hiprightpos - kneerightpos).magnitude;
			h += (kneerightpos - anklerightpos).magnitude;
			h += (anklerightpos - footrightpos).magnitude;
		}

		if (count_tracked (leftarm) > count_tracked (rightarm)) {
			
			h += (shoulderleftpos - elbowleftpos).magnitude;
			h += (elbowleftpos - wristleftpos).magnitude;
			h += (wristleftpos - handleftpos).magnitude;
		} else {
			h += (shoulderrightpos - elbowrightpos).magnitude;
			h += (elbowrightpos - wristrightpos).magnitude;
			h += (wristrightpos - handrightpos).magnitude;
		}

		
		return h;

	}


	int count_tracked(int[] joint_list)
	{
		int tracked = 0;
		for (int i=0; i<joint_list.Length; i++)
						if (pointskel.sw.boneState [0,joint_list [i]] == Kinect.NuiSkeletonPositionTrackingState.Tracked)
								tracked++;
		return tracked;
	}

}

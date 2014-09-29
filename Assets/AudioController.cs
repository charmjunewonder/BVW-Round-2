using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	public AudioSource[] audios;

	public void turnOffSound(){
		foreach(AudioSource audio in audios){
			audio.mute = true;
		}
	}

	public void turnOnSound(){
		foreach(AudioSource audio in audios){
			audio.mute = false;
		}
	}
}

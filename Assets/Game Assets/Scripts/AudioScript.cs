using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {

	private GameManagerCS gm;

	public AudioClip musicSource;
	private bool isPlaying;

	void awake(){
		gm = GameObject.Find ("GameManager").GetComponent<GameManagerCS>();
	}

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().clip = musicSource;
	}
	
	// Update is called once per frame
	void Update () {
	if (gm.gameRunning)
			GetComponent<AudioSource>().Stop();
		else
		{
			if (!isPlaying)
			{
				PlayAudio();
			}
		}
	}

	void PlayAudio()
	{
		GetComponent<AudioSource>().Play();
		isPlaying = true;
	}
}

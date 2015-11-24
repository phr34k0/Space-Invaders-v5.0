using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ScoreCS : MonoBehaviour {

    private GameManagerCS gm;
    Text txt;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }
    
    // Use this for initialization
    void Start () {
        txt = gameObject.GetComponent<Text>();
        txt.text = gm.gameScore.ToString("0000");
    }
	
	// Update is called once per frame
	void Update () {
        txt = gameObject.GetComponent<Text>();
        txt.text = gm.gameScore.ToString("0000");
    }
}

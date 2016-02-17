using UnityEngine;
using System.Collections;

public class SaucerCS : MonoBehaviour {

	public GameObject bombPrefab;
	private GameObject bombObject;
	public AudioClip[] saucerSound;

	public int value;
	public int type;
	public bool bombMade;
	private Vector3 saucerPos;

	private GameManagerCS gm;

	void Awake ()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
	}

	// Use this for initialization
	void Start () 
	{
		InitSaucer();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	void FixedUpdate()
	{
		MoveSaucer();

		if (!bombMade 
		    && (Random.Range(0.0f, 1.0f) < gm.bombPercent)
			&& (saucerPos.x > -gm.invaderBounds && saucerPos.x < gm.invaderBounds))
		{
			MakeBomb(saucerPos);
		}
	}

	void InitSaucer()
	{
		bombMade = false;
		GetComponent<AudioSource>().clip = saucerSound[(type % 2)];
		GetComponent<AudioSource>().Play();
	}

	void MoveSaucer()
	{
		saucerPos = this.transform.position;

		if(gm.saucerDirection == -1)
		{
			if (saucerPos.x < -gm.saucerBounds)
			{
				DestroySaucer();
			}
			else
			{
				saucerPos.x -= gm.saucerSpeed * Time.deltaTime;
			}
		}
		else if(gm.saucerDirection == 1)
		{
			if (saucerPos.x > gm.saucerBounds)
			{
				DestroySaucer();
			}
			else
			{
				saucerPos.x += gm.saucerSpeed * Time.deltaTime;
			} 
		}
		this.transform.position = saucerPos;
	}

	public void DestroySaucer()
	{
		gm.saucerMade = false;
		gm.saucerTimer = Time.time + Random.Range(5.0f, 10f);
		Destroy (this.gameObject);
	}

	void MakeBomb(Vector3 saucerPos)
	{
		bombObject = (GameObject)Instantiate(bombPrefab,
		                                        new Vector3(saucerPos.x, saucerPos.y - 0.5f, saucerPos.z),
		                                        Quaternion.identity);
		bombObject.name = "Bomb";
		bombObject.transform.parent = this.transform;
		bombMade = true;
	}
}

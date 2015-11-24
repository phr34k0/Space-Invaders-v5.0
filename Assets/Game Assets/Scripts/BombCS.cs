using UnityEngine;
using System.Collections;

public class BombCS : MonoBehaviour {

	private Vector3 bombPos;
	
	public GameObject parentObj;
	private GameManagerCS gm;
	
	public GameObject explosionPrefab;
	private GameObject explosionObject;
	
	public float explosionTime;
	
	void Awake()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
	}
	
	// Use this for initialization
	void Start () 
	{
		parentObj = this.transform.parent.gameObject; 
		this.transform.parent = null;
	}

	// Update is called once per frame
	void Update () 
	{
		MoveBomb ();
	}

	void MoveBomb()
	{
		bombPos = this.transform.position;
		bombPos.y -= gm.bombSpeed;
		if (bombPos.y < gm.missileMin)
			DestroyBomb();
		
		this.transform.position = bombPos;
	}

	void DestroyBomb()
	{
		if (parentObj != null)
		{
			parentObj.GetComponent<SaucerCS>().bombMade = false;
		}
		MakeExplosion(this.transform.position);
		Destroy(this.gameObject);
	}

	void MakeExplosion(Vector3 explosionPos)
	{
		explosionObject = (GameObject)Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
		explosionObject.name = "Explosion";
		Destroy(explosionObject, explosionTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			//Debug.Log("Bullet hit: " + other.gameObject.name);
			gm.playerDead = true;
			gm.playerLives--;
			Destroy(other.gameObject);
			
			DestroyBomb();
		}
		else if(other.gameObject.tag == "Shelter")
		{
			Destroy(other.gameObject);
			DestroyBomb();
		}
	}
}

using UnityEngine;
using System.Collections;

public class MissileCS : MonoBehaviour {

    private Vector3 missilePos;

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
	void Start () {
        parentObj = this.transform.parent.gameObject; 
        this.transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
        MoveMissile();
	}

    void MoveMissile()
    {
        missilePos = this.transform.position;
        missilePos.y -= gm.missileSpeed;
        if (missilePos.y < gm.missileMin)
            DestroyMissile();

        this.transform.position = missilePos;

    }

    void DestroyMissile()
    {
        if (parentObj != null)
        {
            parentObj.GetComponent<EnemyCS>().missileMade = false;
        }
        MakeExplosion(this.transform.position);
        gm.totalMissiles--;
        if (gm.totalMissiles <= 0) gm.totalMissiles = 0;
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

            DestroyMissile();
        }
        else if(other.gameObject.tag == "Shelter")
        {
            Destroy(other.gameObject);
            DestroyMissile();
        }
    }
}

using UnityEngine;
using System.Collections;

public class BulletCS : MonoBehaviour {

    private Vector3 bulletPos;
    public ShooterCS shooter;
    private GameManagerCS gm;

    public GameObject explosionPrefab;
    private GameObject explosionObject;

    public float explosionTime;

	public AudioClip impactSound;
	public AudioClip bombimpactSound;

    void Awake()
    {
        shooter = GameObject.Find("Player").GetComponent<ShooterCS>();
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        MoveBullet();
	}

    void MoveBullet()
    {
        bulletPos = this.transform.position;
        bulletPos.y += gm.bulletSpeed;
        if (bulletPos.y > gm.missileMax)
            DestroyBullet();

        this.transform.position = bulletPos;
       
    }

    void DestroyBullet()
    {
        shooter.bulletMade = false;
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
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.LogError("Bullet hit: " + other.gameObject.name);
            gm.gameScore += other.gameObject.GetComponent<EnemyCS>().value;
            Destroy(other.gameObject);
			AudioSource.PlayClipAtPoint(impactSound, transform.position, 0.5f);
			gm.CheckLevel();
            DestroyBullet();
        }
		else if (other.gameObject.tag == "Saucer")
		{
			//Debug.LogError("Bullet hit: " + other.gameObject.name);
			gm.gameScore += other.gameObject.GetComponent<SaucerCS>().value;
			Destroy(other.gameObject);
			AudioSource.PlayClipAtPoint(impactSound, transform.position, 0.5f);
			DestroyBullet();
			gm.saucerMade = false;
		}
        else if (other.gameObject.tag == "Shelter")
        {
            Destroy(other.gameObject);
            DestroyBullet();
        }
    }
}


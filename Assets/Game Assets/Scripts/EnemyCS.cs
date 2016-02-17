using UnityEngine;
using System.Collections;

public class EnemyCS : MonoBehaviour {

    private Vector3 invaderPos;
    public bool moveDown;
	//public bool moveStep;

    public GameObject missilePrefab;
    private GameObject missileObject;
    private GameManagerCS gm;
    
    public bool isBottom;
    public string hitName;
    
    public bool missileMade;
    public int value;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }

	// Use this for initialization
	void Start () {
        InitInvader();
	}
	
    void InitInvader()
    {
        missileMade = false;
        moveDown = false;
        isBottom = false;
    }

	// Update is called once per frame
	void Update () {
		MoveInvader();
	}

    void LateUpdate()
    {
        if (hitName.Contains("Invader"))
        {
            isBottom = false;
        }
        else
        {
            isBottom = true;
        }
    }

    void FixedUpdate()
    {
		/*if(moveStep)
		{
			moveStep = false;
			MoveInvader();
		}*/

        CheckBottom();
        if (isBottom && !missileMade && gm.totalMissiles < gm.maxMissiles && Random.Range(0, 10) < 0.1)
        {
			missileMade = true;
            MakeMissile(invaderPos);
        }
    }

    void MoveInvader()
    {
        invaderPos = this.transform.position;
 
        if(gm.invaderDirection == -1)
        {
            if (invaderPos.x < -gm.invaderBounds)
            {
                invaderPos.x = -gm.invaderBounds;
                gm.invaderDirection = 1;
                gm.moveDown = true;
            }
            else
            {
				//invaderPos.x += gm.invaderSpeed * Time.deltaTime;
                invaderPos.x -= gm.invaderSpeed * (gm.maxInvaders/ gm.currentInvaders) * gm.currentLevel * 0.01f;
            }
        }
        else if(gm.invaderDirection == 1)
        {
            if (invaderPos.x > gm.invaderBounds)
            {
                invaderPos.x = gm.invaderBounds;
                gm.invaderDirection = -1;
                gm.moveDown = true;         
            }
            else
            {
                //invaderPos.x += gm.invaderSpeed * Time.deltaTime;
				invaderPos.x += gm.invaderSpeed * (gm.maxInvaders/ gm.currentInvaders) * gm.currentLevel * 0.01f;
            } 
        }

        if (invaderPos.y < gm.invaderBottom)
        {
            gm.playerDead = true;
            gm.playerLives--;
            Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
            gm.gameOver = true;
        }

        if (moveDown)
        {
            
            invaderPos.y -= 0.25f;
            moveDown = false;  
        }
        this.transform.position = invaderPos;
    }


    void MakeMissile(Vector3 enemyPos)
    {
        missileObject = (GameObject)Instantiate(missilePrefab,
                                               new Vector3(enemyPos.x, enemyPos.y - 0.5f, enemyPos.z),
                                               Quaternion.identity);
        missileObject.name = "Missile";
        missileObject.transform.parent = this.transform;
        missileMade = true;
        gm.totalMissiles++;
    }

    void CheckBottom()
    {
        RaycastHit2D hit = Physics2D.Raycast((new Vector2(this.transform.position.x, 
                                                          this.transform.position.y - 0.25f)),
                                                          -Vector2.up);

        hitName = string.Empty;
        if (hit.collider != null)
        {
            hitName = hit.collider.gameObject.name;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shelter")
        {
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Player")
        {
            gm.playerDead = true;
            gm.playerLives--;
            Destroy(other.gameObject);
            gm.gameOver = true;
        }
    }
}

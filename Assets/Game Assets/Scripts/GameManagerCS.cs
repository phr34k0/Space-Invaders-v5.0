//
//
// Coded By C-zaR
//
using UnityEngine;
using System.Collections;

public class GameManagerCS : MonoBehaviour {

    public GameObject playerPrefab;
    private GameObject playerObject;
   
    public GameObject[] enemyPrefab;
    private GameObject[] enemyObject;
    public int[] enemyValue;

    public GameObject groundPrefab;
    private GameObject groundObject;

	public GameObject saucerPrefab;
	private GameObject saucerObject;

	private GameObject bulletObject;
	private GameObject bombObject;
	private GameObject[] missileObject;
	private GameObject[] explosionObject;

    public GameObject shelterPrefab;
    private GameObject[] shelterObject;

    public Vector3 playerPos = Vector3.zero;
    public Vector3 enemyPos = Vector3.zero;
    public Vector3 groundPos = Vector3.zero;
	public Vector3 saucerPos = Vector3.zero;

    public GameObject gameScreenPrefab;
    private GameObject gameScreen;

    private GameObject scoreScreen;
    private GameObject gameGUI;

    public GameObject[] gameLife;

    public float shelterPos = 0f;
    public float shelterSpread = 2.25f;
    private int shelterNr; 

    public float playerSpeed;
    private int playerDirection;
    public float playerBounds;
    public bool playerMade;

    public float invaderBounds;
    public float invaderSpeed;
    public int invaderDirection;
    public float invaderBottom;

    public float missileMin;
    public float missileMax;

    public bool moveDown;
    public int totalMissiles;
    public int maxMissiles;

	public float bombPercent;
	//public float saucerPercent;
	public float missilePercent;

    public float missileSpeed;
    public float bulletSpeed;
	public float bombSpeed;

	public int saucerType;
	public int saucerScore;
	public bool saucerMade;
	public int saucerDirection;
	public float saucerSpeed;
	public float saucerBounds;

	//public int backgroundDirection;
	//public float backgroundSpeed;
	//private Vector3 backgroundPos;

    public bool playerDead;
    public float playerRespawn;
    private int invaderNr;

    public int playerLives;
	public int startLives;

    public int maxLives;
    public int rowSize;
    public float rowSpacing;
    public bool gameOver;
    public bool gameStart;
    public bool gameRunning;
	public bool levelClear;
	public int currentInvaders;

	//private float STimer;

    public int gameScore;
	
	public int pitchCount = 0;
	//public AudioClip[] invadermarchSound;
	public AudioSource invaderAudio;
	public bool invmarchAudio;
	//public int invaderaudiowait = 2;
	//public float marchTimer;
	//public float marchTimer2;
	//public float marchTimer3;
	//public float marchTimer4;
	//public bool soundPlayed = false;

	public AudioClip extralifeSound;
	public AudioClip stageclearSound;
	public AudioClip gameoverSound;

    void Awake()
    {
        enemyObject = new GameObject[55];

        shelterObject = new GameObject[4];
        SetupGame();
    }

    // Use this for initialization
	void Start () {
        MakeBackground();
        scoreScreen.SetActive(true);
        gameGUI.SetActive(false);
    }

	// Update is called once per frame
	void Update () {

        if (!gameStart && !gameRunning)
        {
            if (Input.GetKey(KeyCode.Space))
			{
                gameStart = true;
            }
        }

        if(gameRunning)
        {

            if (playerLives <= 0)
            {
                gameOver = true;
            }

            if (playerDead && !gameOver)
            {
                MakePlayer();
				ShowLives(playerLives);
            }

            if (playerObject != null)
                MovePlayer();

            if (moveDown)
            {
                foreach (GameObject entity in enemyObject)
                {
                    if (entity != null)
                        entity.GetComponent<EnemyCS>().moveDown = true;
                }
                moveDown = false;
            }

			//if (Time.time > marchTimer && !soundPlayed)
			//{
			//PlayInvaderMarch(0);
			//}
			//
			//if (Time.time > marchTimer2 && soundPlayed)
			//{
			//PlayInvaderMarch2(0);
			//}
			//
			//if (Time.time > marchTimer3 && soundPlayed)
			//{
			//PlayInvaderMarch3(0);
			//}
			//
			//if (Time.time > marchTimer4 && soundPlayed)
			//{
			//PlayInvaderMarch4(0);
			//}

			
			if (levelClear)
			{
				//StopCoroutine("Soundspeed");
				//invmarchAudio = true;
				AudioSource.PlayClipAtPoint(stageclearSound, transform.position, 2f);
				levelClear = false;
				CancelInvoke("MakeSaucer");
				//CancelInvoke("PlayInvaders");
				ClearBoard();
				NewLevel();
				AddPlayerLife();
			}

            if (gameOver)
            {
				AudioSource.PlayClipAtPoint(gameoverSound, transform.position, 2f);
				StopCoroutine("Soundspeed");
				//invmarchAudio = true;
				CancelInvoke("MakeSaucer");
				//CancelInvoke("PlayInvaders");
                ClearBoard();
                scoreScreen.SetActive(true);
                gameGUI.SetActive(false);
                gameRunning = false;
            }

			if (currentInvaders <= 54)
			{
				invaderSpeed = 0.1f;
			}

			if (currentInvaders <= 30)
			{
				invaderSpeed = 0.5f;
			}

			if (currentInvaders <= 20)
			{
				invaderSpeed = 1.0f;
			}

			if (currentInvaders <= 10)
			{
				invaderSpeed = 1.5f;
			}

			if (currentInvaders <= 3 )
			{
				invaderSpeed = 2.0f;
			}
		}
	}

	IEnumerator Soundspeed()
	{
		invmarchAudio = false;
		do
		{
			yield return new WaitForSeconds(0.9f * (currentInvaders / 50f));
			PlayInvaders();
		}while(!invmarchAudio);
	}

	void PlayInvaders()
	{
		pitchCount = ++pitchCount % 4;
		invaderAudio.pitch = 1.0f - (pitchCount * 0.1f);
		invaderAudio.Play();
	}

	void CheckSaucer()
	{
		if (currentInvaders <= 54)
		{
			if (!saucerMade)
			{
				InvokeRepeating("MakeSaucer",2f,20f);
			}
		}

		if (currentInvaders <= 3)
		{
			if (!saucerMade)
			{
				saucerMade = true;
			}
		}
	}

    void SetupGame()
    {
        gameStart = false;
        scoreScreen = GameObject.Find("TitlePanel");
        gameGUI = GameObject.Find("GameGUI");
    }

    void LateUpdate()
    {
        if(gameStart)
        {
            GameInit();
        }
    }

    void NewLevel()
    {
        gameOver = false;
        playerDead = true;
        moveDown = false;
		saucerMade = false;
        invaderDirection = 1;
        invaderNr = 0;
        totalMissiles = 0;
        shelterNr = 0;
		levelClear = false;
		ClearBoard();
       	//MakePlayer();
        MakeInvaders();
		CheckSaucer();
		//Soundspeed();

        for (int i = 0; i <= 1; i++)
        {
            MakeShelter(new Vector3(shelterSpread + (2.25f * i), shelterPos, 0f));
            MakeShelter(new Vector3(-shelterSpread - (2.25f * i), shelterPos, 0f));
        }
        
        MakeGround(groundPos);
    }

    void GameInit()
    {
        gameRunning = true;
        gameStart = false;
        scoreScreen.SetActive(false);
        gameGUI.SetActive(true);
        gameScore = 0;
		//maxLives = startLives;
		playerLives = 3;
		invmarchAudio = false;
		ShowLives(playerLives);
		CheckLevel();
        NewLevel();
		StartCoroutine("Soundspeed");
    }

    public void ClearBoard()
    {
		DestroyBullet();
		DestroyMissile();
		DestroyBomb();
		DestroyExplosion();
		DestroyEnemies();
        DestroyBases();
        DestroyGround();
		DestroySaucer();
		DestroyPlayer();
		currentInvaders = -1;
    }

    void MakePlayer()
    {
        playerObject = (GameObject)Instantiate(playerPrefab, playerPos, Quaternion.identity);
        playerObject.name = "Player";
        playerMade = true;
        playerDead = false;
    }

	void AddPlayerLife()
	{
		playerLives++;
		AudioSource.PlayClipAtPoint(extralifeSound, transform.position, 2f);
		if (playerLives >= maxLives) playerLives = maxLives;
		ShowLives(playerLives);
	}

    void MakeInvaders()
    {
        MakeInvaderRow(2, 2.0f); 
        MakeInvaderRow(1, 1.5f);
        MakeInvaderRow(1, 1.0f);
        MakeInvaderRow(0, 0.5f);
        MakeInvaderRow(0, 0.0f);
    }

    void MakeInvaderRow(int invaderType, float invaderLoc)
    {
        float rowStart = rowSize /2 * rowSpacing;
        for (int row = 0; row < rowSize; row++)
        {
            float rowPos = rowStart - (row * rowSpacing);
            MakeInvader(invaderType, new Vector3(rowPos, invaderLoc, enemyPos.z));
        }
    }

    void MakeInvader(int invaderType, Vector3 enemyPos)
    {
        enemyObject[invaderNr] = (GameObject)Instantiate(enemyPrefab[invaderType], enemyPos, Quaternion.identity) as GameObject;
        enemyObject[invaderNr].name = "Invader " + (invaderNr + 1).ToString();
        enemyObject[invaderNr].GetComponent<EnemyCS>().value = enemyValue[invaderType];
        invaderNr++;
		currentInvaders++;
    }

    void MakeGround(Vector3 groundPos)
    {
        groundObject = (GameObject)Instantiate(groundPrefab, groundPos, Quaternion.identity);
        groundObject.name = "Ground";
    }

    void MakeBackground()
    {
        gameScreen = (GameObject)Instantiate(gameScreenPrefab, Vector3.zero, Quaternion.identity);
        gameScreen.name = "Background";
    }
	
	void MakeSaucer()
	{
		saucerType = Random.Range (0, 4);
		if (Random.Range (0.0f, 1.0f) < 0.5f) 
		{
			saucerDirection = -1;
		}
		else
		{
			saucerDirection = 1;
		}
		saucerScore = (saucerType + 1) * 50;
		if (saucerType == 2)
		{
			saucerScore = 300;
		}
		if (saucerType == 3)
		{
			saucerScore = 1000;
		}

		saucerPos = new Vector3 ((saucerBounds * -saucerDirection), missileMax - 0.5f, 0.0f);

		//STimer = Random.Range (0.0f, 1.0f);
		//yield return new WaitForSeconds(10.0f);

		// So as not to keep mass spawn saucer
		if (!saucerMade)
		{
			//Instantiate(saucerPrefab, saucerPos, Quaternion.identity);
			saucerObject = (GameObject)Instantiate(saucerPrefab, saucerPos, Quaternion.identity);
			saucerObject.GetComponent<SaucerCS>().value = saucerScore;
			saucerObject.GetComponent<SaucerCS>().type = saucerType;
			saucerObject.name = "Saucer";

			if (saucerType <= 1)
			{
				saucerObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			}
			if (saucerType == 2)
			{
				saucerObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
			}
			if (saucerType == 3)
			{
				saucerObject.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
			}

			saucerMade = true;
		}
	}


    void MakeShelter(Vector3 shelterPos)
    {
        shelterObject[shelterNr] = (GameObject)Instantiate(shelterPrefab, shelterPos, Quaternion.identity);
        shelterObject[shelterNr].name = "Shelter";
        shelterObject[shelterNr].tag = "Shelter";
        shelterNr++;
    }

    void MovePlayer()
    {
        playerPos = playerObject.transform.position;
        playerDirection = 0;
        if (Input.GetKey(KeyCode.A))
        {
            playerDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerDirection = 1;
        }

        playerPos.x += playerSpeed * playerDirection;
        if (playerPos.x < -playerBounds)
            playerPos.x = -playerBounds;
        else if (playerPos.x > playerBounds)
            playerPos.x = playerBounds;

        playerObject.transform.position = playerPos;
    }

	// Destroy Objects

    void DestroyEnemies()
    {
        foreach (GameObject entity in enemyObject)
        {
            if (entity != null)
                Destroy(entity.gameObject);
        }
    }

    void DestroyBases()
    {
        foreach (GameObject baseObj in shelterObject)
        {
            if (baseObj != null)
                Destroy(baseObj.gameObject);
        }
    }

    void DestroyGround()
    {
        if (groundObject != null)
            Destroy(groundObject.gameObject);
    }

	void DestroySaucer()
	{
		saucerObject = GameObject.FindGameObjectWithTag ("Saucer");
		Destroy (saucerObject);
		//if (saucerObject != null)
		//	Destroy(saucerObject.gameObject);
	}

	void DestroyBullet()
	{
		bulletObject = GameObject.FindGameObjectWithTag ("Bullet");
		Destroy (bulletObject);
	}

	void DestroyMissile()
	{
		missileObject = GameObject.FindGameObjectsWithTag ("Missile");
		foreach (GameObject missile in missileObject) 
		{
			Destroy(missile);
		}
	}

	void DestroyBomb()
	{
		bombObject = GameObject.FindGameObjectWithTag ("Bomb");
		Destroy (bombObject);
	}

	void DestroyPlayer()
	{
		playerObject = GameObject.FindGameObjectWithTag ("Player");
		Destroy (playerObject);
	}

	void DestroyExplosion()
	{
		explosionObject = GameObject.FindGameObjectsWithTag ("Explosion");
		foreach (GameObject explosion in explosionObject) 
		{
			Destroy(explosion);
		}
	}

   public void ShowLives(int playerLife)
    {
		for (int i = 0; i < maxLives - 1; i++)
		{
			gameLife[i].SetActive(false);
		}

		for (int i = 0; i < playerLives - 1; i++)
		{
			gameLife[i].SetActive(true);
		}
    }

	public void CheckLevel()
	{
		currentInvaders--;
		if (currentInvaders <= -1)
		levelClear = true;
	}

	//void PlayInvaderMarch(int type)
	//{
	//GetComponent<AudioSource>().clip = invadermarchSound[type];
	//GetComponent<AudioSource>().Play();
	//marchTimer2 = marchTimer + 1.0f;
	//marchTimer3 = marchTimer + 1.0f;
	//marchTimer4 = marchTimer + 1.0f;
	//soundPlayed = true;
	//}
	//
	//void PlayInvaderMarch2(int type)
	//{
	//GetComponent<AudioSource>().clip = invadermarchSound[type + 1];
	//GetComponent<AudioSource>().Play();
	//marchTimer = Time.time * (invaderNr / 50f);
	//soundPlayed = false;
	//}
	//
	//void PlayInvaderMarch3(int type)
	//{
	//GetComponent<AudioSource>().clip = invadermarchSound[type + 1];
	//GetComponent<AudioSource>().Play();
	//marchTimer2 = Time.time * (invaderNr / 50f);
	//soundPlayed = false;
	//}
	//
	//void PlayInvaderMarch4(int type)
	//{
	//GetComponent<AudioSource>().clip = invadermarchSound[type + 1];
	//GetComponent<AudioSource>().Play();
	//marchTimer3 = Time.time * (invaderNr / 50f);
	//soundPlayed = false;
	//}
}

//
//
// Coded By Mohammad Azhar Bin Tahir // SAE DIT 915
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
	private GameObject HSPanel;
	private GameObject LevelPanel;
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
	public float saucerTimer;

	//public int backgroundDirection;
	//public float backgroundSpeed;
	//private Vector3 backgroundPos;

    public bool playerDead;
    public float playerRespawn;
    public int invaderNr;

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
	public AudioClip[] invadermarchSound;
	public float marchTimer = 0f;
	public float marchTimer2 = 0f;
	public bool soundPlayed = false;
	//public AudioSource invaderAudio;
	//public bool invmarchAudio;
	//public int invaderaudiowait = 2;
	
	public AudioClip extralifeSound;
	public AudioClip stageclearSound;
	public AudioClip gameoverSound;

    public int gameScore;
	public int highScore;
	public int levelText;
	
	public int pitchCount = 0;

	public int maxInvaders;

	public float currentLevel;
	public int gameLevel;

	/*public bool moveStep;
	private float moveTimer;
	public float moveSpeed;*/

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
		LevelPanel.SetActive(true);
        gameGUI.SetActive(false);
    }

	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
        if (!gameStart && !gameRunning)
        {
            if (Input.GetKey(KeyCode.S))
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

			/*if (moveStep)
			{
				foreach (GameObject entity in enemyObject)
				{
					if (entity != null)
						entity.GetComponent<EnemyCS>().moveStep = true;
				}
				moveStep = false;
			}

			if(Time.time > moveTimer && !moveStep)
			{
				moveTimer = Time.time + 0.25f;
				moveStep = true;
			}*/

			if (Time.time > marchTimer && !soundPlayed)
			{
				PlayInvaderMarch(0);
			}

			if (Time.time > marchTimer2 && soundPlayed)
			{
				PlayInvaderMarch2(0);
			}

			if(!saucerMade && Time.time > saucerTimer)
			{
				MakeSaucer();
			}

			
			if (levelClear)
			{
				//StopCoroutine("Soundspeed");
				//invmarchAudio = true;
				AudioSource.PlayClipAtPoint(stageclearSound, transform.position, 2f);
				levelClear = false;
				soundPlayed = true;
				//CancelInvoke("MakeSaucer");
				//CancelInvoke("PlayInvaders");
				ClearBoard();
				NewLevel();
				AddPlayerLife();
			}

            if (gameOver)
            {
				AudioSource.PlayClipAtPoint(gameoverSound, transform.position, 2f);
				//StopCoroutine("Soundspeed");
				//invmarchAudio = true;
				//CancelInvoke("MakeSaucer");
				//CancelInvoke("PlayInvaders");
                ClearBoard();
                scoreScreen.SetActive(true);
				LevelPanel.SetActive(true);
                gameGUI.SetActive(false);
                gameRunning = false;
				if(gameScore > highScore)
					highScore = gameScore;
            }

			/*if (currentInvaders <= 55)
			{
				invaderSpeed = 1.1f;
			}

			if (currentInvaders <= 40)
			{
				invaderSpeed = 1.15f;
			}

			if (currentInvaders <= 30)
			{
				invaderSpeed = 1.2f;
			}

			if (currentInvaders <= 20)
			{
				invaderSpeed = 1.25f;
			}

			if (currentInvaders <= 10)
			{
				invaderSpeed = 1.3f;
			}

			if (currentInvaders <= 3 )
			{
				invaderSpeed = 1.35f;
			}*/
		}
	}

	// IEnumerator Soundspeed()
	//{
	//	invmarchAudio = false;
	//	do
	//	{
	//		yield return new WaitForSeconds(0.9f * (currentInvaders / 50f));
	//		PlayInvaders();
	//	}while(!invmarchAudio);
	//}

	//void PlayInvaders()
	//{
	//	pitchCount = ++pitchCount % 4;
	//	invaderAudio.pitch = 1.0f - (pitchCount * 0.1f);
	//	invaderAudio.Play();
	//}

	//void CheckSaucer()
	//{
	//	if (currentInvaders <= 54)
	//	{
	//		if (!saucerMade)
	//		{
	//			InvokeRepeating("MakeSaucer",2f,20f);
	//		}
	//	}
	//
	//	if (currentInvaders <= 3)
	//	{
	//		if (!saucerMade)
	//		{
	//			saucerMade = true;
	//		}
	//	}
	//}

    void SetupGame()
    {
        gameStart = false;
		highScore = 0;
        scoreScreen = GameObject.Find("TitlePanel");
		LevelPanel = GameObject.Find("LevelPanel");
		HSPanel = GameObject.Find("HSPanel");
        gameGUI = GameObject.Find("GameGUI");
		saucerTimer = Time.time + Random.Range(5.0f, 10.0f);
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
        //moveDown = false;
		/*moveStep = false;*/
		saucerMade = false;
        invaderDirection = 1;
        invaderNr = 0;
		currentInvaders = 0;
        totalMissiles = 0;
		maxMissiles = (gameLevel + 1);
        shelterNr = 0;
		levelClear = false;
		/*moveTimer = 0.0f;*/
		ClearBoard();
       	//MakePlayer();
        MakeInvaders();
		//OnGUI();
		levelText = gameLevel;

		maxInvaders = invaderNr;
		//CheckSaucer();
		//Soundspeed();

		currentLevel = (gameLevel + 1) / 10f;

		if(gameScore > highScore)
			highScore = gameScore;

		saucerTimer = Time.time + Random.Range(5.0f, 10f);

        for (int i = 0; i <= 1; i++)
        {
            MakeShelter(new Vector3(shelterSpread + (2.25f * i), shelterPos, 0f));
            MakeShelter(new Vector3(-shelterSpread - (2.25f * i), shelterPos, 0f));
        }
        
        MakeGround(groundPos);
    }

	/*void OnGUI (){
		GUI.Label( new Rect(450,100,Screen.width,Screen.height),"highScore");
	}*/

    void GameInit()
    {
        gameRunning = true;
        gameStart = false;
        scoreScreen.SetActive(false);
		LevelPanel.SetActive(true);
        gameGUI.SetActive(true);
        gameScore = 0;
		//maxLives = startLives;
		playerLives = 3;
		//invmarchAudio = false;
		ShowLives(playerLives);
		CheckLevel();
        NewLevel();
		gameLevel = 0;
		levelText = 0;
		//StartCoroutine("Soundspeed");
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
		currentInvaders = 0;
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
		gameLevel++;
		levelText++;
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
			saucerScore = 150;
		}
		if (saucerType == 3)
		{
			saucerScore = 300;
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

			saucerTimer = Time.time + Random.Range(5.0f, 10f);
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
		if (currentInvaders <= 0)
		levelClear = true;
	}

	void PlayInvaderMarch(int type)
	{
		pitchCount = ++pitchCount % 4;
	//	invaderAudio.pitch = 1.0f - (pitchCount * 0.1f);
		GetComponent<AudioSource>().clip = invadermarchSound[type];
		GetComponent<AudioSource>().pitch = 1.0f - (pitchCount * 0.1f);
		GetComponent<AudioSource>().Play();
		marchTimer2 = marchTimer + 0.1f;
		soundPlayed = true;
	}

	void PlayInvaderMarch2(int type)
	{
	//GetComponent<AudioSource>().clip = invadermarchSound[type + 1];
	//GetComponent<AudioSource>().Play();
		if (currentInvaders > 0)
			marchTimer = Time.time + (currentInvaders / 70f);
		else
			marchTimer = Time.time + 0.01f;

		/*if (currentInvaders <= 3 )
		{
			invaderSpeed = gameLevel;
		}*/

	soundPlayed = false;
	}
}

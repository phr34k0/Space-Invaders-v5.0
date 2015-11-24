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

	private float STimer;

    public int gameScore;
	
	public int pitchCount = 0;
	public AudioSource invaderAudio;
	//public int invaderaudiowait = 2;

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
            //if (Input.GetKey(KeyCode.Space))
			//{
				InvokeRepeating("PlayInvaders",0.001f,0.8f);
                gameStart = true;
            //}
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

			if (!saucerMade)
			{
				StartCoroutine( MakeSaucer() );
			}

            if (gameOver)
            {
				CancelInvoke();
                ClearBoard();
                scoreScreen.SetActive(true);
                gameGUI.SetActive(false);
                gameRunning = false;
            }
        }
	}

	void PlayInvaders()
	{
		pitchCount = ++pitchCount % 4;
		invaderAudio.pitch = 1.0f - (pitchCount * 0.1f);
		GetComponent<AudioSource>().pitch = invaderAudio.pitch;
		GetComponent<AudioSource>().Play();
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
        MakePlayer();
        MakeInvaders();

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
		maxLives = startLives;
        playerLives = maxLives;
		ShowLives(playerLives);
		CheckLevel();
        NewLevel();
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
    }

    void MakePlayer()
    {
        playerObject = (GameObject)Instantiate(playerPrefab, playerPos, Quaternion.identity);
        playerObject.name = "Player";
        playerMade = true;
        playerDead = false;
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

	IEnumerator MakeSaucer()
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
		if (saucerType == 3)
		{
			saucerScore = 300;
		}

		saucerPos = new Vector3 ((saucerBounds * -saucerDirection), missileMax - 0.5f, 0.0f);

		//STimer = Random.Range(10.0f, 30.0f);
		yield return new WaitForSeconds(10.0f);

		// So as not to keep mass spawn saucer
		if (!saucerMade)
		{
			//Instantiate(saucerPrefab, saucerPos, Quaternion.identity);
			saucerObject = (GameObject)Instantiate(saucerPrefab, saucerPos, Quaternion.identity);
			saucerObject.GetComponent<SaucerCS>().value = saucerScore;
			saucerObject.name = "Saucer";
			saucerMade = true;
		}

		//saucerObject = (GameObject)Instantiate(saucerPrefab, saucerPos, Quaternion.identity);
		//saucerObject.GetComponent<SaucerCS>().value = saucerScore;
		//saucerObject.name = "Saucer";
		//saucerMade = true;
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
	}
}

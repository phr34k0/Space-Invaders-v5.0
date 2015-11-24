using UnityEngine;
using System.Collections;

public class ShooterCS : MonoBehaviour {

    public GameObject bulletPrefab;
    private GameObject bulletObject;

    public bool bulletMade;
    private GameManagerCS gm;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }

    // Use this for initialization
	void Start () {
        bulletMade = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.Space) && !bulletMade)
        {
            MakeBullet(this.transform.position);
        }
	}

    void MakeBullet(Vector3 playerPos)
    {
        bulletObject = (GameObject)Instantiate(bulletPrefab,
                                               new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z), 
                                               Quaternion.identity);
        bulletMade = true;
    }
}

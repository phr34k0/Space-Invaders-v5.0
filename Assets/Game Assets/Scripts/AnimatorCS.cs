using UnityEngine;
using System.Collections;

public class AnimatorCS : MonoBehaviour {
    
    public Sprite[] animFrames;

    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;

    public int animationIdx = 0;
	
    // Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        spriteRenderer.sprite = animFrames[0]; // initial sprite
	}
	
	// Update is called once per frame
	void Update () {
        AnimationSequence();
	}

    void AnimationSequence()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index = index % animFrames.Length;
        spriteRenderer.sprite = animFrames[index];
    }
}

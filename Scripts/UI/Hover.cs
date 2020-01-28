using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {

    private SpriteRenderer spriteRenderer, rangedSpriteRenderer;

    public bool IsVisible { get; private set; }

	// Use this for initialization
	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangedSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();    // gets the sprite renderer from the child object (range)
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
	}

    private void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);    // so that the icon is in front of the game
    }

    public void Activate(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;

        rangedSpriteRenderer.enabled = true;
        IsVisible = true;
    }

    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        rangedSpriteRenderer.enabled = false;

        Game_Manager.Instance.ClickedBtn = null;

        IsVisible = false;
    }
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneObject : MonoBehaviour {

    public static float maxDistToPlayer;
    public string text;
    public List<string> unlocks;
    public int ID;
	public bool increaseRadius;

    public bool startsActive;
    public bool isEnding;
    public bool isLate;
    public bool isGrandma;

    void Awake()
    {
        var width = transform.localScale.x / GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        maxDistToPlayer = width / 2 + 1;
    }

    // Use this for initialization
    void Start()
    {
        if (!StateManager.cleanLevels.Contains(Application.loadedLevelName))
        {
            if (ID == 1)
            {
                if (!startsActive)
                {
                    //gameObject.GetComponent<Renderer>().enabled = false;
                    enabled = false;
                }
            }
            else
                enabled = false;
        }
        
    }

    public void SetData(string imagePath, Vector2 position, string clickText)
    {
        /*spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        Texture2D tex = Resources.Load<Texture2D>(imagePath);
        spriteRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        transform.position.Set(position.x, position.y, 0);
        text = clickText;*/
    }

    public void Unlock()
    {
        if (!enabled)
        {
            var sceneObjects = GetComponents<SceneObject>();
            bool blockUnlock = false;
            foreach (var so in sceneObjects)
            {
                if (so.enabled && so.ID > ID)
                    blockUnlock = true;

            }
            if (!blockUnlock)
            {
                foreach (var so in sceneObjects)
                {
                    if (so.GetComponent<Renderer>().enabled && so.ID < ID)
                        so.enabled = false;
                }
                enabled = true;
                if (!gameObject.GetComponent<Renderer>().enabled)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }
	
    public void OnMouseDown()
    {
        if (!enabled || StateManager.textOnScreen)
            return;
		//GenerateTextBox ("");
        var player = GameObject.Find("Character");
        if (player != null)
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) > maxDistToPlayer)
            {
                //too far
            }
            else
            {
                if (isGrandma)
                {
                    StateManager.hasCalledGrandma = true;
                }
                if (isLate)
                {
                    StateManager.Late();
                }
				if(isEnding){
					StateManager.hasBedEnd = true;
					SceneManager.GenerateTextBox("The bed looks so comfy. A quick nap would probably be fine…");
                    return;
				}
				if(increaseRadius) {
					StateManager.radius += 5;
					increaseRadius = false;
				}
                SceneManager.GenerateTextBox(text);
                player.GetComponent<Character>().hasBrokenOut = true;
                var controller = player.GetComponent<Character>();
                if (controller != null)
                {
                    controller.TurnAround(true);
                    Debug.Log(text);
                }
                foreach (string unlock in unlocks)
                {
                    var tryUnlock = SceneManager.UnlockSceneObject(unlock);
                    //scene object may be in another scene, store for later checking
                    if (!tryUnlock)
                    {
                        StateManager.queuedUnlocks.Add(unlock);
                    }
                }
            }
        }
    }

	

	// Update is called once per frame
	void Update () {
	
	}
}

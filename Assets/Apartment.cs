﻿using UnityEngine;
using System.Collections;

public class Apartment : MonoBehaviour {

    public bool playerHasMoved = false;
    private Character player;
    private float destination;
    
    // Use this for initialization
	void Start () {
        Timer.Subscribe(LeaveOnTime, 7, 30);
        Timer.Subscribe(LeaveLastMinute, 8, 0);
        var go = GameObject.Find("Character");
        player = go.GetComponent<Character>();
        destination = GameObject.Find("Chair").transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (!player.hasBrokenOut || destination == 10)
        {
            var xpos = player.transform.position.x;
            var move = 5 * Time.deltaTime;
            //player is within move distance of destination
            if (Mathf.Abs(xpos - destination) < move)
            {
                var pos = player.transform.position;
                pos.x = destination;
                player.transform.position = pos;
            }
            //player is to the left of destination
            else if (xpos < destination - move)
            {
                player.MoveRight();
            }
            //player is to the right of destination
            else
            {
                player.MoveLeft();
            }
        }
        if (player.transform.position.x == 10)
        {
            StateManager.SaveState();
            Application.LoadLevel("Second");
        }
    }

    public void LeaveOnTime(int hour, int minute)
    {
        if (!player.hasBrokenOut)
        {
            destination = 10;
        }
    }

    public void LeaveLastMinute(int hour, int minute)
    {
        destination = 10;
    }
}

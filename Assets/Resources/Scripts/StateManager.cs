﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

    public static Dictionary<string, Dictionary<string, bool>> activeList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Restores state stored in list to all SceneObjects present in scene
    /// </summary>
    public static void RestoreState()
    {
        foreach (string key in activeList.Keys)
        {
            var go = GameObject.Find(key);
            if (go != null)
            {
                var dict = activeList[key];
                var sceneObjects = go.GetComponents<SceneObject>();
                foreach (var so in sceneObjects)
                {
                    if (dict.ContainsKey(so.ID.ToString()))
                    {
                        so.enabled = dict[so.ID.ToString()];
                    }
                }
                if (dict.ContainsKey("Renderer"))
                {
                    go.GetComponent<Renderer>().enabled = dict["Renderer"];
                }
                if (dict.ContainsKey("Collider"))
                {
                    go.GetComponent<Collider2D>().enabled = dict["Collider"];
                }
            }
        }
    }

    /// <summary>
    /// Saves the state of all SceneObjects currently in the scene to static list
    /// </summary>
    public static void SaveState()
    {
        var interactiveObjects = FindObjectsOfType<SceneObject>();
        activeList = new Dictionary<string, Dictionary<string, bool>>();
        foreach (var so in interactiveObjects)
        {
            //if gameobject is already in dictionary, add this component's data
            if (activeList.ContainsKey(so.gameObject.name))
            {
                var data = activeList[so.gameObject.name];
                data.Add(so.ID.ToString(), so.enabled);
            }
            //if gameobject is not in dictionary, add render and collision data, then this component's data
            else
            {
                var data = new Dictionary<string, bool>();
                var renderer = so.GetComponent<Renderer>();
                if (renderer != null)
                    data.Add("Renderer", renderer.enabled);
                var collider = so.GetComponent<Collider2D>();
                if (collider != null)
                    data.Add("Collider", collider.enabled);
                data.Add(so.ID.ToString(), so.enabled);
                activeList.Add(so.gameObject.name, data);
            }
        }
    }
}
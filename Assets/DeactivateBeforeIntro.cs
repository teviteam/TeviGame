﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateBeforeIntro : MonoBehaviour {

    public float num = 37f;

    private GameObject connectionObj;
    private bool myNewPlayer;

    // Use this for initialization
    void Start () {

        connectionObj = GameObject.Find("ConnectionInfos");
        if (connectionObj)
        {
            myNewPlayer = connectionObj.GetComponent<Connecting>().newPlayer;
        }

        if (myNewPlayer)
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (myNewPlayer)
        {
            if (num > 0)
            {
                num = num - Time.deltaTime;
            }
            else
            {
                gameObject.GetComponent<Canvas>().enabled = true;
            }
        }

	}
}
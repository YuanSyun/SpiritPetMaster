﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFly : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        //water
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<Rigidbody2D>().angularVelocity = 0;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 2650);
        }
    }
}

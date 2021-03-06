﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer sr;

    public float moveSpeed = 5;
    public float stepTimer /*= -1f*/;

    public bool walking;
    public bool candlePuzClear;
    public bool clearSoundPlayed;

    public KeyCode rightKey = KeyCode.D;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    public AudioSource footStepSource;
    public AudioClip stepOnWood;
    public AudioSource screamSource;

    public List<LightingScript> candles;
    public int numCandlesLit = 0;

    public GameObject blueFlames;

    public Animator anim;
    
	// Use this for initialization
	void Start () {

        sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
        stepTimer = 0;

        candlePuzClear = false;
        blueFlames.SetActive(false);
        clearSoundPlayed = false;

        anim = GetComponent<Animator>();

        walking = false;
        

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        Debug.Log(stepTimer);

        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);

        if (Input.GetKey(rightKey))
        {
            walking = true;
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            walkingSound();
            // Debug.Log(walking);

            sr.flipX = false;
            sideRun();

            //anim.SetBool("isRunning", true);

            anim.SetBool("isWalking", false);

        }
        else 
        {
            walking = false;

            //anim.SetBool("isWalking", false);
        }

        if (Input.GetKey(leftKey))
        {
            walking = true;
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            walkingSound();

            sr.flipX = true;
            sideRun();

            anim.SetBool("isWalking", false);

        }
        else { walking = false;
            //anim.SetBool("isWalking", false);
        }


        if (Input.GetKey(upKey))
        {
            walking = true;
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            walkingSound();

            sr.flipX = false;
            walkingAnim();

        }
        else { walking = false;
           // anim.SetBool("isWalking", false);
        }


        if (Input.GetKey(downKey)) {

            walking = true;
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            walkingSound();
            walkingAnim();
            sr.flipX = false;

        }
        else { walking = false;

           //anim.SetBool("isWalking", false);

        }


        for (int i = 0; i < candles.Count; i++)
        {
            numCandlesLit = 0;

            if (candles[0].candleLit == true)
            {
                numCandlesLit++;
            }

            if (candles[1].candleLit == true)
            {
                numCandlesLit++;
            }

            if (candles[2].candleLit == true)
            {
                numCandlesLit++;
            }

            if (candles[3].candleLit == true)
            {
                numCandlesLit++;
            }

            if (candles[4].candleLit == true)
            {
                numCandlesLit++;
            }

            if (candles[5].candleLit == true)
            {
                numCandlesLit++;
            }

        }

        if (numCandlesLit == 6) {
            candlePuzClear = true;
        }

        if(candlePuzClear == true)
        {
            blueFlames.SetActive(true);

            if (clearSoundPlayed == false)
            {
                screamSound();
                clearSoundPlayed = true;
            }

        }

    }

    public void walkingSound()
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            footStepSource.Play();
            footStepSource.pitch = Random.Range(1, 5);
            stepTimer = 0.2f;
        }

    }

    public void screamSound()
    {
        if (!screamSource.isPlaying)
        {
            screamSource.Play();
        }

    }

    public void walkingAnim() {

        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);

    }

    public void sideRun() {
        Debug.Log("running");
        anim.SetBool("isRunning", true);
        anim.SetBool("isWalking", false);

    }

}

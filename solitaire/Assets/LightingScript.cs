using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingScript : MonoBehaviour {

    public float litTimer;
    public float numOfLitCandles = 0f;

    public GameObject flamePrefab;

    public bool candleLit = false;

    public bool changeCounter = false;

    public AudioSource fireSource;

    // Use this for initialization
    void Start() {

        litTimer = 0f;

        flamePrefab.SetActive(false);

        changeCounter = true;
        //numOfLitCandles = 0f;
    }

    // Update is called once per frame
    void Update() {

        if (litTimer > 0 && litTimer < 5)
        {
            this.flamePrefab.SetActive(true);
        }
        else {
            this.flamePrefab.SetActive(false);
        }

        if (candleLit == true) {

            litTimer -= Time.deltaTime;

            if (changeCounter == true) {

                numOfLitCandles++;
                changeCounter = false;

            }

        }

        if (candleLit == false) {

        }

        if (litTimer <= 0) {

            candleLit = false;
            changeCounter = true;

        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            litTimer = 5f;
            candleLit = true;
            litSound();

        }
    }

    public void litSound(){
        fireSource.Play();
        fireSource.pitch = Random.Range(1,4);

    }

}

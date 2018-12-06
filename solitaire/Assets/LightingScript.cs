using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingScript : MonoBehaviour {

    public float litTimer;
    public GameObject flamePrefab;

	// Use this for initialization
	void Start () {
        litTimer = 0f;
        flamePrefab.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

       // litTimer -= Time.deltaTime;

        if (litTimer > 0 && litTimer < 5)
        {
            this.flamePrefab.SetActive(true);          

        }
        else {
            this.flamePrefab.SetActive(false);
        }


	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            litTimer = 5f;
            litTimer -= Time.deltaTime;

        }
    }

}

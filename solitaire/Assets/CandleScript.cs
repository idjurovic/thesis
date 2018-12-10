using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandleScript : MonoBehaviour {

    public GameObject lightButton;

    public AudioSource screamSource;
    public AudioClip screamClip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //lightButton.SetActive(true);
            //screamSound();
            if (Input.GetKey("space")) {
                SceneManager.LoadScene(1);
            }


        } else { lightButton.SetActive(false); }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        lightButton.SetActive(false);


    }

    public void screamSound()
    {
        if (!screamSource.isPlaying)
        {
            screamSource.Play();
        }        

    }

}

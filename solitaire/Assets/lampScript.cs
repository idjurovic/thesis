using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampScript : MonoBehaviour {

    public AudioSource switchSource;
    public AudioClip switchClip;

    public AudioSource demonSource;
    public AudioClip demonClip;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {


    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        Debug.Log("lampColl");

     if (coll.gameObject.tag == "Player")

            if (Input.GetKeyDown(KeyCode.Space))
            {
                switchSound();
                demonGrumbling();
            }

    }

    public void switchSound() {

        if (!switchSource.isPlaying)
        {
            switchSource.Play();
        }
    }

    public void demonGrumbling() {
        if (!demonSource.isPlaying) {
            demonSource.Play();
        }

    }

}

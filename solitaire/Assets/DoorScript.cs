using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {

    public Player playerScript;
    public bool colliding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (playerScript.candlePuzClear == true) {

            if (colliding == true)
            {
                if (Input.GetKey(KeyCode.Space)) {
                    SceneManager.LoadScene(2);

                }

            }
        }
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            colliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            colliding = false;
        }
    }

}

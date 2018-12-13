using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    private IEnumerator coroutine;

    // Use this for initialization
    void Start() {
        coroutine = Wait(3f);
        StartCoroutine(coroutine);
    }

    private IEnumerator Wait(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("SampleScene");
    }
}

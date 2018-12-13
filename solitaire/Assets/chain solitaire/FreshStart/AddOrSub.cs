using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddOrSub : MonoBehaviour {

    public CardManager theDeck;
    public Text predictText;
    public bool showPrediction = true;
    public int counter = 0;
    public bool alreadyValued = false;  //did you already add or subtract this card? no bm pls
    public bool predictUsed = false;
    public bool skipUsed = false;
    public GameObject predictButton;
    public GameObject skipButton;
    //public bool showRoundNumber = false;
    //public int extraRoundNumber = 0;
    public bool trapped = false;
    public int goalRange = 0;
    public int endingNumber;

    public AudioSource predictSource;


    private void Start() {
        this.GetComponent<Text>().text = "Round " + (theDeck.round + 1) + " of " + (theDeck.totalRounds - 1) + "\n" + counter;
        predictText.text = "";
        predictButton.SetActive(true);
        skipButton.SetActive(false);
        endingNumber = 0;

        predictSource = GameObject.Find("predictSource").GetComponent<AudioSource>();
        
    }

    private void Update() {
        //if (counter == theDeck.goal) {
        //    skipButton.SetActive(true);
        //    trapped = true;
        //    theDeck.goalText.text = "Goal: " + (theDeck.goal - goalRange) + " - " + (theDeck.goal + goalRange);
        //}
    }

    public void Add () {
        if (!alreadyValued) {
            Debug.Log("" + theDeck.myCard.rank + " of " + theDeck.myCard.suit);

            counter += theDeck.myCard.rank;
            if (theDeck.myCard.suit.ToString() == "diamonds") {
                //showRoundNumber = true;
                //extraRoundNumber++;
                //theDeck.totalRounds++;
            }
            //if (showRoundNumber) {
            //    //this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + " (+" + extraRoundNumber + " extra!)" + "\n" + counter;
            //}
            //else {
            this.GetComponent<Text>().text = "Round " + (theDeck.round + 1) + " of " + (theDeck.totalRounds - 1) + "\n" + counter;
            //}
            //this.GetComponent<Text>().text = "Round " + theDeck.totalRounds + "\n" + counter;
            alreadyValued = true;
            Draw();
        }
    }
	
	public void Subtract () {
        if (!alreadyValued) {
            counter -= theDeck.myCard.rank;
            if (theDeck.myCard.suit.ToString() == "diamonds") {
                //showRoundNumber = true;
                //extraRoundNumber++;
                //theDeck.totalRounds++;
            }
            //if (showRoundNumber) {
            //    //this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + " (+" + extraRoundNumber + " extra!)" + "\n" + counter;
            //}
            //else {
            this.GetComponent<Text>().text = "Round " + (theDeck.round + 1) + " of " + (theDeck.totalRounds - 1) + "\n" + counter;
            //}
            //this.GetComponent<Text>().text = "Round " + theDeck.totalRounds + "\n" + counter;
            alreadyValued = true;
            Draw();
        }
    }

    public void AddSecond () {
        if (!alreadyValued) {
            counter += theDeck.secondCard.rank;
            if (theDeck.myCard.suit.ToString() == "diamonds") {
                //showRoundNumber = true;
                //extraRoundNumber++;
                //theDeck.totalRounds++;
            }
            //if (showRoundNumber) {
            //    //this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + " (+" + extraRoundNumber + " extra!)" + "\n" + counter;
            //}
            //else {
            this.GetComponent<Text>().text = "Round " + (theDeck.round + 1) + " of " + (theDeck.totalRounds - 1) + "\n" + counter;
            //}
            //this.GetComponent<Text>().text = "Round " + theDeck.totalRounds + "\n" + counter;
            alreadyValued = true;
            Draw();
        }
    }

    public void SubtractSecond() {
        if (!alreadyValued) {
            counter -= theDeck.secondCard.rank;
            if (theDeck.myCard.suit.ToString() == "diamonds") {
                //showRoundNumber = true;
                //extraRoundNumber++;
                //theDeck.totalRounds++;
            }
            //if (showRoundNumber) {
            //    //this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + " (+" + extraRoundNumber + " extra!)" + "\n" + counter;
            //}
            //else {
            this.GetComponent<Text>().text = "Round " + (theDeck.round + 1) + " of " + (theDeck.totalRounds - 1) + "\n" + counter;
            //}
            //this.GetComponent<Text>().text = "Round " + theDeck.totalRounds + "\n" + counter;
            alreadyValued = true;
            Draw();
        }
    }

    public void Draw () {
        theDeck.Draw();
        if (predictUsed) {
            showPrediction = false;
            predictText.text = "";
            predictButton.SetActive(false);
        }
        if (skipUsed) {
            //skipButton.SetActive(false);
        }
        if (counter == theDeck.goal) {
            skipButton.SetActive(true);
            trapped = true;
            Trapped();
        }
    }

    public void Replay() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (endingNumber == 1) {
            SceneManager.LoadScene("bestEnd");
        }
        else if (endingNumber == 2) {
            SceneManager.LoadScene("goodEnd");
        }
        else if (endingNumber == 3) {
            SceneManager.LoadScene("badEnd");
        }
        else {
            SceneManager.LoadScene("goodEnd");
        }

        //SceneManager.LoadScene(0);
    }

    public void Predict() {
        if (!predictUsed && showPrediction) {
            predictText.text = "I'm sensing the next cards will be the\n" + theDeck.deck[0].rank + " of " + theDeck.deck[0].suit + " and\n"
                + "the " + theDeck.deck[1].rank + " of " + theDeck.deck[1].suit + ".";
            predictionSound();
        }
        predictUsed = true;
    }

    public void Skip() {
        //skip 1 turn code
        //if (!skipUsed) {
        //    skipUsed = true;
        //    alreadyValued = true;
        //    theDeck.round--;
        //    Draw();
        //}
    }

    public void Trapped() {
        if (trapped) {
            goalRange++;
            theDeck.goalText.text = "Goal: " + (theDeck.goal - goalRange) + " - " + (theDeck.goal + goalRange);
            //trapped = false;
        }
    }

    public void predictionSound() {

        predictSource.Play();

    }

}

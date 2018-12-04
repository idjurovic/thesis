using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddOrSub : MonoBehaviour {

    public CardManager theDeck;
    public int counter = 0;
    public bool alreadyValued = false;  //did you already add or subtract this card? no bm pls
    //public bool showRoundNumber = false;
    //public int extraRoundNumber = 0;

    private void Start() {
        this.GetComponent<Text>().text = this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + "\n" + counter;
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
                this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + "\n" + counter;
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
                this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + "\n" + counter;
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
                this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + "\n" + counter;
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
                this.GetComponent<Text>().text = "Round " + theDeck.round + " of " + theDeck.totalRounds + "\n" + counter;
            //}
            //this.GetComponent<Text>().text = "Round " + theDeck.totalRounds + "\n" + counter;
            alreadyValued = true;
            Draw();
        }
    }

    public void Draw () {
        theDeck.Draw();
    }

    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

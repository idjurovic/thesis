using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddOrSub : MonoBehaviour {

    public CardManager theDeck;
    public int counter = 0;
    public bool alreadyValued = false;  //did you already add or subtract this card? no bm pls

	public void Add () {

        //Debug.Log(Deck.current_card_rank);
        //Debug.Log(counter);

        //counter += Deck.current_card_rank + 1;
        //this.GetComponent<Text>().text = "" + counter;
        //Deck.RunNewCard();

        //Debug.Log(theDeck.myCard.rank);

        if (!alreadyValued) {
            counter += theDeck.myCard.rank;
            this.GetComponent<Text>().text = "" + counter;
            alreadyValued = true;
        }
    }
	
	public void Subtract () {

        //Debug.Log(Deck.current_card_rank);
        //Debug.Log(counter);

        //counter -= Deck.current_card_rank + 1;
        //this.GetComponent<Text>().text = "" + counter;
        //Deck.RunNewCard();

        //Debug.Log(theDeck.myCard.rank);

        if (!alreadyValued) {
            counter -= theDeck.myCard.rank;
            this.GetComponent<Text>().text = "" + counter;
            alreadyValued = true;
        }
    }

    public void AddSecond () {
        if (!alreadyValued) {
            counter += theDeck.secondCard.rank;
            this.GetComponent<Text>().text = "" + counter;
            alreadyValued = true;
        }
    }

    public void SubtractSecond() {
        if (!alreadyValued) {
            counter -= theDeck.secondCard.rank;
            this.GetComponent<Text>().text = "" + counter;
            alreadyValued = true;
        }
    }

    public void Draw () {
        theDeck.Draw();
    }
}

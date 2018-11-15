using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {

    public List<Card> deck;
    public Sprite[] cardSprites;
    public GameObject drawnCard;
    public GameObject drawnSecondCard;
    public Card myCard;
    public Card secondCard;
    public int round;   //how many rounds of cards
    public AddOrSub gameData;
    public Text goalText;   //display the goal
    public int goal;    //number to get
	
	void Start () {
        for (int i = 0; i < 4; i++) {
            Card.Suit newCardSuit = (Card.Suit)i;
            for (int j = 0; j < 13; j++) {
                deck.Add(new Card(newCardSuit, j));
            }
        }

        goal = (int)Random.Range(3, 24);
        goalText.text = "Goal: " + goal;

        ShuffleDeck();
        
        //first card in hand
        myCard = deck[0];
        deck.RemoveAt(0);

        drawnCard = GameObject.Find("Card_Sprite");
        drawnCard.GetComponent<SpriteRenderer>().sprite = GetCardSprite(myCard.suit, myCard.rank);

        //second card in hand
        secondCard = deck[0];
        deck.RemoveAt(0);

        drawnSecondCard = GameObject.Find("Card_Second");
        drawnSecondCard.GetComponent<SpriteRenderer>().sprite = GetCardSprite(secondCard.suit, secondCard.rank);
    }

	Sprite GetCardSprite(Card.Suit suit, int rank) {
        return cardSprites[((int)suit * 13) + rank -1]; //this sometimes goes out of range...
    }
	
	void Update () {
		
	}

    public void Draw() {
        if (round < 5 && gameData.alreadyValued) {
            //Debug.Log("mouse down");
            myCard = deck[0];
            deck.RemoveAt(0);
            drawnCard.GetComponent<SpriteRenderer>().sprite = GetCardSprite(myCard.suit, myCard.rank);

            secondCard = deck[0];
            deck.RemoveAt(0);
            drawnSecondCard.GetComponent<SpriteRenderer>().sprite = GetCardSprite(secondCard.suit, secondCard.rank);

            gameData.alreadyValued = false;
            round++;
        }
        else if (round < 5 && !gameData.alreadyValued) {
            //just don't end the game ok
        }
        else {
            Debug.Log("game over");
            if (gameData.counter == goal) {
                gameData.GetComponent<Text>().text = "Game Over\nBest End!!";
            }
            else if (gameData.counter > goal - 3 && gameData.counter < goal + 3) {
                gameData.GetComponent<Text>().text = "Game Over\nGood End";
            }
            else if ((gameData.counter < goal - 3 && gameData.counter > goal - 5) || (gameData.counter > goal + 3 && gameData.counter < goal + 5)) {
                gameData.GetComponent<Text>().text = "Game Over\nLukewarm End";
            }
            else {
                gameData.GetComponent<Text>().text = "Game Over\nDid you even try?";
            }
        }
    }

    void ShuffleDeck() {
        for (int i = 0; i < deck.Count; i++) {
            int j = Random.Range(0, deck.Count);
            Card tmpCard = new Card(deck[j].suit, deck[j].rank);
            deck[j] = deck[i];
            deck[i] = tmpCard;
        }
    }
}
[System.Serializable]
public class Card {

    public enum Suit {
        diamonds,
        hearts,
        spades,
        clubs
    }
    public Suit suit;
    public int rank;

    public Card (Suit _suit, int _rank) {
        suit = _suit;
        rank = _rank;
    }



}
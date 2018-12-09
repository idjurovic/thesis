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
    public int round = 1;   //how many rounds of cards
    public AddOrSub gameData;
    public Text goalText;   //display the goal
    public int goal;    //number to get
    public int totalRounds = 5;
	
	void Start () {
        //for (int i = 0; i < 4; i++) {
        //    Card.Suit newCardSuit = (Card.Suit)i;
        //    for (int j = 1; j < 14; j++) {
        //        deck.Add(new Card(newCardSuit, j));
        //    }
        //}

        //only 10 cards of 1 suit
        for (int i = 0; i < 1; i++) {
            Card.Suit newCardSuit = (Card.Suit)i;
            for (int j = 1; j < 11; j++) {
                deck.Add(new Card(newCardSuit, j));
            }
        }

        //goal = (int)Random.Range(3, 21);
        goal = (int)Random.Range(3, 16);
        goalText.text = "Goal: " + goal;
        round = 1;
        totalRounds = 6;

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
        //Pressed();
	}

    public void Draw() {
        round++;
        if (round < totalRounds && gameData.alreadyValued) {
            //Debug.Log("mouse down");
            myCard = deck[0];
            deck.RemoveAt(0);
            drawnCard.GetComponent<SpriteRenderer>().sprite = GetCardSprite(myCard.suit, myCard.rank);

            secondCard = deck[0];
            deck.RemoveAt(0);
            drawnSecondCard.GetComponent<SpriteRenderer>().sprite = GetCardSprite(secondCard.suit, secondCard.rank);

            gameData.alreadyValued = false;
        }
        else if (round < totalRounds && !gameData.alreadyValued) {
            //just don't end the game ok
        }
        else {
            Debug.Log("game over");
            if (gameData.counter == goal || gameData.trapped && (gameData.counter >= goal - 1 && gameData.counter <= goal + 1)) {
                if (gameData.counter == goal) {
                    gameData.GetComponent<Text>().text = gameData.counter + "\nBest End!!";
                }
                else {
                    gameData.GetComponent<Text>().text = gameData.counter + "\nSecret End (TRAPPED)";
                }
            }
            else if (gameData.counter > goal - 3 && gameData.counter < goal + 3) {
                gameData.GetComponent<Text>().text = gameData.counter + "\nGood End";
            }
            else if ((gameData.counter < goal - 3 && gameData.counter > goal - 5) || (gameData.counter > goal + 3 && gameData.counter < goal + 5)) {
                gameData.GetComponent<Text>().text = gameData.counter + "\nLukewarm End";
            }
            else {
                gameData.GetComponent<Text>().text = gameData.counter + "\nDid you even try?";
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

    //void Pressed() {
    //    if (myCard.isPressed) {
    //        Vector2 MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //        Vector2 objPosition = Camera.main.ScreenToWorldPoint(MousePosition);
    //        drawnCard.transform.position = objPosition;
    //    }
    //    else if (secondCard.isPressed) {
    //        Vector2 MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //        Vector2 objPosition = Camera.main.ScreenToWorldPoint(MousePosition);
    //        drawnSecondCard.transform.position = objPosition;
    //    }
    //}
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
    //public bool isPressed;

    public Card (Suit _suit, int _rank) {
        suit = _suit;
        rank = _rank;
        //isPressed = false;
    }



}
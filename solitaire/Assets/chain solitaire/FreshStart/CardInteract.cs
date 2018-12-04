﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInteract : MonoBehaviour {

    public bool isPressed;
    private Vector3 startPos;
    public AddOrSub addOrSub;
    public bool colliding;
    public int methodNumber;    //0 is Add, 1 is Subtract, 2 is AddSecond, 3 is SubtractSecond
    //public CardManager cardManager;

    void Start() {
        isPressed = false;
        startPos = this.transform.position;
        colliding = false;
        methodNumber = -1;
    }

    void Update() {
        Pressed();
    }

    // Use this for initialization
    void OnMouseDown() {
        isPressed = true;
    }

    void OnMouseUp() {
        if (colliding) {
            if (methodNumber == 0) {
                addOrSub.Add();
            }
            else if (methodNumber == 2) {
                addOrSub.AddSecond();
            }
            else if (methodNumber == 1) {
                addOrSub.Subtract();
            }
            else if (methodNumber == 3) {
                addOrSub.SubtractSecond();
            }
        }
        isPressed = false;
    }

    void Pressed() {
        if (isPressed) {
            //Vector2 MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Vector2 objPosition = Camera.main.ScreenToWorldPoint(MousePosition);
            //gameObject.transform.position = objPosition;

            Vector3 temp = Input.mousePosition;
            temp.z = 10f;
            this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(temp);
        }
        else {
            this.gameObject.transform.position = startPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        colliding = true;
        if (collision.tag == "add") {
            if (this.gameObject.tag == "card") {
                methodNumber = 0;
            }
            else {
                methodNumber = 2;
            }
        }
        else if (collision.tag == "subtract") {
            if (this.gameObject.tag == "card") {
                methodNumber = 1;
            }
            else {
                methodNumber = 3;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        colliding = false;
    }
}

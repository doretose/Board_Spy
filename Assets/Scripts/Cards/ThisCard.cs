﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThisCard : MonoBehaviour, IPointerClickHandler
{
    public List<Card> thisCard = new List<Card>();

    public int id;
    public string cardName;

    public Sprite iconSprite, nameSprite;
    public Image iconImage, nameImage;

    public bool cardBack = true;

    public GameObject Hand;

    public int numberOfCardsInDeck;

    //클릭된 카드의 상태를 확인하기 위한 bool형
    private bool cardSelect = false;

    void Start()
    {
        numberOfCardsInDeck = spyCardDeck.deckSize;
    }

    void Update()
    {
        Hand = GameObject.Find("Hand");

        if (this.transform.parent == Hand.transform.parent)
        {
            cardBack = false;
        }

        id = thisCard[0].id;
        cardName = thisCard[0].cardName;

        iconSprite = thisCard[0].thisImage;
        nameSprite = thisCard[0].cardNameImage;

        iconImage.sprite = iconSprite;
        nameImage.sprite = nameSprite;

        if(this.tag == "Clone")
        {
            thisCard[0] = spyCardDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            spyCardDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }

        if (this.tag == "Attack")
        {
            thisCard[0] = CardDataBase.cardList[1];
            cardBack = false;
            this.tag = "Untagged";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int? selectNum = NetworkRoundManager.selectCard;
        if (cardSelect)
        {
            NetworkRoundManager.selectCard = null;
            cardSelect = false;
            this.tag = "Untagged";
            GetComponent<CardToHand>().selectThisCard = false;
            return;
        }

        if (selectNum.HasValue)
        {
            return;
        }
        else
        {
            //플레이어 ID와 카드 ID값을 이벤트처리 배열에 입력
            NetworkRoundManager.selectCard = id;
            cardSelect = true;
            this.tag = "UseCard";
            GetComponent<CardToHand>().selectThisCard = true;
        }
    }
}

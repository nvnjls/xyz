using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strange.examples.CardGame {
	public class CardManager : MonoBehaviour , ICardsManager {

		[Inject]
		public ShowResultSignal showResult { get; set; }

		[Inject]
		public ShowWarningSignal showWarning { get; set; }

		[Inject]
		public UpdatePlayerScoreSignal updatePlayerScore { get; set; }

		[Inject]
		public UpdateAIScoreSignal updateAIScore { get; set; }

		public enum Type : int
		{
			BEGINNER = 0,
			INTERMEDIATE = 1 ,
			EXPERT = 2
		}

		[System.Serializable]
		public class GameMode
		{
			public Type _Type;
			public int _CardsCount;
			public bool _IsSingleStep ;
		}
		public List<GameMode> _GameModes;
		public static string SheetName ;
		public GameObject UiCardObj;
		public GameObject AICards;
		public GameObject PlayerCards;
		public float CardSpaceWidth = 40;
		public static Sprite[] sprites;
		public static GameMode pCurrentGameMode;
		/*
		 * This list will be used to make sure that the Random numbers will not be repeated
		 * */
		public static List<int> tempList;
		
		public static UiCard pSelectedCard = null;
		public static int mPlayerScore , mAIScore , mCheckCount;
		public static bool initDone = false;
		
		
		public UiCard GetSelectedCard ()
		{
			return pSelectedCard;
		}

		private int GetRandomInt()
		{
			int rand = Random.Range (0, 10);
			if (tempList.Contains (rand))
				return GetRandomInt ();
			else {
				tempList.Add (rand);
			}
			return rand;
		}

		/*
		 * This is to create a Card Object and assigning a Random priority .
		 * Here I am assuming that all the sprites in the spritesheet are arranged in Ascending Order.
		 * */
		private GameObject CreateObject(Transform parent , bool isAI = false)
		{
			GameObject obj = (GameObject)Instantiate(UiCardObj, parent.position, Quaternion.identity);
			int rand = GetRandomInt ();
			UiCard cardInfo = obj.GetComponent<UiCard>();
			cardInfo.pPriority = rand;
			if(!isAI)
				cardInfo.SetSprite (sprites [rand]);
			cardInfo.IsAI = isAI;
			obj.transform.parent = parent;
			return obj;
		}

		public void CardSelected(UiCard selectedCard)
		{
			UiCard []playerCards = PlayerCards.GetComponentsInChildren<UiCard> ();
			pSelectedCard = selectedCard;
			foreach (UiCard card in playerCards)
				if (card.pPriority != selectedCard.pPriority)
					card.ResetPosition ();
			showWarning.Dispatch (false);
		}

		
		public void GenerateCards(int count , bool mIsAICard)
		{
			Transform parentTransform;
			if (mIsAICard)
				parentTransform = AICards.transform;
			else
				parentTransform = PlayerCards.transform;
			for(int i =0 ; i < count; i++)
			{
				GameObject obj = CreateObject (parentTransform , mIsAICard);
				obj.transform.position = new Vector3(obj.transform.position.x + i  * CardSpaceWidth , obj.transform.position.y , obj.transform.position.z);
			}
		}

		/*
		 * This is to Show the AI cards to the User after the player clicks of Submit.
		 * */
		public void ShowAICards()
		{
			UiCard [] cards = GetAICards ();
			foreach(UiCard card in cards)
				card.SetSprite (sprites [card.pPriority]);
		}
		

		public UiCard[] GetAICards()
		{
			return  AICards.GetComponentsInChildren<UiCard> ();
		}
		public UiCard[] GetPlayerCards()
		{
			return  PlayerCards.GetComponentsInChildren<UiCard> ();
		}


		
		

		public UiCard GetLeastHigherPriorityAICard(UiCard selectedCard)
		{
			UiCard [] cards = GetAICards ();
			UiCard LeastHigherPriorityCard = null;
			foreach(UiCard card in cards)
			{
				if(card.pHasChecked)
					continue;
				if(card.pPriority > selectedCard.pPriority)
				{
					if(LeastHigherPriorityCard == null)
						LeastHigherPriorityCard = card;
					else if(LeastHigherPriorityCard.pPriority > card.pPriority)
						LeastHigherPriorityCard = card;
				}
			}
			if(LeastHigherPriorityCard != null)
			{
				LeastHigherPriorityCard.pHasChecked = true;
				LeastHigherPriorityCard.SetSprite(sprites[LeastHigherPriorityCard.pPriority]);
			}
			return LeastHigherPriorityCard;
		}
		/* 
		 * This is to get the Max priority card from the give array of Cards
		 * */
		public UiCard GetMaxPriorityCard(UiCard []cards)
		{
			UiCard maxCard = cards [0];
			foreach(UiCard card in cards)
			{
				if(maxCard.pPriority < card.pPriority)
					maxCard = card;
			}
			return maxCard;
		}
		public void DeleteAllCards()
		{
			UiCard []aicards = GetAICards();
			UiCard []playerCards = GetPlayerCards();
			foreach (UiCard card in aicards)
				DestroyObject (card.gameObject);
			foreach (UiCard card in playerCards)
				DestroyObject (card.gameObject);

		}
		public void SetGameMode(Type type)
		{
			foreach (GameMode mode in _GameModes)
				if (mode._Type == type)
					pCurrentGameMode = mode;
		}
	
		
	}
}

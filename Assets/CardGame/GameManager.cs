using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strange.examples.CardGame
{
     

    public class GameManager : MonoBehaviour, IGameManager
    {
        [Inject]
        public IScoreManager scoreManager { get; set; }

        [Inject]
        public ShowResultSignal showResult { get; set; }

        [Inject]
        public ICardsManager cardManager { get; set; }

        [Inject]
        public ShowWarningSignal showWarning { get; set; }

        public static GameManager pInstance;

        public void StartGame()
        {
            if (!CardManager.initDone)
            {
                pInstance = this;
                CardManager.sprites = Resources.LoadAll<Sprite>("playingcards");
                cardManager.SetGameMode(CardManager.Type.BEGINNER);
                CardManager.tempList = new List<int>();
                CardManager.initDone = true;
            }
            Restart();
        }
        public void OnCheck()
        {
            if ((CardManager.pSelectedCard == null && cardManager.GetPlayerCards().Length > 1) || (CardManager.pSelectedCard != null && CardManager.pSelectedCard.pHasChecked))
            {
                Debug.Log("Please select a card");
                //_WarningText.gameObject.SetActive(true);
                showWarning.Dispatch(true);
                return;
            }
            CardManager.mCheckCount++;
            if (CardManager.pSelectedCard == null)
                CardManager.pSelectedCard = cardManager.GetMaxPriorityCard(cardManager.GetPlayerCards());
            CardManager.pSelectedCard.pHasChecked = true;
            if (cardManager.GetLeastHigherPriorityAICard(CardManager.pSelectedCard) != null)
            {
                CardManager.mAIScore++;
                Debug.Log("Computer Won");
            }
            else
            {
                CardManager.mPlayerScore++;
                Debug.Log("Pllayer Won");
            }
            scoreManager.UpdateScores();
            if (CardManager.mCheckCount >= 3 || CardManager.pCurrentGameMode._IsSingleStep)
                scoreManager.OnSubmitScores();
            CardManager.pSelectedCard.ResetPosition();
            CardManager.pSelectedCard = null;
        }
        public void Restart()
        {
            CardManager.mPlayerScore = 0;
           CardManager. mAIScore = 0;
            CardManager.mCheckCount = 0;
           CardManager. pSelectedCard = null;
            scoreManager.UpdateScores();
            //_WonText.gameObject.SetActive (false);
            //_WarningText.gameObject.SetActive(false);
            showWarning.Dispatch(false);
            showResult.Dispatch(false, "");

            CardManager.tempList.Clear();
            cardManager.DeleteAllCards();
            /* For now we have only 2 players.i.e Player and AI Player(Computer/Mobile) , so hard coding for now
             * */
            cardManager.GenerateCards(CardManager.pCurrentGameMode._CardsCount, true);
            cardManager.GenerateCards(CardManager.pCurrentGameMode._CardsCount, false);
        }
    }
}
using UnityEngine;
using System.Collections;
namespace strange.examples.CardGame
{
    public class ScoreManager : MonoBehaviour, IScoreManager
    {
        [Inject]
        public ICardsManager cardManager { get; set; }

        [Inject]
        public UpdatePlayerScoreSignal updatePlayerScore { get; set; }

        [Inject]
        public UpdateAIScoreSignal updateAIScore { get; set; }
       
        [Inject]
        public ShowResultSignal showResult { get; set; }

        public void UpdateScores()
        {
            updatePlayerScore.Dispatch(CardManager.mPlayerScore);
            updateAIScore.Dispatch(CardManager.mAIScore);
        }

        public void OnSubmitScores()
        {
            cardManager.ShowAICards();
            if (CardManager.mAIScore > CardManager.mPlayerScore)
            {
                showResult.Dispatch(true, "You Lost");

                Debug.Log("Computer score is higher");
            }
            else if (CardManager.mAIScore < CardManager.mPlayerScore)
            {
                showResult.Dispatch(true, "You Won");

                Debug.Log("Player score is higher");
            }
            else
            {
                showResult.Dispatch(true, "Match Tie");

                Debug.Log("Both are equal");
            }
        }
    
    
    
    
    
    
    }




}
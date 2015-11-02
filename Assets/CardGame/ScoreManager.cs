using UnityEngine;
using System.Collections;
namespace strange.examples.CardGame
{
    public class ScoreManager : MonoBehaviour, IScoreManager
    {

        [Inject]
        public UpdatePlayerScoreSignal updatePlayerScore { get; set; }

        [Inject]
        public UpdateAIScoreSignal updateAIScore { get; set; }

        public void UpdateScores()
        {
            updatePlayerScore.Dispatch(CardManager.mPlayerScore);
            updateAIScore.Dispatch(CardManager.mAIScore);
        }
    }
}
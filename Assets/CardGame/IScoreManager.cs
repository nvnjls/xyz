using UnityEngine;
using System.Collections;
namespace strange.examples.CardGame{
    public interface IScoreManager
    {
        void UpdateScores();
        void OnSubmitScores();
    }

}

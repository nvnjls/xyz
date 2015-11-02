using UnityEngine;
using System.Collections;
namespace strange.examples.CardGame
{
    public interface IGameManager
    {
        void OnCheck();
        void StartGame();
  
    }
}
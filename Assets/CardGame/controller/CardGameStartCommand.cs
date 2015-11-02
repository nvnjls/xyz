using System;

using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.examples.CardGame {
	public class CardGameStartCommand : Command {

		[Inject]
		public IGameManager manager { get; set; }

		public override void Execute() {
			// perform all game start setup here
			Debug.Log("Hello World");
			manager.StartGame ();
		}
		
	}
}
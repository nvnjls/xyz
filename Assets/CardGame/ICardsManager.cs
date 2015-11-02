namespace strange.examples.CardGame {
	public interface ICardsManager {

		void CardSelected(UiCard selectedCard);
		UiCard GetSelectedCard();
		void SetGameMode(CardManager.Type mode);
        void ShowAICards();
        void DeleteAllCards();
        UiCard[] GetPlayerCards();
        void GenerateCards(int count, bool mIsAICard);
        UiCard GetMaxPriorityCard(UiCard[] cards);
        UiCard GetLeastHigherPriorityAICard(UiCard selectedCard);
	}
}
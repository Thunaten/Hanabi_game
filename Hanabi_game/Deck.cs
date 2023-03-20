using System;
using System.Collections.Generic;


namespace Hanabi_game
{
    class Deck
    {
        private Queue<Card> _deck = new Queue<Card>();
        public int СardsInDeckCounter { get; private set; } = 0;
        private bool IsOutOfCards
        {
            get
            {
                return _deck.Count == 0;
            }
        }
        private readonly char[] _cardColor = new char[] { 'W', 'B', 'R', 'G', 'Y' };
        private readonly int[] _cardRank = new int[] { 1, 1, 1, 2, 2, 3, 3, 4, 4, 5 };
        private Card TopCard;

        public Deck()
        {
            foreach (char color in _cardColor)
            {
                foreach (int rank in _cardRank)
                {
                    Card card = new Card(rank, color);
                    _deck.Enqueue(card);
                    СardsInDeckCounter++;
                }
            }
            ShuffleDeck();
        }

        private void ShuffleDeck()
        {
            List<Card> deckBeforeShuffle;
            Random random = new Random();
            deckBeforeShuffle = new List<Card>(СardsInDeckCounter);
            for (int i = 0; i < СardsInDeckCounter; i++)
            {
                Card shufflingCard = _deck.Dequeue();
                deckBeforeShuffle.Insert(random.Next(deckBeforeShuffle.Count), shufflingCard);
            }
            for (int i = 0; i < deckBeforeShuffle.Count; i++)
            {
                _deck.Enqueue(deckBeforeShuffle[i]);
            }
        }

        public void ShowState()
        {
            Console.SetCursorPosition(10, 9);
            if (IsOutOfCards)
                Console.WriteLine("В колоде не осталось карт! ");
            else
                Console.WriteLine($"В колоде осталось: {СardsInDeckCounter} карт. ");
        }

        public List<Card> DrawCard(List<Card> hand)
        {
            if (IsOutOfCards == false)
            {
                TopCard = _deck.Dequeue();
                hand.Add(TopCard);
                СardsInDeckCounter--;
                return hand;
            }
            else
            {
                ShowState();
                return hand;
            }
        }
    }
}

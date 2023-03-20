using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace Hanabi_game
{
    internal class Game
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            Deck deck = new Deck();
            Table table = new Table();
            // do {player.SelectAction()} while (EndGameCheck == false);
            //тест работоспособности
            player.StartPlay();
            for(int i = 0; i < deck._cardsInDeckCounter;)
            {
                //Console.ReadKey();
                player.SelectAction(table, player);
            }
            Console.ReadKey();
        }
    }

    class Player
    {
        public int HandSize { get; private set; } = 5;
        private List<Card> _cardsInHand = new List<Card>();
        private Deck _gameDeck = new Deck();
        private Card _activeCard;
        public int activeCardNumber;
        private int _cursorPositionX;
        private int _cursorPositionY;
        private int _cursorCurrentPosition;

        public Player()
        {

        }

        public void StartPlay()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Игра началась!");
            Console.ReadKey();
            while (_cardsInHand.Count < HandSize)
            {
                _gameDeck.DrawCard(_cardsInHand);
            }
            _gameDeck.ShowState();
            Console.SetCursorPosition(40, 16);
            Console.Write("Карты на руке:");
            ShowHand();
        }

        public void ShowHand()
        {
            Console.SetCursorPosition(40, 17);
            Console.Write("                                                  ");
            Console.SetCursorPosition(40, 17);
            foreach (Card cardInHand in _cardsInHand)
            {
                Console.Write(cardInHand.FullCardValue + " ");
            }
        }

        public void PlayerDraw()
        {
            if (_cardsInHand.Count < 5)
            {
            _gameDeck.DrawCard(_cardsInHand);
            _gameDeck.ShowState();
            ShowHand();
            }
            else
            {
                Console.SetCursorPosition(57, 17);
                Console.Write("Вы не можете взять больше карт");
            }
        }

        public Card PlayCard(int cardInHandNumber)
        {
            if (_cardsInHand.Count > 0 && cardInHandNumber <= _cardsInHand.Count && cardInHandNumber > 0)
            {
                _activeCard = _cardsInHand[cardInHandNumber - 1];
                _cardsInHand.RemoveAt(cardInHandNumber - 1);
                ShowHand();
                return _activeCard;
            }
            else
                return null;
        }

        public void SelectAction(Table table, Player player)
        {
            Console.SetCursorPosition(36, 19);
            Console.Write("Выберете действие:");
            Console.SetCursorPosition(36, 20);
            _cursorPositionY = Console.CursorTop;
            _cursorCurrentPosition = Console.CursorTop;
            Console.Write("Взять карту");
            Console.SetCursorPosition(36, 21);
            Console.Write("Сыграть карту");
            Console.SetCursorPosition(36, 20);
            Console.CursorSize = 100;
            ConsoleKey keyY;
            while ((keyY = Console.ReadKey(true).Key) != ConsoleKey.Enter)
            {
                if (keyY == ConsoleKey.UpArrow)
                {
                    if (_cursorCurrentPosition > _cursorPositionY)
                    {
                        _cursorCurrentPosition--;
                        Console.CursorTop = _cursorCurrentPosition;
                    }
                }
                else if (keyY == ConsoleKey.DownArrow)
                {
                    if (_cursorCurrentPosition < _cursorPositionY + 1)
                    {
                        _cursorCurrentPosition++;
                        Console.CursorTop = _cursorCurrentPosition;
                    }
                }
            }
            if (_cursorCurrentPosition == _cursorPositionY)
            {
                Console.SetCursorPosition(57, 17);
                Console.Write("                           ");
                PlayerDraw();
                Console.SetCursorPosition(36, 20);
            } else if (_cursorCurrentPosition == _cursorPositionY + 1)
            {
                Console.SetCursorPosition(41, 17);
                _cursorPositionX = Console.CursorLeft;
                _cursorCurrentPosition = Console.CursorLeft;
                ConsoleKey keyX;
                while ((keyX = Console.ReadKey(true).Key) != ConsoleKey.Enter)
                {
                    if (keyX == ConsoleKey.LeftArrow)
                    {
                        if (_cursorCurrentPosition > _cursorPositionX)
                        {
                            _cursorCurrentPosition = _cursorCurrentPosition - 3;
                            Console.CursorLeft = _cursorCurrentPosition;
                        }
                    }
                    else if (keyX == ConsoleKey.RightArrow)
                    {
                        if (_cursorCurrentPosition < _cursorPositionX + (_cardsInHand.Count - 1)*3)
                        {
                            _cursorCurrentPosition = _cursorCurrentPosition + 3;
                            Console.CursorLeft = _cursorCurrentPosition;
                        }
                    }
                }
                if (_cardsInHand.Count != 0)
                {
                    activeCardNumber = (((_cursorCurrentPosition - 41) / 3) + 1);
                    table.PlayOnTable(player, activeCardNumber);
                } 
                else
                {
                    Console.WriteLine("Закончились карты в руке.");
                    Console.SetCursorPosition(36, 20);
                }
            }

        }
    }

    class Deck
    {
        private Queue<Card> _deck = new Queue<Card>();
        public int _cardsInDeckCounter { get; private set; } = 0;
        private bool IsOutOfCards
        {
            get
            {
                return Convert.ToInt16(_deck.Count) == 0;
            }
        }
        private List<char> _cardColor = new List<char>() { 'W', 'B', 'R', 'G', 'Y' };
        private List<int> _cardRank = new List<int>() { 1, 1, 1, 2, 2, 3, 3, 4, 4, 5 };
        private Card TopCard;
        private List<Card> deckBeforeShuffle;

        public Deck()
        {
            foreach (char color in _cardColor)
            {
                foreach (int rank in _cardRank)
                {
                    Card card = new Card(rank, color);
                    _deck.Enqueue(card);
                    _cardsInDeckCounter++;
                }
            }
            _shuffleDeck();
        }

        private void _shuffleDeck()
        {
            Random random = new Random();
            deckBeforeShuffle = new List<Card>(_cardsInDeckCounter);
            for (int i = 0; i < _cardsInDeckCounter; i++)
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
                Console.WriteLine($"В колоде осталось: {_cardsInDeckCounter} карт. ");
        }

        public List<Card> DrawCard(List<Card> hand)
        {
            if (IsOutOfCards == false)
            {
                TopCard = _deck.Dequeue();
                hand.Add(TopCard);
                _cardsInDeckCounter--;
                return hand;
            }
            else
            {
                ShowState();
                return hand;
            }
        }
    }

    class Card
    {
        public int CardRank { get; private set; }
        public char CardColor { get; private set; }
        public string FullCardValue { get; private set; }

        public Card(int rank, char color)
        {
            CardRank = rank;
            CardColor = color;
            FullCardValue = (rank + "" + color);
        }
    }

    class Table
    {
        private List<Card> _cardOnTable = new List<Card>(25);
        private bool IsFullTable
        {
            get
            {
                return Convert.ToInt16(_cardOnTable.Count) == 25;
            }
        }
        private Card _activeCard;
        private int _dropCardCounter = 0;
        private bool _cardWasPlayed;
        
        public Table()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetCursorPosition(0, 1);
            Console.Write("Правила:" +
                "\nВаша цель заполнить стол разыгрывая карты" +
                "\nКарта имеет цвет(R,W,B,Y,G) и числовое значение (1-5)" +
                "\nПри разыгрывании карта оказывается на столе в двух случаях:" +
                "\n- если её значение 1 и нет другой карты того же цвета" +
                "\n- если её значение на еденицу больше последней карты того же лежащей на столе." +
                "\nВ противном случае карта сбрасывается. Предел карт на руке: 5." +
                "\nБудьте осторожны, карт с большими значениями меньше, чем с низкими. Удачи.");

            for (int i = 10; i < 15; i++)
            {
                Console.SetCursorPosition(40, i);
                Console.Write("** ** ** ** **");
            }
            Console.SetCursorPosition(31, 10);
            Console.Write(" __ " +
                "\n                               |  |" +
                "\n                               |  |" +
                "\n                                ‾‾ ");
            Console.SetCursorPosition(90, 0);
            Console.Write("Сброшенные карты:");
        }

        public void PlayOnTable(Player player, int cardForPlay)
        {
            do
            {
                _cardWasPlayed = false;
                _activeCard = player.PlayCard(cardForPlay);
                Console.SetCursorPosition(36, 20);

                if (_activeCard == null)
                {
                    Console.SetCursorPosition(40, 22);
                    Console.WriteLine("Такой карты нет. Попробуйте снова. ");
                }
                else if ((_activeCard.CardRank == 1) &&
                    (_cardOnTable.TrueForAll(x => x.CardColor != _activeCard.CardColor)))
                {
                    Console.SetCursorPosition(40, 22);
                    Console.WriteLine("                                                                                        ");
                    _cardOnTable.Add(_activeCard);
                    var (x, y) = CardPlasePosition(_activeCard);
                    Console.SetCursorPosition(x, y);
                    Console.Write(_activeCard.FullCardValue);
                    _cardWasPlayed = true;
                }
                else if ((_cardOnTable.Exists(x => ((x.CardRank == (_activeCard.CardRank - 1)) &&
                (x.CardColor == _activeCard.CardColor)))) &&
                (_cardOnTable.TrueForAll(x => (x.FullCardValue != _activeCard.FullCardValue))))
                {
                    Console.SetCursorPosition(40, 22);
                    Console.WriteLine("                                                                                        ");
                    _cardOnTable.Add(_activeCard);
                    var (x, y) = CardPlasePosition(_activeCard);
                    Console.SetCursorPosition(x, y);
                    Console.Write(_activeCard.FullCardValue);
                    _cardWasPlayed = true;
                    WinGameCheck();
                }
                else
                {
                    Console.SetCursorPosition(40, 22);
                    Console.WriteLine("                                                                                        ");
                    Console.SetCursorPosition(105, _dropCardCounter + 1);
                    Console.Write(_activeCard.FullCardValue);
                    _dropCardCounter++;
                    _cardWasPlayed = true;
                }
            } while (_cardWasPlayed == false || IsFullTable == true);
        }

        private (int, int) CardPlasePosition(Card playedCard)
        {
            int xPosition = 0;
            int yPosition = 0;
            
            switch (playedCard.CardColor)
            {
                case 'W':
                    xPosition = 40;
                    break;
                case 'R':
                    xPosition = 43;
                    break;
                case 'B':
                    xPosition = 46;
                    break;
                case 'Y':
                    xPosition = 49;
                    break;
                case 'G':
                    xPosition = 52;
                    break;
            }

            switch (playedCard.CardRank)
            {
                case 1:
                    yPosition = 10;
                    break;
                case 2:
                    yPosition = 11;
                    break;
                case 3:
                    yPosition = 12;
                    break;
                case 4:
                    yPosition = 13;
                    break;
                case 5:
                    yPosition = 14;
                    break;
            }

            return (xPosition, yPosition);
        }

        private void WinGameCheck()
        {
            if (IsFullTable == true)
            {
                Console.Clear();
                Console.SetCursorPosition(40, 9);
                Console.Write("Вы победили!");
            }
        }
    }
}

using System;
using System.Collections.Generic;


namespace Hanabi_game
{
    class Player
    {
        public int HandSize { get; private set; } = 5;
        public List<Card> cardsInHand = new List<Card>();
        private Card _activeCard;
        public int activeCardNumber;
        private int _cursorPositionX;
        private int _cursorPositionY;
        private int _cursorCurrentPosition;
        private readonly int _sizeFullCardValue = 3; //FullCardValue + " "
        private readonly int _firstCardInHandPosition = 41;

        public void StartPlay(Deck deck)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Игра началась!");

            Console.ReadKey();

            while (cardsInHand.Count < HandSize)
            {
                deck.DrawCard(cardsInHand);
            }

            deck.ShowState();

            Console.SetCursorPosition(40, 16);
            Console.Write("Карты на руке:");

            ShowHand();
        }

        private void ShowHand()
        {
            Console.SetCursorPosition(40, 17);
            Console.Write("                                                  ");
            Console.SetCursorPosition(40, 17);

            foreach (Card cardInHand in cardsInHand)
            {
                Console.Write(cardInHand.FullCardValue + " ");
            }
        }

        public void PlayerDraw(Deck deck)
        {
            if (cardsInHand.Count < 5)
            {
                deck.DrawCard(cardsInHand);

                deck.ShowState();

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
            if (cardsInHand.Count > 0 && cardInHandNumber <= cardsInHand.Count && cardInHandNumber > 0)
            {
                _activeCard = cardsInHand[cardInHandNumber - 1];

                cardsInHand.RemoveAt(cardInHandNumber - 1);

                ShowHand();

                return _activeCard;
            }
            else
                return null;
        }

        public void SelectAction(Table table, Player player, Deck deck)
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

                PlayerDraw(deck);
                Console.SetCursorPosition(36, 20);
            } 
            else if (_cursorCurrentPosition == _cursorPositionY + 1)
            {
                Console.SetCursorPosition(_firstCardInHandPosition, 17);
                _cursorPositionX = Console.CursorLeft;
                _cursorCurrentPosition = Console.CursorLeft;
                ConsoleKey keyX;

                while ((keyX = Console.ReadKey(true).Key) != ConsoleKey.Enter)
                {
                    if (keyX == ConsoleKey.LeftArrow)
                    {
                        if (_cursorCurrentPosition > _cursorPositionX)
                        {
                            _cursorCurrentPosition = _cursorCurrentPosition - _sizeFullCardValue;
                            Console.CursorLeft = _cursorCurrentPosition;
                        }
                    }
                    else if (keyX == ConsoleKey.RightArrow)
                    {
                        if (_cursorCurrentPosition < _cursorPositionX + (cardsInHand.Count - 1)*_sizeFullCardValue)
                        {
                            _cursorCurrentPosition = _cursorCurrentPosition + _sizeFullCardValue;
                            Console.CursorLeft = _cursorCurrentPosition;
                        }
                    }
                }
                if (cardsInHand.Count != 0)
                {
                    activeCardNumber = (((_cursorCurrentPosition - _firstCardInHandPosition) / _sizeFullCardValue) + 1);
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
}

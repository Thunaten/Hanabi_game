using System;
using System.Collections.Generic;
using System.Text;


namespace Hanabi_game
{
    class Table
    {
        private List<Card> _cardOnTable = new List<Card>(25);
        private bool IsFullTable
        {
            get
            {
                return _cardOnTable.Count == 25;
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

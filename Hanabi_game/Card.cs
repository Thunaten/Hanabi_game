namespace Hanabi_game
{
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
}

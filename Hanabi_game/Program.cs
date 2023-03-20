using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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

            player.StartPlay(deck);
            for(int i = 0; i < deck.СardsInDeckCounter;)
            {
                //Console.ReadKey();
                player.SelectAction(table, player, deck);
            }
            Console.ReadKey();
        }
    }
}

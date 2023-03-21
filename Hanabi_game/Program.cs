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
            while (table.GameOverCheck(player, deck) == false)
            {
                player.SelectAction(table, player, deck);
            }
            Console.ReadKey();
        }
    }
}

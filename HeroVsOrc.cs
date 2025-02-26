using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp11
{
    internal class Program
    {
        static int playerHP = 20;
        static int orcHP = 20;
        static int newPlayerHP = playerHP;
        static int newOrcHP = orcHP;

        static int LiveDamageAmountPlayer;
        static int LiveDamageAmountOrc;

        static int playerAction;
        static int orcAction;

        static bool playerHittable;
        static bool orcHittable;

        static int critChance = 8;
        static int missChance = 25;
        static int failChance = 10;

        static int player = 0;
        static int orc = 1;

        static bool gameOver = false;
        static void Main(string[] args)
        {
            Random random = new Random();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Blind Hero, defeat the blind orc ahead of you?");

            while (!gameOver)
            {
                playerHittable = true;
                orcHittable = true;
                if (newPlayerHP <= 0 || newOrcHP <= 0)
                { gameOver = true; }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"You: {newPlayerHP}/{playerHP}   Orc: {newOrcHP}/{orcHP}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"1 - Attack \n" +
                    $"2 - Defend");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("What will you do?");
                Console.ForegroundColor = ConsoleColor.Blue;
                try
                {
                    playerAction = Convert.ToInt32(Console.ReadLine());
                }
                catch 
                { 
                    playerAction = 0;
                }
                orcAction = random.Next(1,5);
                if (orcAction != 2 || newOrcHP > orcHP*0.75f)
                { orcAction = 1; }
                
                if (playerAction != 1 && playerAction != 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You hit yourself...");
                    Damage(player);
                }
                else
                {
                    if (playerAction == 2 && orcAction == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green
                            ; Console.WriteLine("It seems you both tried to defend, nothing happens...");
                    }
                    else
                    {
                        if (playerAction == 2) 
                        { Defend(player); }
                        if (orcAction == 2)
                        { Defend(orc); }
                    }

                    if (orcAction == 1) 
                    { Damage(player); }
                    if (playerAction == 1)
                    { Damage(orc); }
                }
                if (newPlayerHP <= 0 || newOrcHP <= 0)
                { gameOver = true; }
            }
            if (newPlayerHP > newOrcHP)
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"Congratulations you won!\n" +
                    "          o  o   o  o\r\n" +
                    "         |\\/ \\^/ \\/|\r\n" +
                    "         |,-------.|\r\n" +
                    "       ,-.(|)   (|),-.\r\n" +
                    "       \\_*._ ' '_.* _/\r\n" +
                    "        /`-.`--' .-'\\\r\n" +
                    "   ,--./    `---'    \\,--.\r\n" +
                    "   \\   |(  )     (  )|   /\r\n" +
                    "    \\  | ||       || |  /\r\n" +
                    "     \\ | /|\\     /|\\ | /\r\n" +
                    "     /  \\-._     _,-/  \\\r\n" +
                    "    //| \\\\  `---'  // |\\\\\r\n" +
                    "   /,-.,-.\\       /,-.,-.\\\r\n" +
                    "  o   o   o      o   o    o");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("You lost...\n" +
                    "                                   ,----.__                         |\r\n" +
                    "                                ,'        `.                       |\r\n" +
                    "                            _  /            :                      ,-.\r\n" +
                    "                           |.`:              :                    /  -\r\n" +
                    "            ,'''''-._      | )               :                 _.'  --\r\n" +
                    "           /         '.  _.`.   (88o    _    |_           _.-''      -\r\n" +
                    "           |           `/    |   \"\"\"   9@8o  / )-..__._.-'      ,/'`-/\r\n" +
                    "           ]     \\    ,:     `.         \"\"  :_/              ,-'  |\r\n" +
                    "            :     \\-_/        `. `a,    ,   :              ,'    /\r\n" +
                    "             `.    Y'       ,_  \\ \"7888\"  ,'   _.--''''---')     |\r\n" +
                    "               \\ .'      _/'  `._\\      ,'---.<...        /     |\r\n" +
                    "               .'      ,' '-.._   ':._,::...,'   /'     ,'      /\r\n" +
                    "              /'     ,/        '`''''           /     ,'       /\r\n" +
                    "             ,'    /  :                        /    ,'       ,-''''._\r\n" +
                    "             |    ()   :                      |    |      .-'        '\r\n" +
                    "             `.   :     ) __............____ .'    |_ .--'\r\n" +
                    "              `.   `.  ,'                   `/       `'-.__\r\n" +
                    "      _,....--'>    : /                     |   __...-._   `\\\r\n" +
                    "   ,-'       .' |   `.                      \"--'        ` ._/'--._\r\n" +
                    " ,'        /'    |   `.                                           '   \r\n" +
                    "/         (.,   /|     :                                            \\.\r\n" +
                    "             `'' :     :\\\r\n" +
                    "                 )     :.:\r\n" +
                    "                 : ; . ; '\r\n" +
                    "                 '_: . '\r\n" +
                    "                   '_:'");
            }

        }

        static void Damage(int targetID)
        {
            Random random = new Random();
            LiveDamageAmountPlayer = 0;
            LiveDamageAmountOrc = 0;
            int number = random.Next(1, 7);
            int Rng = random.Next(1, 101);
            bool miss = false;

            if (Rng <= critChance)
            { number += number / 2; Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Crit!"); }

            if (Rng <= missChance)
            { miss = true; }

            if (targetID == player && playerHittable) // Damage player
            {
                if (miss) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("You avoided the the attack!"); }
                else
                {
                    newPlayerHP -= number; 
                    Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"You lost {number}HP...");
                    LiveDamageAmountPlayer = number;
                }
            }
            else if (targetID == orc && orcHittable) // Damage orc
            {
                if (miss) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("You missed..."); }
                else
                {
                    newOrcHP -= number;
                    Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"The orc lost {number}HP!");
                    LiveDamageAmountOrc = number;
                }
            }
        }

        static void Defend(int targetID)
        {
            Random random = new Random();
            int Rng = random.Next(1, 101);
            bool fail = false;

            if (Rng <= 30)
            { fail = true; }

            if (targetID == player) // Defend player
            {
                if (fail) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("You failed to defend yourself..."); }
                else
                {
                    playerHittable = false;
                    newPlayerHP += LiveDamageAmountPlayer; Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"You successfully defended yourself");
                }
            }
            else if (targetID == orc) // Defend Orc
            {
                if (fail) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("The orc failed to defend himself!"); }
                else
                {
                    orcHittable = false;
                    newOrcHP += LiveDamageAmountOrc; Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("The orc successfully defended himself...");
                }
            }
        }
    }
}

namespace ConsoleApp11
{
    internal class HeroVsOrc
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
            Console.WriteLine(
                "Urhea ritari, sinut on lähetetty tappamaan kylää kiusaava örkki." +
                "\nLöydät örkin metsästä ja tämä hyökkää sinua kohti. Taitelu alkakoon!");

            while (!gameOver) // Pyörii niin kauan kunnes gameOver = true
            {
                // tekee molemmista osuttavia taas jos aikaisemmalla vuorolla joku onnistui puolustautua
                playerHittable = true; 
                orcHittable = true;

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"You: {newPlayerHP}/{playerHP}   Orc: {newOrcHP}/{orcHP}"); // Vuoron alun HP Pelaajalta ja örkiltä
                Console.ForegroundColor = ConsoleColor.Yellow;

                // Mahdolliset actionit
                Console.WriteLine(
                    $"1 - Hyökkää \n" +
                    $"2 - Puolusta"); 

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Mitä aijot tehdä?");
                Console.ForegroundColor = ConsoleColor.Blue;

                try
                {
                    playerAction = Convert.ToInt32(Console.ReadLine()); // Lukee Pelaajan syöttämän tekstin ja converttaa Integer arvoksi
                }
                catch
                {
                    playerAction = 0; // Jos pelaajan syöttämä teksti tuottaa errorin PlayerAction = 0
                }

                orcAction = random.Next(1, 5); // heittää noppaa yhden ja neljän välillä

                if (orcAction != 2 || newOrcHP > orcHP * 0.75f) // käytännössä tekee niin että örkki ei ikinä puolusta jos hänen Hp on yli 75% alkuperäisestä Hpeesta
                { orcAction = 1; }

                if (playerAction != 1 && playerAction != 2) // Jos pelaaja ei anna sopivaa vastausta hän osuu itseensä
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Löit itseäsi...");
                    Damage(player);
                }
                else
                {
                    if (playerAction == 2 && orcAction == 2) // Jos molemmat puolustautuvat
                    {
                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.WriteLine("Molemmat nostivat kilpensä, mitään ei tapahdu...");
                    }
                    else
                    {
                        if (playerAction == 2) // Jos pelaaja puolustautuu yritä puolustaa pelaajaa
                        { Defend(player); }
                        if (orcAction == 2) // Jos örkki puolustautuu yritä puolustaa örkkiä
                        { Defend(orc); }
                    }

                    if (orcAction == 1) // Jos örkki hyökkää yritä vahingoittaa pelaajaa
                    { Damage(player); }
                    if (playerAction == 1) // Jos pelaaja hyökkää yritä vahingoittaa örkkiä
                    { Damage(orc); }
                }

                if (newPlayerHP <= 0 || newOrcHP <= 0) // Jos pelaajan HP tai örkin HP on 0 tai vähemmän peli päättyy
                { gameOver = true; }
            }

            EndScreen(); // Jos poistuu while loopista gameOverin vuoksi valitsee sopivan voitto näytön joko voitto sammakon tai hävitessä örkistä kuvan

            static void Damage(int targetID)
            {
                Random random = new Random();

                int number = random.Next(1, 7); // Vahingon suuruus
                int Rng = random.Next(1, 101); // arvottu luku 1 ja 100 välillä
                bool miss = false; // Säilöö tiedon jos vahingoitus epäonnistuu ohilyönnin vuoksi

                if (Rng <= critChance) // jos arvottu luku osuu alle tai yhtäkuin critChancen, ja pienentää mahdollisuutta epäonnistua puolustautuessa
                { 
                    number += number / 2;
                    failChance -= 2;
                    Console.ForegroundColor = ConsoleColor.White; 
                    Console.WriteLine("Crit!"); 
                }

                if (Rng <= missChance) // jos arvottu luku osuu alle tai yhtäkuin missChance
                { miss = true; }

                if (targetID == player && playerHittable) // yritetään vahingoittaa pelaajaa
                {
                    if (miss) // jos örkki osuu ohi 
                    { 
                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.WriteLine("Väistit lyönnin!"); 
                    }
                    else // osuessa pelaajan Hpeeta vähennetään arvottu määrä ja päivitetään 
                    {
                        newPlayerHP -= number;
                        Console.ForegroundColor = ConsoleColor.Red; 
                        Console.WriteLine($"Menetit {number}HP...");
                        LiveDamageAmountPlayer = number;
                    }
                }
                else if (targetID == orc && orcHittable) // yritetään vahingoittaa örkkiä
                {
                    if (miss) // Jos pelaaja osuu ohi
                    { 
                        Console.ForegroundColor = ConsoleColor.Red; 
                        Console.WriteLine("Löit ohi..."); 
                    }
                    else // osuessa örkin Hpeeta vähennetään arvottu määrä ja säilötään paljonko vahinkoa tehtiin
                    {
                        newOrcHP -= number;
                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.WriteLine($"Örkki menetti {number}HP!");
                        LiveDamageAmountOrc = number; 
                    }
                }
            }

            static void Defend(int targetID)
            {
                Random random = new Random();
                int Rng = random.Next(1, 101); // arvottu luku 1 ja 100 välillä
                bool fail = false; // Säilöö tiedon jos puolustautuminen epäonnistuu
                failChance += 5; // vaikeutaa puolustautumista joka kerta kun puolustaudut

                if (Rng <= failChance) // Jos arvottu luku osuu alle failChancen
                { fail = true; }

                if (targetID == player) // Yritä puolustaa pelaajaa
                {
                    if (fail) // jos puolustautuminen epäonnistui
                    { 
                        Console.ForegroundColor = ConsoleColor.Red; 
                        Console.WriteLine("Kilpesi lipsahti kädestäsi..."); 
                    }
                    else // Jos puolustautuminen onnistui estä pelaajan vahingoittaminen
                    {
                        playerHittable = false;
                        newPlayerHP += LiveDamageAmountPlayer; // Jos aikaisemmalla vuorolla örkki vahingoitti pelaajaa paranna pelaajan Hpeeta sen verran
                        if (newPlayerHP > playerHP) // varmistaa ettei pelaajan Hp mene yli maximi Hp 
                        { newPlayerHP = playerHP; }
                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.WriteLine($"Puolustauduit kilvelläsi!");
                    }
                }
                else if (targetID == orc) // Yritä puolustaa örkkiä
                {
                    if (fail) // jos epäonnistuu
                    { 
                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.WriteLine("Örkin kilpi oli väärässä kädessä!"); 
                    }
                    else // Jos onnistuu estä örkin vahingoittaminen
                    {
                        orcHittable = false;
                        newOrcHP += LiveDamageAmountOrc; // Jos aikaisemmalla vuorolla pelaaja vahingoitti örkkiä paranna örkin Hpeeta vahingon määrällä
                        Console.ForegroundColor = ConsoleColor.Red; 
                        Console.WriteLine("Örkki onnistui suojata itsensä...");
                    }
                }
            }

            static void EndScreen() 
            {
                if (newPlayerHP > newOrcHP) // Jos Pelaaja voittaa piirtää voitto näytön
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(
                        "Tapoit örkin!\n" +
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
                else // Jos pelaaja häviää piirtää örkistä kuvan
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(
                        "Hävisit örkille...\n" +
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
        }
    }
}

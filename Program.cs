using System;
using System.Text;
using System.IO;

namespace Torpedo
{
    public class Palya
    {
        int hosszusag;
        int szelesseg;
        int hajokszama;
       // bool[,] palya;
       // bool[,] talalt;
            
        public Palya(int hosszusag, int szelesseg, int hajokszama,bool[,] palya,bool[,] talalt)
        {
            this.hosszusag = hosszusag;
            this.szelesseg = szelesseg;
            this.hajokszama = hajokszama;
            //this.palya = new bool[hosszusag, szelesseg];
            //this.talalt = new bool[hosszusag, szelesseg];
            for (int i = 0; i < hosszusag; i++)
            {
                for (int j = 0; j < szelesseg; j++)
                {
                    palya[i, j] = false;
                    talalt[i, j] = false;
                }
            }
        }

        public void palyaberendez(bool[,] palya, bool[,] talalt)
        {
            int k1 = 0;
            int k2 = 0;
                
            for (int i = 0; i < hajokszama; i++)
            {
                do
                {
                    Random r1 = new Random();
                    k1=r1.Next(hosszusag);
                    Random r2 = new Random();
                    k2 = r2.Next(szelesseg);
                } while (palya[k1, k2] == true);
                palya[k1, k2] = true;
            }
        }

        public void palyamutat(bool teszt, bool[,] palya, bool[,] talalt)
        {
            Console.Clear();
            if (teszt == false) // Súgó: hajók helyzetének megmutatása (true)
            {
                Console.WriteLine("Hajók elrendezése:");
                Console.Write("   ");
                for (int i = 0; i < szelesseg; i++)
                {
                    Console.Write(i+1);
                }
                Console.WriteLine();
                for (int i = 0; i < hosszusag; i++)
                {
                    Console.Write(Convert.ToChar(i+65) + ": ");

                    for (int j = 0; j < szelesseg; j++)
                    {
                        if (palya[i, j] == true)
                        {
                            Console.Write("O");
                        }
                        else
                        {
                            Console.Write("-");
                        }

                    }
                    Console.WriteLine("");
                }
            }

            // Pálya kirajzolása játékhoz
            Console.WriteLine();
            Console.WriteLine("Játéktér - találatok:");
            Console.Write("   ");
            for (int i = 0; i < szelesseg; i++)
            {
                Console.Write(i + 1);
            }
            Console.WriteLine();
            for (int i = 0; i < hosszusag; i++)
            {
                Console.Write(Convert.ToChar(i + 65) + ": ");
                for (int j = 0; j < szelesseg; j++)
                {
                    if (talalt[i, j] == true)
                    {
                        Console.Write("+");
                    }
                    else
                    {
                        Console.Write("-");
                    }

                }
                Console.WriteLine("");
            }
        }
    }

    public class Jatek
    {
        static int hosszusag = 4;
        static int szelesseg = 4;
        static int hajokszama = 3;
        static int talalatok = 0;
        static int hibasloves = 0;
        static bool[,] palya = new bool[20, 20];
        static bool[,] talalt = new bool[20, 20];
        Palya p = new Palya(hosszusag, szelesseg, hajokszama, palya, talalt);

        internal void palyaberendez(bool[,] palya, bool[,] talalt)
        {
            p.palyaberendez(palya,talalt);
        }

        internal void palyamutat(bool teszt, bool[,] palya, bool[,] talalt)
        {
            p.palyamutat(teszt, palya, talalt);
        }

        public void indit(bool teszt)
        {

            bool vege = false;
            int lovesekszama = 0;
            this.palyaberendez(palya, talalt);
            do
            {
                this.palyamutat(teszt, palya, talalt);
                vege = this.loves(teszt);
                lovesekszama++;
            }
            while (!vege);


            Console.WriteLine("Játék vége...");
            //Iras
            StreamWriter sw = new StreamWriter("eredmeny.txt");

            sw.WriteLine("Dátum:" + DateTime.Now+" Találatok száma:" +talalatok+" Hibás lövés:"+hibasloves);
            sw.Close();

        }

        private bool loves(bool teszt)
        {
            bool InputOK = false;
            bool vege = false;
            int koord1=0;
            int koord2=0;
            do
            {
                Console.WriteLine("Adja meg a lövés kordinátáit szóközzel elválasztva (pl. A 3)!");
                Console.WriteLine("Kilépés: ESC vagy 'v' és Enter");
                string kordinata = Console.ReadLine();

                string[] k;
                k = kordinata.Split(' ');
                string ESC = Convert.ToString(27);
                if (k[0] == "v" || k[0] == ESC)
                {
                    return true; // játék vége
                }

                if (k.Length!=2)
                {
                    Console.WriteLine("Pontosan két adat kell szóközzel elválasztva!");
                    Console.ReadLine();
                    continue;
                }
                
                byte[] k_ascii = Encoding.ASCII.GetBytes(k[0].ToUpper());
                koord1 = k_ascii[0] - 64;
                if (koord1> hosszusag)
                {
                    InputOK = false;
                    Console.WriteLine("Hibás koordináta!");
                    continue;
                }
                koord2 = Int32.Parse(k[1]);
                if (koord2 > szelesseg)
                {
                    InputOK = false;
                    Console.WriteLine("Hibás koordináta!");
                    continue;
                }
                if (koord1<1 || koord2<1)
                {
                    Console.WriteLine("Érvénytelen koordináta!" + koord1 + "," + koord2);
                    Console.ReadLine();
                    continue;
                }
                if (talalt[koord1 - 1, koord2 - 1])
                {
                    Console.WriteLine("Hajó a(z) " + koord1 + "," + koord2 + " koordinátán már talált, süllyedt!");
                    Console.ReadLine();
                    continue;
                }
                InputOK = true;
            } while (!InputOK);

            if (palya[koord1-1,koord2-1])
            {
                talalt[koord1 - 1, koord2 - 1] = true;
                Console.WriteLine("Talált,süllyedt!");
                talalatok++;
            }
            else
            {
                Console.WriteLine("Nem talált!");
                hibasloves++;
            }
            Console.ReadLine();

            if (talalatok==hajokszama)
            {
                vege = true;
            }
            return vege;
        }
        
       
    }

    class Program
    {

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;


            
            Jatek j = new Jatek();
            
            j.indit(true);
            Console.ReadKey();
        }
    }
}

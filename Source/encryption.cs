using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    partial class Program
    {
        static uint p;
        static int g;
        static int e2;
        static uint k;
        static Random EncryptionRand;


        public static void Encryption()
        {
            Console.WriteLine("Starting encryption.\n");

            //get values from text file
            var FromFile = File.ReadAllText("pubkey.txt");
            var array = FromFile.Split(' ');

            EncryptionRand = new Random();

            p = Convert.ToUInt32(array[0]);
            g = Convert.ToInt32(array[1]);
            e2 = Convert.ToInt32(array[2]);

            //Get plaintext
            var Plaintext = File.ReadAllText("ptext.txt");

            Console.WriteLine("p:\n\t" + p);
            Console.WriteLine("g:\n\t" + g);
            Console.WriteLine("e2:\n\t" + e2);
            Console.WriteLine("Plaintext from file:\n\t" + Plaintext);



            var charArray = Plaintext.ToArray();

            //This is the original plaintext in a list of characters
            var charList = new List<char>(charArray);

            //This is a character list that should only be 4 characters long
            var tempCharList = new List<char>();

            var outputList = new List<string>();

            while (charList.Count > 0)
            {
                tempCharList.Add(charList[0]);
                charList.RemoveAt(0);

                if(tempCharList.Count == 4)
                {
                    //Encrypt this 4 byte / 32 bit block
                    outputList.Add(GetC1C2(tempCharList));
                }
            }

            //tempCharList might have a block of info that is partially complete
            if(tempCharList.Count > 0)
            {
                outputList.Add(GetC1C2(tempCharList));
            }

            string WriteToFile = string.Join(" ", outputList.ToArray());

            Console.WriteLine("Encryption done. Writing to file:\n\t" + WriteToFile + "\n");

            File.WriteAllText("ctext.txt", WriteToFile);
        }

        public static string GetC1C2(List<char> input)
        {
            //Convert plaintext to ascii, then to 32 bit int
            int PlaintextAsInt = 0;

            //Leftmost ascii value is multiplied by 2^24
            while (input.Count > 0)
            {
                int ascii = input[0];
                input.RemoveAt(0);
                ascii = ascii * (int)Math.Pow(2, 8 * input.Count);
                PlaintextAsInt += ascii;
            }

            int m = PlaintextAsInt;

            Console.WriteLine("Plaintext converted to ascii characters, and then stored as an int:\n\t" + m);


            //int32 max is 2147483647
            int max = 2147483647;
            int diff = (int)(p - max);

            k = (uint)EncryptionRand.Next(1, max);
            k += (uint)EncryptionRand.Next(0, diff + 1);

            var c1 = ModPow((ulong)g, k, p);

            //Get e2^k mod p
            var x = ModPow((ulong)e2, (uint)k, (uint)p);

            //Get M mod p
            var y = m % p;

            //Multiply them
            var z = x * y;

            //Get the final result
            var c2 = z % p;

            return c1 + " " + c2;
        }
    }
}

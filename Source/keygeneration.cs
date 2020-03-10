using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        public static void KeyGeneration()
        {
            bool KeepLooping = true;

            Console.WriteLine("Generating keys.\n");

            uint p = 0;

            while (KeepLooping)
            {
                Console.WriteLine("Enter a number to be a random seed:");
                
                var seedString = Console.ReadLine();

                int seed;

                if(!int.TryParse(seedString, out seed))
                {
                    Console.WriteLine("Your input could not be converted to an int. Please try again.");
                }
                else
                {
                    KeepLooping = false;
                }
            }

            Console.WriteLine("\tSeed accepted.\n");
            //create random numbers in the range of 1073741824 < p < 2147483647
            //when q is prime, test for q % 12 = 5
            //if that is true, test for 2q + 1 = a prime

            var rand = new Random();

            KeepLooping = true;
            int q = 0;

            Console.WriteLine("Press enter to find a suitable prime (p).");
            Console.ReadLine();

            int LoopCount = 0;
            while (KeepLooping)
            {
                //Generate random value for q
                q = rand.Next(1073741823, 2147483647) + 1;

                //Skip even numbers
                if (q % 2 == 0)
                    q--;

                Console.WriteLine("[" + LoopCount + "] q selected: " + q);

                //Test if q is a prime
                if (IsPrime((ulong)q) && IsPrime((uint)q) && NaiveIsPrime((uint)q))
                {
                    Console.WriteLine("\tq was a prime.");
                    //Test if q % 12 = 5
                    if (q % 12 == 5)
                    {
                        Console.WriteLine("\t\tq % 12 did equal 5.");
                        //test if 2q + 1 is prime
                        //ulong check = Convert.ToUInt64(2 * q + 1);
                        p = (uint)(2 * q + 1);

                        if (NaiveIsPrime((uint)p) && IsPrime((uint)p))
                        {
                            Console.WriteLine("\t\t\t2 * q + 1 was a prime. Looping halted.");
                            KeepLooping = false;
                        }
                        else
                        {
                            Console.WriteLine("\t\t\t2 * q + 1 was not a prime.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\t\tq % 12 did not equal 5.");
                    }
                }
                else
                {
                    Console.WriteLine("\tq was not a prime.");
                }
                LoopCount++;
            }

            Console.WriteLine("Final p value: " + p + "\n");

            //Generator is 2
            //Pick a random d

            Console.WriteLine("Generator is 2.\n");

            //int32 max is 2147483647
            int max = 2147483647;
            int diff = (int)(p - max);

            uint d = (uint)rand.Next(0, max);
            d += (uint)rand.Next(0, diff + 1);

            Console.WriteLine("Selecting a random value for d:\n\t" + d + "\n");

            var e2 = ModPow(2, (uint)d, (uint)p);

            Console.WriteLine("e2 = 2 ^ " + d + " mod " + p + ":\n\t" + e2 + "\n");

            //write to file
            //public file:
            //p g e2
            string pubkey = p + " 2 " + e2;

            Console.WriteLine("Writing to pubkey.txt:\n\t" + pubkey);
            File.WriteAllText("pubkey.txt", pubkey);

            //private file:
            //p g d
            string prikey = p + " 2 " + d;
            Console.WriteLine("Writing to prikey.txt:\n\t" + prikey + "\n");
            File.WriteAllText("prikey.txt", prikey);

            Console.WriteLine("Key generation complete.\n");
        }
    }
}

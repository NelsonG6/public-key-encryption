using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ConsoleApp1
{
    partial class Program
    {        
        static void Main(string[] args)
        {
            bool KeepLooping = true;

            Console.WriteLine("CS485 Project 2 program started.\n");

            while(KeepLooping)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("(1) Key generation");
                Console.WriteLine("(2) Encryption");
                Console.WriteLine("(3) Decryption");

                var input = Console.ReadLine();

                Console.WriteLine();

                if (input.Contains("1"))
                    KeyGeneration();
                else if (input.Contains("2"))
                    Encryption();
                else if (input.Contains("3"))
                    Decryption();

                Console.WriteLine("(1) Keep going");
                Console.WriteLine("(2) Stop");

                input = Console.ReadLine();

                if (input.Contains("1"))
                    Console.WriteLine("\nContinuing.\n");
                else if (input.Contains("2"))
                    KeepLooping = false;
            }

            Console.WriteLine("Program exiting. Press enter to close.");
            Console.ReadLine();
        }
    }
}
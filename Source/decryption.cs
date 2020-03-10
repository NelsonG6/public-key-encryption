using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        public static void Decryption()
        {
            Console.WriteLine("Beginning decryption.\n");

            var fromFile = File.ReadAllText("prikey.txt");
            var split = fromFile.Split(' ');
            var p = Convert.ToUInt32(split[0]);
            var d = Convert.ToUInt32(split[2]);

            Console.WriteLine("p:\n\t" + p);
            Console.WriteLine("d:\n\t" + d);

            var ciphertextFromFile = File.ReadAllText("ctext.txt");
            var splitCipher = ciphertextFromFile.Split(' ');
            //splitCipher should have pairs of c1 and c2

            var CiphertextList = splitCipher.ToList();

            //Store the decrypted output text
            var charList = new List<char>();

            char ConvertedFromInt;

            while (CiphertextList.Count > 0)
            {
                var c1 = Convert.ToUInt32(CiphertextList[0]);
                var c2 = Convert.ToUInt32(CiphertextList[1]);

                //Console.WriteLine("c1:\n\t" + c1);
                //Console.WriteLine("c2:\n\t" + c2);

                CiphertextList.RemoveAt(0);
                CiphertextList.RemoveAt(0);

                uint exp = p - 1 - d;

                var first = ModPow((ulong)c1, exp, p);
                var second = c2 % p;

                BigInteger third = (BigInteger)first * (BigInteger)second;
                
                var tempM = (third) % p;

                var m = (uint)tempM;

                //Console.WriteLine("m:\n\t" + m);

                byte[] bytes = BitConverter.GetBytes(m);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                var byteList = bytes.ToList();
                //First 4 entries are empty for whatever reason
                while(byteList[0] == 0)
                {
                    byteList.RemoveAt(0);
                }

                while(byteList.Count > 0)
                {
                    ConvertedFromInt = (char)byteList[0];
                    charList.Add(ConvertedFromInt);
                    byteList.RemoveAt(0);
                }

                //Convert m from an integer to the original ascii
            }

            var charArray = charList.ToArray();
            var decrypted = string.Join("", charArray);

            Console.WriteLine("Decrypted text:\n\t" + decrypted);

        }
    }
}

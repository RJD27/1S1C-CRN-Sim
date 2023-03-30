using System;

namespace Reachability1S1C
{
    internal class Functions
    {
        public static void DisplaySpecies(int[] speciesInts)
        {
            Console.WriteLine("A: " + speciesInts[0]);
            Console.WriteLine("B: " + speciesInts[1]);
            Console.WriteLine("C: " + speciesInts[2]);
            Console.WriteLine("D: " + speciesInts[3]);

            Console.WriteLine("-----------------------------------------");
        }
        public static void AddingSpecies(int[] speciesInts, char leftLetter, string leftNum1, char leftLetter2, string leftNum2, char rightLetter, string rightNum1, char rightLetter2, string rightNum2)
        {
            int subNum1 = int.Parse(leftNum1), subNum2 = int.Parse(leftNum2);
            int addNum1 = int.Parse(rightNum1), addNum2 = int.Parse(rightNum2);

            speciesInts[Char.ToUpper(leftLetter) - 'A'] += subNum1;
            speciesInts[Char.ToUpper(leftLetter2) - 'A'] += subNum2;

            speciesInts[Char.ToUpper(rightLetter) - 'A'] -= addNum1;
            speciesInts[Char.ToUpper(rightLetter2) - 'A'] -= addNum2;

            DisplaySpecies(speciesInts);

            System.Threading.Thread.Sleep(1500);
        }

        public static void SubtractingSpecies(int[] speciesInts, char leftLetter, string leftNum1, char leftLetter2, string leftNum2, char rightLetter, string rightNum1, char rightLetter2, string rightNum2)
        {
            int subNum1 = int.Parse(leftNum1), subNum2 = int.Parse(leftNum2);
            int addNum1 = int.Parse(rightNum1), addNum2 = int.Parse(rightNum2);

            speciesInts[Char.ToUpper(leftLetter) - 'A'] -= subNum1;
            speciesInts[Char.ToUpper(leftLetter2) - 'A'] -= subNum2;

            speciesInts[Char.ToUpper(rightLetter) - 'A'] += addNum1;
            speciesInts[Char.ToUpper(rightLetter2) - 'A'] += addNum2;

            DisplaySpecies(speciesInts);

            System.Threading.Thread.Sleep(1500);
        }
    }
}



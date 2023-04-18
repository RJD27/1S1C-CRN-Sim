using System;
using System.Linq;

namespace Reachability1S1C
{
    internal class Functions
    {
        public static void DisplaySpecies(int[] speciesInts, char[] speciesLetters)
        {
            for (int i = 0; i < speciesLetters.Length; i++)
            {
                Console.WriteLine(Char.ToUpper(speciesLetters[i]) + ": " + speciesInts[i]);
            }
            Console.WriteLine("-----------------------------------------");
        }

        public static void IsSelectingRightRule(ref int rightNum1, ref int rightNum2, ref char rightLetter1, ref char rightLetter2, string selectedRuleRight)
        {
            rightLetter1 = '\0';
            rightLetter2 = '\0';
            rightNum1 = 0;
            rightNum2 = 0;
            // Can be shrunken down
            foreach (char r in selectedRuleRight)
            {
                if (char.IsDigit(r))
                {
                    if (rightLetter1 == '\0')
                    {
                        rightNum1 = rightNum1 * 10 + (r - '0');
                    }
                    else
                    {
                        rightNum2 = rightNum2 * 10 + (r - '0');
                    }
                }
                else if (char.IsLetter(r))
                {

                    if (rightLetter1 == '\0')
                    {
                        rightLetter1 = r;
                    }
                    else
                    {
                        rightLetter2 = r;

                    }
                }
            }
        }

        public static void IsSelectingLeftRule(ref char leftLetter1, ref char leftLetter2, string selectedRuleLeft, ref int leftNum1, ref int leftNum2)
        {
            leftLetter1 = '\0';
            leftLetter2 = '\0';
            leftNum1 = 0;
            leftNum2 = 0;

            foreach (char r in selectedRuleLeft)
            {
                if (char.IsDigit(r))
                {
                    if (leftLetter1 == '\0')
                    {
                        leftNum1 = leftNum1 * 10 + (r - '0');
                    }
                    else
                    {
                        leftNum2 = leftNum2 * 10 + (r - '0');
                    }
                }
                else if (char.IsLetter(r))
                {

                    if (leftLetter1 == '\0')
                    {
                        leftLetter1 = r;
                    }
                    else
                    {
                        leftLetter2 = r;

                    }
                }
            }
        }


        public static void AddingSpecies(ref int[] speciesInts, char[] speciesLetters, ref char leftLetter1, ref int leftNum1, ref char leftLetter2, ref int leftNum2, ref char rightLetter1, ref int rightNum1, ref char rightLetter2, ref int rightNum2, bool printOutput)
        {

            speciesInts[Char.ToUpper(leftLetter1) - 'A'] += leftNum1;
            speciesInts[Char.ToUpper(leftLetter2) - 'A'] += leftNum2;

            speciesInts[Char.ToUpper(rightLetter1) - 'A'] -= rightNum1;
            speciesInts[Char.ToUpper(rightLetter2) - 'A'] -= rightNum2;

            if (printOutput)
            {
                DisplaySpecies(speciesInts, speciesLetters);
                System.Threading.Thread.Sleep(1500);
            }
        }

        public static void SubtractingSpecies(ref int[] speciesInts, char[] speciesLetters, ref char leftLetter1, ref int leftNum1, ref char leftLetter2, ref int leftNum2, ref char rightLetter1, ref int rightNum1, ref char rightLetter2, ref int rightNum2, bool printOutput)
        {

            speciesInts[Char.ToUpper(leftLetter1) - 'A'] -= leftNum1;
            speciesInts[Char.ToUpper(leftLetter2) - 'A'] -= leftNum2;

            speciesInts[Char.ToUpper(rightLetter1) - 'A'] += rightNum1;
            speciesInts[Char.ToUpper(rightLetter2) - 'A'] += rightNum2;

            if (printOutput)
            {
                DisplaySpecies(speciesInts, speciesLetters);
                System.Threading.Thread.Sleep(1500);
            }
        }

        public static bool ArraysEqual(int[] a1, int[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }// can put species reached here

            return true;
        }

        public static (int, int) MaxDiff(int[] speciesInts, int[] reachInts, bool[] existNegativeNumber)
        {
            int maxDiff = int.MinValue;
            int maxIndex = -1;
            for (int i = 0; i < speciesInts.Length; i++)
            {
                if (!existNegativeNumber[i])
                {
                    int diff = Math.Abs(speciesInts[i] - reachInts[i]);
                    if (diff > maxDiff)
                    {
                        maxDiff = diff;
                        maxIndex = i;
                    }
                }
            }
            return (maxDiff, maxIndex);
        }

    }
}



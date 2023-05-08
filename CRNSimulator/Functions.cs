using System;
using System.Collections.Generic;
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



        public static void AddingSpecies(ref int[] speciesInts, int[] reachInts, bool[] speciesFlag, char[] speciesLetters, ref char leftLetter1, ref int leftNum1, ref char leftLetter2, ref int leftNum2, ref char rightLetter1, ref int rightNum1, ref char rightLetter2, ref int rightNum2, bool printOutput)
        {
            if (!IsSpeciesRepeated(speciesInts, reachInts, speciesFlag, speciesLetters, leftLetter1, leftLetter2, rightLetter1, rightLetter2))
            {
                speciesInts[Char.ToUpper(leftLetter1) - 'A'] += leftNum1;
                speciesInts[Char.ToUpper(leftLetter2) - 'A'] += leftNum2;

                speciesInts[Char.ToUpper(rightLetter1) - 'A'] -= rightNum1;
                speciesInts[Char.ToUpper(rightLetter2) - 'A'] -= rightNum2;
                if (printOutput)
                {
                    Console.WriteLine("Left Species: " + leftLetter1 + " " + leftLetter2);
                    Console.WriteLine("Right Species: " + rightLetter1 + " " + rightLetter2);
                    DisplaySpecies(speciesInts, speciesLetters);
                    //System.Threading.Thread.Sleep(1500);
                }
            }

        }

        public static void SubtractingSpecies(ref int[] speciesInts, int[] reachInts, bool[] speciesFlag, char[] speciesLetters, ref char leftLetter1, ref int leftNum1, ref char leftLetter2, ref int leftNum2, ref char rightLetter1, ref int rightNum1, ref char rightLetter2, ref int rightNum2, bool printOutput)
        {
            if (!IsSpeciesRepeated(speciesInts, reachInts, speciesFlag, speciesLetters, leftLetter1, leftLetter2, rightLetter1, rightLetter2))
            {
                speciesInts[Char.ToUpper(leftLetter1) - 'A'] -= leftNum1;
                speciesInts[Char.ToUpper(leftLetter2) - 'A'] -= leftNum2;

                speciesInts[Char.ToUpper(rightLetter1) - 'A'] += rightNum1;
                speciesInts[Char.ToUpper(rightLetter2) - 'A'] += rightNum2;

                if (printOutput)
                {
                    Console.WriteLine("Left Species: " + leftLetter1 + " " + leftLetter2);
                    Console.WriteLine("Right Species: " + rightLetter1 + " " + rightLetter2);
                    DisplaySpecies(speciesInts, speciesLetters);
                    //System.Threading.Thread.Sleep(1500);
                }
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

        public static (int, int) LowestDiff(int[] speciesInts, int[] reachInts, bool[] speciesFlag)
        {
            int lowestDiff = int.MaxValue;
            int lowestIndex = 0;
            for (int i = 0; i < speciesInts.Length; i++)
            {
                if (!speciesFlag[i])
                {
                    int diff = Math.Abs(speciesInts[i] - reachInts[i]); // Convert negative values to positive values using Math.Abs
                    if (diff == 0) continue;  // Skip species with difference of 0
                    if (diff < lowestDiff)
                    {
                        lowestDiff = diff;
                        lowestIndex = i;
                    }
                }
            }
            return (lowestDiff, lowestIndex);
        }



        public static bool IsSpeciesRepeated(int[] speciesInts, int[] reachInts, bool[] speciesFlag, char[] speciesLetters, char leftLetter1, char leftLetter2, char rightLetter1, char rightLetter2)
        {
            int lowestIndex = LowestDiff(speciesInts, reachInts, speciesFlag).Item2;
            if (lowestIndex == -1)
            {
                return false;
            }
            char currentSpecies = Char.ToUpper(speciesLetters[lowestIndex]);

            int specCount = 0;
            if (Char.ToUpper(leftLetter1) == currentSpecies) specCount++;
            if (Char.ToUpper(leftLetter2) == currentSpecies) specCount++;
            if (Char.ToUpper(rightLetter1) == currentSpecies) specCount++;
            if (Char.ToUpper(rightLetter2) == currentSpecies) specCount++;

            return specCount == 2 || specCount == 4;
        }

        public static int CheckingForRepeatedCycles(int[] speciesInts, ref bool unreachable, ref int checkingForRepetition)
        {
            List<int[]> speciesHistory = new List<int[]>();
            speciesHistory.Add((int[])speciesInts.Clone());
            if (checkingForRepetition >= 1)
            {
                for (int speciesClone = 0; speciesClone < checkingForRepetition; speciesClone++)
                {
                    if (speciesClone < speciesHistory.Count)
                    {
                        bool areEqual = Enumerable.SequenceEqual(speciesInts, speciesHistory[speciesClone]);
                        if (areEqual)
                        {
                            if (speciesHistory.Count(h => Enumerable.SequenceEqual(h, speciesInts)) > 1)
                            {
                                unreachable = true;
                                return -1;
                            }
                        }
                    }
                }
            }
            speciesHistory.Add((int[])speciesInts.Clone());
            return speciesHistory.Count - 1;
        }
        public static int GetIncrementValue(int lowestDiff, char[] speciesLetters, int lowestIndex, string selectedRule, char letter1, int num1, char letter2, int num2)
        {
            if (lowestDiff == 1)
            {
                return 1;
            }
            else if (selectedRule.Contains(Char.ToLower((char)(lowestIndex + 'A'))) && letter1 == speciesLetters[lowestIndex])
            {
                return lowestDiff / num1;
            }
            else if (selectedRule.Contains(Char.ToLower((char)(lowestIndex + 'A'))) && letter2 == speciesLetters[lowestIndex])
            {
                return lowestDiff / num2;
            }
            else
            {
                return 0;
            }
        }
        public static void UpdateSpeciesFlag(int[] speciesInts, int[] reachInts, bool[] speciesFlag, int lowestIndex)
        {
            if (speciesFlag[lowestIndex])
            {
                return;
            }

            if (speciesInts[lowestIndex] != reachInts[lowestIndex])
            {
                speciesFlag[lowestIndex] = true;
            }
            else if (speciesInts[lowestIndex] == reachInts[lowestIndex])
            {
                Array.Fill(speciesFlag, false);
                Console.WriteLine("Species reached!");
            }
        }

    }
}



using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Reachability1S1C;

namespace CRNSimulator
{
    class Program
    {
        public static void Main(string[] args)
        {

            string[] species = { };
            string[] reachSpecies = { };
            string[] reachCheck = { };
            char[] speciesLetters = { };

            int i;
            int speciesNum = 0;
            bool validInput = false;
            string input;
            char letter = 'a';

            //Asking for how many species a person is going to use
            while (!validInput)
            {
                Console.Write("How many species do you want to work with (between 4 - 26): ");
                input = Console.ReadLine();

                if (int.TryParse(input, out speciesNum) && speciesNum >= 4 && speciesNum <= 26)
                {
                    validInput = true;
                    species = new string[speciesNum];
                    reachSpecies = new string[speciesNum];
                    speciesLetters = new char[speciesNum];

                }
                else
                {
                    Console.WriteLine("Please pick a number between 4 and 26.");
                }
            }

            Console.WriteLine("===========================================");

            //Checking for initial amount of species
            for (i = 0; i < species.Length; i++)
            {
                int isNum;
                bool isNumber = false;
                speciesLetters[i] = letter;
                while (!isNumber)
                {
                    Console.Write("How many " + letter + "'s do you want to start with: ");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out isNum) && isNum >= 0)
                    {
                        species[i] = input;
                        isNumber = true;

                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input.");
                    }
                }

                isNumber = false;
                while (!isNumber)
                {
                    Console.Write("How many " + letter + "'s do you want to reach: ");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out isNum) && isNum >= 0)
                    {
                        reachSpecies[i] = input;
                        isNumber = true;

                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input.");
                    }
                }

                letter++;
            }

            //Check if starting species counts equals reachability species counts
            Console.WriteLine("===========================================");
            int totalSpecies = 0;
            int totalReach = 0;

            int[] speciesInts = Array.ConvertAll(species, int.Parse);
            int[] reachInts = Array.ConvertAll(reachSpecies, int.Parse);


            for (i = 0; i < species.Length; i++)
            {
                totalSpecies += speciesInts[i];
                totalReach += reachInts[i];
            }

            Console.WriteLine(totalSpecies);
            Console.WriteLine(totalReach);
            if (totalSpecies == totalReach)
            {
                Console.WriteLine("1st check complete!");
            }
            else
            {
                Console.WriteLine("Not reachable for a 1-source 1-consuming CRN.");
            }

            Console.WriteLine("===========================================");
            //Input user rules
            Console.WriteLine("What restrictions do you want to implement (Type 'q' when done): ");

            string[] userRules = { };

            int numRule = 0;
            do
            {
                input = Console.ReadLine().ToLower();
                if (input != "q")
                {
                    input = Regex.Replace(input, @"[^a-zA-Z0-9=]|=(?=.*=)", "");

                    if (input.Length >= 9)
                    {
                        Array.Resize(ref userRules, numRule + 1);
                        userRules[numRule] = input;
                        numRule++;
                    }
                }
            } while (input != "q" && input != "Q");

            int[,] oneSC = new int[2, speciesNum];
            string currentRule;
            char currentSpecies = 'a';
            bool afterEquals = false;
            bool isCatalyst = false;
            bool not1s1c = false;
            bool onlyOnce = false;
            string notInt = "";
            int isInt = 0;
            int isBInt = 0;
            int isAInt = 0;

            //Checks if crn Rule set is 1-source 1-consuming
            for (int l = 0; l < speciesNum; l++)
            {
                for (i = 0; i < userRules.Length; i++)
                {
                    currentRule = userRules[i];
                    afterEquals = false;
                    isCatalyst = false;
                    for (int j = 0; j < currentRule.Length; j++)
                    {
                        if (currentRule[j] == currentSpecies && afterEquals == false)
                        {
                            isCatalyst = true;
                            oneSC[0, l] += 1;
                        }
                        else if (currentRule[j] == '=')
                        {
                            afterEquals = true;
                        }
                        else if (currentRule[j] == currentSpecies && afterEquals == true)
                        {
                            oneSC[1, l] += 1;

                            if (oneSC[1, l] > 1 && isCatalyst == false)
                            {
                                Console.WriteLine("Not a 1-source 1-consuming CRN due to species " + currentSpecies + " being more than 1-source.");
                                not1s1c = true;
                            }
                            else if (isCatalyst == true)
                            {
                                oneSC[0, l] -= 1;
                                oneSC[1, l] -= 1;
                            }
                        }
                        else if (oneSC[0, l] > 1 && j == currentRule.Length - 1)
                        {
                            Console.WriteLine("Not a 1-source 1-consuming CRN due to species " + currentSpecies + " being more than 1-consuming.");
                            not1s1c = true;
                        }
                    }
                }
                currentSpecies++;
            }

            if (not1s1c == true)
            {
                Console.WriteLine("Not 1-source 1-consuming due to above reasons");
            }

            //checks for equal amount of species being consumed and produced
            for (i = 0; i < userRules.Length; i++)
            {
                currentRule = userRules[i];
                afterEquals = false;
                for (int j = 0; j < currentRule.Length; j++)
                {
                    if (currentRule[j] == '=')
                    {
                        afterEquals = true;
                    }

                    else if (currentRule[j] >= '0' && currentRule[j] <= '9' && afterEquals == false)
                    {
                        for (int x = j; currentRule[x]! >= '0' && currentRule[x]! <= '9'; x++)
                        {
                            notInt += currentRule[x];
                            j += 1;
                        }
                        isInt = int.Parse(notInt);
                        isBInt += isInt;
                        notInt = "";
                    }
                    else if (currentRule[j] >= '0' && currentRule[j] <= '9' && afterEquals == true)
                    {
                        for (int x = j; currentRule[x]! >= '0' && currentRule[x]! <= '9'; x++)
                        {
                            notInt += currentRule[x];
                            j += 1;
                        }
                        isInt = int.Parse(notInt);
                        isAInt += isInt;
                        notInt = "";
                    }
                }

                Console.WriteLine(isBInt);
                Console.WriteLine(isAInt);
                if (isBInt != isAInt && onlyOnce == false)
                {
                    int current = i + 1;
                    Console.WriteLine("Rule " + current + " is not equal on both consuming and producing sides.");
                    onlyOnce = true;
                }

                isBInt = 0;
                isAInt = 0;
                onlyOnce = false;
            }

            //checks if species has correct rule to manipulate for reachability
            currentSpecies = 'a';
            for (int s = 0; s < speciesInts.Length; s++)
            {
                if (speciesInts[s] > reachInts[s])
                {
                    if (oneSC[0, s] < 1)
                    {
                        Console.WriteLine("Not reachable because species " + currentSpecies + " counts cannot go lower");
                    }
                }
                if (speciesInts[s] < reachInts[s])
                {
                    if (oneSC[1, s] < 1)
                    {
                        Console.WriteLine("Not reachable because species " + currentSpecies + " counts cannot go higher");
                    }
                }
                currentSpecies++;
            }


            //splitting each rule from left to right in their own array
            string[] subLeft = new string[userRules.Length];
            string[] addRight = new string[userRules.Length];

            for (i = 0; i < userRules.Length; i++)
            {
                string[] ruleSplit = userRules[i].Split('=');
                subLeft[i] = ruleSplit[0];
                addRight[i] = ruleSplit[1];
            }


            //Algorithm
            /*bool complete = false;
            do
            {
                char specCheck = 'A';

                for (int j = 0; j < species.Length; j++){

                    specCheck++;
                }


            } while (reachInts == speciesInts);*/

            for (i = 0; i < speciesInts.Length; i++)
            {
                string leftNum1 = "", leftNum2 = "", rightNum1 = "", rightNum2 = "";

                string selectedRuleLeft, selectedRuleRight;
                char leftLetter = '\0', leftLetter2 = '\0', rightLetter = '\0', rightLetter2 = '\0';


                //grabs each rule from the left and right of '='
                for (int f = 0; f < userRules.Length; f++)
                {
                    selectedRuleLeft = subLeft[f];
                    selectedRuleRight = addRight[f];

                    //traverses through the selected rule looking for species
                    foreach (char l in selectedRuleLeft)
                    {
                        if (char.IsDigit(l))
                        {
                            if (leftLetter == '\0')
                            {
                                leftNum1 += l;
                            }
                            else
                            {
                                leftNum2 += l;
                            }
                        }
                        else if (char.IsLetter(l))
                        {
                            if (leftLetter == '\0')
                            {
                                leftLetter = l;
                            }
                            else
                            {
                                leftLetter2 = l;

                            }
                        }
                    }

                    foreach (char r in selectedRuleRight)
                    {
                        if (char.IsDigit(r))
                        {
                            if (rightLetter == '\0')
                            {
                                rightNum1 += r;
                            }
                            else
                            {
                                rightNum2 += r;
                            }
                        }
                        else if (char.IsLetter(r))
                        {

                            if (rightLetter == '\0')
                            {
                                rightLetter = r;
                            }
                            else
                            {
                                rightLetter2 = r;

                            }
                        }
                    }

                    //Checking if leftLetter needs subtracting or adding
                    if (speciesInts[Char.ToUpper(leftLetter) - 'A'] > reachInts[Char.ToUpper(leftLetter) - 'A'] || speciesInts[Char.ToUpper(leftLetter2) - 'A'] > reachInts[Char.ToUpper(leftLetter2) - 'A'])
                    {
                        Console.WriteLine("Subtracting from species " + Char.ToUpper(leftLetter) + "...");

                        while (speciesInts[Char.ToUpper(leftLetter) - 'A'] > reachInts[Char.ToUpper(leftLetter) - 'A'] || speciesInts[Char.ToUpper(leftLetter2) - 'A'] > reachInts[Char.ToUpper(leftLetter2) - 'A'])
                        {
                            Functions.SubtractingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }

                        Console.WriteLine("No subtracting needed for " + Char.ToUpper(leftLetter) + "!");

                    }
                    
                    if (speciesInts[Char.ToUpper(leftLetter) - 'A'] < reachInts[Char.ToUpper(leftLetter) - 'A'] || speciesInts[Char.ToUpper(leftLetter2) - 'A'] < reachInts[Char.ToUpper(leftLetter2) - 'A'])
                    {
                        while (speciesInts[Char.ToUpper(leftLetter) - 'A'] < reachInts[Char.ToUpper(leftLetter) - 'A'] || speciesInts[Char.ToUpper(leftLetter2) - 'A'] < reachInts[Char.ToUpper(leftLetter2) - 'A'])
                        {
                            Functions.AddingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }
                        Console.WriteLine("No adding needed!");
                    }

                    /*//Checking if leftLetter2 needs subtracting or adding
                    if (speciesInts[Char.ToUpper(leftLetter2) - 'A'] > reachInts[Char.ToUpper(leftLetter2) - 'A'])
                    {
                        Console.WriteLine("Subtracting from species " + speciesInts[Char.ToUpper(leftLetter2) - 'A'] + "...");

                        while (speciesInts[Char.ToUpper(leftLetter2) - 'A'] > reachInts[Char.ToUpper(leftLetter2) - 'A'])
                        {
                            Functions.SubtractingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }

                        Console.WriteLine("No subtracting needed!");

                    }
                    else if (speciesInts[Char.ToUpper(leftLetter2) - 'A'] < reachInts[Char.ToUpper(leftLetter2) - 'A'])
                    {
                        while (speciesInts[Char.ToUpper(leftLetter2) - 'A'] < reachInts[Char.ToUpper(leftLetter2) - 'A'])
                        {
                            Functions.AddingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }
                        Console.WriteLine("No adding needed!");
                    }*/

                    //Checking if rightLetter  needs adding or subtracting
                    if (speciesInts[Char.ToUpper(rightLetter) - 'A'] > reachInts[Char.ToUpper(rightLetter) - 'A'] || speciesInts[Char.ToUpper(rightLetter2) - 'A'] > reachInts[Char.ToUpper(rightLetter2) - 'A'])
                    {
                        while (speciesInts[Char.ToUpper(rightLetter) - 'A'] > reachInts[Char.ToUpper(rightLetter) - 'A'] || speciesInts[Char.ToUpper(rightLetter2) - 'A'] > reachInts[Char.ToUpper(rightLetter2) - 'A'])
                        {
                            Functions.SubtractingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }

                        Console.WriteLine("No subtracting needed!");
                    }
                    else if (speciesInts[Char.ToUpper(rightLetter) - 'A'] < reachInts[Char.ToUpper(rightLetter) - 'A'] || speciesInts[Char.ToUpper(rightLetter2) - 'A'] < reachInts[Char.ToUpper(rightLetter2) - 'A'])
                    {
                        while (speciesInts[Char.ToUpper(rightLetter) - 'A'] < reachInts[Char.ToUpper(rightLetter) - 'A'] || speciesInts[Char.ToUpper(rightLetter2) - 'A'] < reachInts[Char.ToUpper(rightLetter2) - 'A'])
                        {
                            Functions.AddingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }

                        Console.WriteLine("No adding needed!");
                    }
/*
                    //Checking if rightLetter2  needs adding or subtracting
                    if (speciesInts[Char.ToUpper(rightLetter2) - 'A'] > reachInts[Char.ToUpper(rightLetter2) - 'A'])
                    {
                        while (speciesInts[Char.ToUpper(rightLetter2) - 'A'] < reachInts[Char.ToUpper(rightLetter2) - 'A'])
                        {
                            Functions.SubtractingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }

                        Console.WriteLine("No subtracting needed!");
                    }
                    else if (speciesInts[Char.ToUpper(rightLetter2) - 'A'] < reachInts[Char.ToUpper(rightLetter2) - 'A'])
                    {
                        while (speciesInts[Char.ToUpper(rightLetter2) - 'A'] < reachInts[Char.ToUpper(rightLetter2) - 'A'])
                        {
                            Functions.AddingSpecies(speciesInts, leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2);
                        }

                        Console.WriteLine("No adding needed!");
                    }*/
                }

            }


            /*Console.WriteLine(oneSC[0, 0]);
            Console.WriteLine(oneSC[1, 0]);
            Console.WriteLine(oneSC[0, 1]);
            Console.WriteLine(oneSC[1, 1]);
            Console.WriteLine(oneSC[0, 2]);
            Console.WriteLine(oneSC[1, 2]);
            Console.WriteLine(oneSC[0, 3]);
            Console.WriteLine(oneSC[1, 3]);*/

            //Rule Logic Plan B
/*            Program pr = new Program();
            int ruleNum = 0;
            pr.RuleLogic(ruleNum, ref userRules, ref speciesLetters, ref speciesInts, ref reachInts);*/
        }

        //Rule Logic Function Plan B
       /* public void RuleLogic(int ruleNum, ref string[] userRules, ref char[] speciesLetters, ref int[] speciesInts, ref int[] reachInts)
        {
            char let;
            string cRule = userRules[ruleNum];
            bool afterEqual = false;
            string notInt = "";
            int isInt = 0;
            for (int i = 0; i < cRule.Length;)
            {
                let = cRule[i];
                var index = Array.IndexOf(speciesLetters, let);
                if (afterEqual == false && let >= 'a' && let <= 'z')
                {
                    speciesInts[index] -= isInt;
                    i++;
                }
                else if (let == '=')
                {
                    afterEqual = true;
                    i++;
                }
                else if (afterEqual == true && let >= 'a' && let <= 'z')
                {
                    speciesInts[index] += isInt;
                    i++;
                }
                else if (let >= '0' && let <= '9')
                {
                    for (int x = i; cRule[x] !>= '0' && cRule[x] !<= '9'; x++)
                    {
                        notInt += cRule[x];
                        i++;
                    }
                    isInt = int.Parse(notInt);
                    notInt = "";
                }
            }*/
            //Needs for loop to check each species for reach
            /*if (speciesInts == reachInts) 
            {
                Console.WriteLine("Success, reached!");
            }
            else
            {
                Console.WriteLine("Not reached");
            }

            Console.WriteLine(speciesInts);
            Console.WriteLine(reachInts);*/

        //}
    }
}

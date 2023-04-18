using System.Threading;
using Reachability1S1C;
using System;
using System.Text.RegularExpressions;
using System.Linq;

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

            int speciesNum = 0;
            bool validInput = false;
            string input;
            char letter = 'a';

            //Asking for how many species a person is going to use
            while (!validInput)
            {
                Console.Write("How many species do you want to work with (between 2 - 26): ");
                input = Console.ReadLine();

                if (int.TryParse(input, out speciesNum) && speciesNum >= 2 && speciesNum <= 26)
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
            for (int i = 0; i < species.Length; i++)
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


            for (int i = 0; i < species.Length; i++)
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
                for (int i = 0; i < userRules.Length; i++)
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
            for (int i = 0; i < userRules.Length; i++)
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

            for (int i = 0; i < userRules.Length; i++)
            {
                string[] ruleSplit = userRules[i].Split('=');
                subLeft[i] = ruleSplit[0];
                addRight[i] = ruleSplit[1];
            }


            // Algorithm
            bool[] existNegativeNumber = new bool[speciesInts.Length];

            while (!Functions.ArraysEqual(reachInts, speciesInts))
            {
                int rightNum1 = 0, rightNum2 = 0, leftNum1 = 0, leftNum2 = 0;

                string selectedRuleLeft, selectedRuleRight;
                char leftLetter1 = '\0', leftLetter2 = '\0', rightLetter1 = '\0', rightLetter2 = '\0';

                var (greaterDiff, speciesIndex) = Functions.MaxDiff(speciesInts, reachInts, existNegativeNumber);
                Console.WriteLine(greaterDiff + " " + speciesIndex);
                Thread.Sleep(1500);

                // Spliting the rules on the left and right side of '='
                for (int num = 0; num < userRules.Length; num++)
                {
                    double totalInc = 0;

                    selectedRuleLeft = subLeft[num];
                    selectedRuleRight = addRight[num];

                    // If greaterDiff species in speciesInt needs to subtract to reach reachInts and checking where that species is in the rules
                    if (speciesInts[speciesIndex] > reachInts[speciesIndex] && selectedRuleLeft.Contains(Char.ToLower((char)(speciesIndex + 'A'))) && !existNegativeNumber[speciesIndex])
                    {
                        Functions.IsSelectingLeftRule(ref leftLetter1, ref leftLetter2, selectedRuleLeft, ref leftNum1, ref leftNum2);
                        Functions.IsSelectingRightRule(ref rightNum1, ref rightNum2, ref rightLetter1, ref rightLetter2, selectedRuleRight);
                        Console.WriteLine("leftnum1: " + leftNum1 + " LeftNum2: " + leftNum2 + " LeftLetter1: " + leftLetter1 + " LeftLetter2: " + leftLetter2);
                        Console.WriteLine("rightnum1: " + rightNum1 + " rightNum2: " + rightNum2 + " rightLetter1: " + rightLetter1 + " rightLetter2: " + rightLetter2);
                        // Add and subract until the species reaches reachInts

                        totalInc = speciesInts[speciesIndex] / greaterDiff;
                        Console.WriteLine(greaterDiff + " " + speciesInts[speciesIndex]);

                        do
                        {
                            for (int inc = 0; inc < totalInc; inc++)
                            {
                                // Subtraction Loop
                                int[] tempSpeciesInts = (int[])speciesInts.Clone();
                                Functions.SubtractingSpecies(ref tempSpeciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, false);

                                if (tempSpeciesInts.Any(x => x < 0))
                                {
                                    existNegativeNumber[speciesIndex] = true;
                                    break;
                                }

                                Functions.SubtractingSpecies(ref speciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, true);
                                Console.WriteLine(speciesInts[speciesIndex] + " " + reachInts[speciesIndex]);
                            }

                            if (existNegativeNumber[speciesIndex]) 
                            { 
                                break; 
                            }

                            // Addition Loop
                            double subDiff = reachInts[speciesIndex] - speciesInts[speciesIndex];
                            while (subDiff % 1 != 0)
                            {
                                int[] tempSpeciesInts = (int[])speciesInts.Clone();
                                Functions.AddingSpecies(ref tempSpeciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, false);

                                if (tempSpeciesInts.Any(x => x < 0))
                                {
                                    existNegativeNumber[speciesIndex] = true;
                                    break;
                                }
                                Functions.AddingSpecies(ref speciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, true);
                                subDiff = reachInts[speciesIndex] - speciesInts[speciesIndex];
                            }

                            if (existNegativeNumber[speciesIndex]) 
                            { 
                                break; 
                            }

                            // Subtracting till reach 
                            for (int addDiff = 0; addDiff < subDiff; addDiff++)
                            {
                                Functions.SubtractingSpecies(ref speciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, true);
                            }

                            if(speciesInts[speciesIndex] == reachInts[speciesIndex]){
                                Array.Fill(existNegativeNumber, false);
                                Console.WriteLine("Species reached!");
                            }

                        } while (speciesInts[speciesIndex] != reachInts[speciesIndex] && !existNegativeNumber[speciesIndex]);
                    }

                    // If greaterDiff species in speciesInts needs to add to reach reachInts and checking where that species is in the rules
                    if (speciesInts[speciesIndex] < reachInts[speciesIndex] && selectedRuleRight.Contains(Char.ToLower((char)(speciesIndex + 'A'))))
                    {
                        Functions.IsSelectingRightRule(ref rightNum1, ref rightNum2, ref rightLetter1, ref rightLetter2, selectedRuleRight);
                        Functions.IsSelectingLeftRule(ref leftLetter1, ref leftLetter2, selectedRuleLeft, ref leftNum1, ref leftNum2);
                        Console.WriteLine("leftnum1: " + leftNum1 + " LeftNum2: " + leftNum2 + " LeftLetter1: " + leftLetter1 + " LeftLetter2: " + leftLetter2);
                        Console.WriteLine("rightnum1: " + rightNum1 + " rightNum2: " + rightNum2 + " rightLetter1: " + rightLetter1 + " rightLetter2: " + rightLetter2);

                        totalInc = speciesInts[speciesIndex] / greaterDiff;
                        Console.WriteLine(greaterDiff + " " + speciesInts[speciesIndex]);
                        do
                        {
                            for (int inc = 0; inc < totalInc; inc++)
                            {
                                int[] tempSpeciesInts = (int[])speciesInts.Clone();
                                Functions.SubtractingSpecies(ref tempSpeciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, false);

                                if (tempSpeciesInts.Any(x => x < 0))
                                {
                                    existNegativeNumber[speciesIndex] = true;
                                    break;
                                }

                                Functions.SubtractingSpecies(ref speciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, true);
                                Console.WriteLine(speciesInts[speciesIndex] + " " + reachInts[speciesIndex]);
                            }

                            if (existNegativeNumber[speciesIndex]) 
                            { 
                                break; 
                            }

                            double subDiff = reachInts[speciesIndex] - speciesInts[speciesIndex];
                            while (subDiff % 1 != 0)
                            {
                                int[] tempSpeciesInts = (int[])speciesInts.Clone();
                                Functions.AddingSpecies(ref tempSpeciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, false);

                                if (tempSpeciesInts.Any(x => x < 0))
                                {
                                    existNegativeNumber[speciesIndex] = true;
                                    break;
                                }

                                Functions.AddingSpecies(ref speciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, true);
                                subDiff = reachInts[speciesIndex] - speciesInts[speciesIndex];
                            }

                            if (existNegativeNumber[speciesIndex]) 
                            { 
                                break; 
                            }
                            
                            for (int addDiff = 0; addDiff < subDiff; addDiff++)
                            {
                                Functions.SubtractingSpecies(ref speciesInts, speciesLetters, ref leftLetter1, ref leftNum1, ref leftLetter2, ref leftNum2, ref rightLetter1, ref rightNum1, ref rightLetter2, ref rightNum2, true);
                            }

                            if(speciesInts[speciesIndex] == reachInts[speciesIndex]){
                                Array.Fill(existNegativeNumber, false);
                                Console.WriteLine("Species reached!");
                            }

                        } while (speciesInts[speciesIndex] != reachInts[speciesIndex] && !existNegativeNumber[speciesIndex]);
                    }
                }
            }

        }
    }
}

//traverses through the selected rule looking for species
// check if letter is in rule, else change to the next rule
// maybe track used species???


/*//Algorithm
for (int i = 0; i < speciesInts.Length; i++)
{



    //grabs each rule from the left and right of '='



    *//*while (speciesInts != reachInts)
    {
        int greaterDiff = 0, diffIndex = 0, increment = 0;
        Functions.GreaterDiff(speciesInts, reachInts, greaterDiff, diffIndex);

        // Calling subtract rule and adding due to greaterDiff holding a negative number
        if (speciesInts[diffIndex] > reachInts[diffIndex])
        {
            increment = greaterDiff / speciesInts[diffIndex];
            //may need work
            for(int r = 0; r < increment; r++)
            {
                //fix this up but a general idea
                //make it so that it accepts the index of the species and may need to change rules stuff

                Functions.SubtractingSpecies(speciesInts[diffIndex], leftLetter, leftNum1, leftLetter2, leftNum2, rightLetter, rightNum1, rightLetter2, rightNum2); 
            }
        }


    }*//*


}*/
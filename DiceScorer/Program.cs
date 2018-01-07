namespace DiceScorer
{
    /* FYI: I made the conscious choice to prioritize efficiency over readability/maintainability
     * as this program is only one function call, but I hope my comments will help compensate for that choice
     */
    class Program
    {
        static void Main(string[] args)
        {
            int[] dice = new int[] { 1, 1, 1, 4, 8 };
            int score = GetScore(dice);
        }

        /* I do not calculate these scores because they will never be greater than the Chance score: 
         * Ones, Twos, Threes, Fours, Fives, Sixes, Sevens, Eights, ThreeOfAKind, FourOfAKind
         * I do not calculate LargeStraight because NoneOfAKind would also always be present &
         * is less expensive to calculate
         * This function assumes dice parameter is valid and not user-provided
         * And that the order for sequences, like SmallStraight, matter
         */
        public static int GetScore(int[] dice)
        {
            bool smallStraightAtIndexZero = true;
            bool smallStraightAtIndexOne = true;
            int chanceScore = 0;
            int largestCount = 0;
            int secondLargestCount = 0;
            int tempScore = 0;
            int[] dieCount = new int[8];

            int dIndex = 0;
            int dLength = dice.Length;
            while (dIndex < dLength)                        // Build array to keep counts of numbers rolled
            {
                dieCount[dice[dIndex]-1] += 1;
                if (dIndex < dLength - 2 
                    && dice[dIndex] != dice[dIndex+1] - 1)  // Check for smallStraight criteria during loop
                {
                    smallStraightAtIndexZero = false;
                }
                if (dIndex < dLength - 1 &&
                    dIndex > 0
                   && dice[dIndex] != dice[dIndex+1] - 1)
                {
                    smallStraightAtIndexOne = false;
                }
                chanceScore += dice[dIndex];                // Also calculate chanceScore (the sum) during loop
                dIndex++;
            }

            int cIndex = 0;
            int cLength = dieCount.Length;
            while (cIndex < cLength)                        // Find largestCount for allOfAKind & noneOfAKind checks
            {
                if (largestCount < dieCount[cIndex])
                {
                    secondLargestCount = largestCount;      // Save secondLargestCount for fullHouse check
                    largestCount = dieCount[cIndex];
                }
                else if (secondLargestCount < dieCount[cIndex])
                {
                    secondLargestCount = dieCount[cIndex];
                }
                cIndex++;
            }

            if (largestCount == 5) return 50;               // Check for allOfAKind first because highest possible score
            if (largestCount == 1) tempScore = 40;          // Check for noneOfAKind second because second highest possible score
            if (smallStraightAtIndexZero == true            // Check for smallStraight...
                || smallStraightAtIndexOne == true) 
            {
                tempScore = 30;
            }
            else if (largestCount == 3
                && secondLargestCount == 2)                 // Check for fullHouse...
            {
                tempScore = 25;
            }
            if (chanceScore < tempScore) return tempScore;
            return chanceScore;
        }
    }
}


using System.Collections.Immutable;

class WordComparer
{
    private ImmutableArray<string> WordsList { get; }
    private string WordToCompare { get; }
    private ImmutableArray<string> WordToComparePermutations { get; }

    public WordComparer(ImmutableArray<string> wordsList, string wordToCompare)
    {
        this.WordsList = wordsList;
        this.WordToCompare = wordToCompare;
        this.WordToComparePermutations = CreatePermutations();
    }

    private ImmutableArray<string> CreatePermutations()
    {
        var permutations = new List<string>();
        var wordLength = this.WordToCompare.Length;
        var splitWord = this.WordToCompare.OrderBy(c => c).ToList();
        var allPermutationsFactorial = CalculateFactorial(wordLength);
        var factorialMultiple = CalculateFactorial(wordLength - 1);
        var multipleTracker = factorialMultiple;

        Console.WriteLine(allPermutationsFactorial);
        
        var allPerms = new char[allPermutationsFactorial, wordLength];

        // allPerms[1, 2] = 'a';

        var iterationStartPoint = 0;
        for (int x = 0; x < wordLength; x++)
        {
            var space = 0;
            for (int y = iterationStartPoint; y < allPermutationsFactorial; y++)
            {
                allPerms[y, space] = splitWord[x];

                space = (space == wordLength - 1) ? 0 : space + 1;

            }
            space = x;
            for (int y = 0; y < iterationStartPoint; y++)
            {
                allPerms[y, space] = splitWord[x];

                // space = (space == wordLength - 1) ? 0 : space + 1;

                if (space == wordLength - 1 && x != wordLength - 1)
                {
                    space = 0;
                }
                else if (x == wordLength - 1)
                {
                    space = wordLength - 1;
                }
                else
                {
                    space++;
                }

            }
            space = (space != wordLength - 1) ? space++ : space;
            iterationStartPoint++;
            // "abc"
            // "acb"
            // "bac"
            // "bca"
            // "cab"
            // "cba"

            // "a  "
            // "ba "
            // " ba"
            // "a  "
            // " a "
            // "  a"

            // "abcd"
            // "abdc"
            // "acbd"
            // "acdb"
            // "adcb"
            // "adbc"
        }

        for (int i = 0; i < allPerms.GetLength(0); i++)
        {
            var totalString = new char[allPerms.GetLength(1)];
            for (int o = 0; o < allPerms.GetLength(1); o++)
            {
                totalString[o] = allPerms[i, o];
            }
            permutations.Add(new string(totalString));
        }

        foreach (var item in permutations.OrderBy(i => i))
        {
            // Console.WriteLine("Here");
            Console.WriteLine(item);
        }

        return permutations.ToImmutableArray();
    }

    public void FindWords()
    {
        this.WordsList.Where(currentWord => CompareWord(currentWord));
    }

    public bool CompareWord(string compareAgainstWord)
    {
        return string.Equals(this.WordToCompare, compareAgainstWord);
    }

    private int CalculateFactorial(int factorial) {
        return Enumerable.Range(1, factorial).Aggregate(1, (p, item) => p * item);
    }
}
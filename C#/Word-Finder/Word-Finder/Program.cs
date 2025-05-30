// Words JSON from:
// https://github.com/jonathanwelton/word-lists

class Program
{
    public static void Main(string[] args)
    {
        const string wordToFind = "strategic";
        var wordLength = wordToFind.Length;

        if (wordLength > 10) {
            Console.WriteLine("Too many letters, exiting...");
            System.Environment.Exit(0);
        }

        //Path to JSON file
        var path = $@"..\..\..\resources\{wordLength}-letter-words.json";
        var wordsList = FileReader.readJSONFile(path);

        var wc = new WordComparer(wordsList, wordToFind);

        Console.WriteLine("Permutations:");
        foreach (var item in wc.WordToComparePermutations)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine("Found Matches:");
        foreach (var item in wc.WordMatches)
        {
            Console.WriteLine(item);
        }
    }
}


using System.Collections.Immutable;

class WordComparer
{
    private ImmutableArray<string> WordsList { get; }
    public List<string> WordToComparePermutations { get; }
    public List<string> WordMatches { get; }
    private string WordToCompare { get; }

    public WordComparer(ImmutableArray<string> wordsList, string wordToCompare)
    {
        this.WordsList = wordsList;
        this.WordToComparePermutations = new List<string>();
        this.WordToCompare = string.Concat(wordToCompare.OrderBy(c => c));
        CreatePermutations("", this.WordToCompare);
        this.WordMatches = FindWords();
    }

    private void CreatePermutations(string prefix, string word)
    {
        int currentLength = word.Length;
        if (currentLength == 0) { this.WordToComparePermutations.Add(prefix); }
        else
        {
            for (int i = 0; i < currentLength; i++)
            {
                CreatePermutations(prefix + word[i], word.Substring(0, i) + word.Substring(i + 1, currentLength - (i + 1)));
            }
        }
    }

    private List<string> FindWords()
    {
        return this.WordsList.Where(word => this.WordToComparePermutations.Contains(word)).ToList();
    }
}
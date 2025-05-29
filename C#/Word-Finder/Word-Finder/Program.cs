class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        //Path to JSON file
        var path = @"";
        var wordsList = FileReader.readJSONFile(path);
        Console.WriteLine(wordsList[0].ToString());
        Console.WriteLine("asdf");
        var wc = new WordComparer(wordsList, "bcad");
    }
}

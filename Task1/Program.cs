var input = "The \"C# Professional\" course includes the topics I discuss in my CLR via C# book and " +
"teaches how the CLR works thereby showing you how to develop applications and reusable " +
"components for the .NET Framework.";

var output = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).GroupBy(s => s.Length).OrderBy(group => group.Key).Select(words =>
    new
    {
        WordsLength = words.Key,
        Count = words.Distinct().Count(),
        Words = words.Select(w => w).Distinct()
    }
);

foreach (var words in output)
{
    Console.WriteLine($"Words of length: {words.WordsLength}, Count:{words.Count}");
    foreach (var word in words.Words)
    {
        Console.WriteLine(word);
    }
}
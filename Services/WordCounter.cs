public class WordCounter
{
    public int CountSpecificWord(string text, string word)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(word)) return 0;
        var words = text.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var count = words.Count(w => w.Equals(word, StringComparison.InvariantCultureIgnoreCase));
        return count;
    }
}
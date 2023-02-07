using System.Text.RegularExpressions;
class Colorize
{
    private static string[][] arr_text = new string[5][]; // jagged array 5 rows, uninitialized columns
    private static string[] linelist = {"public class Test {",
                             "public static void main(String[] args) {",
                             "System.out.println(\"Hello, World!\");",
                             "}",
                             "}"};
    private static Dictionary<string, string> keywordColors = new Dictionary<string, string>
        {
            {"public", "#569CD6"},
            {"class", "#569CD6"},
            {"Test", "#4EC9B0"},
            {"static", "#569CD6"},
            {"void", "#4EC9B0"},
            {"main", "#DCDCAA"},
            {"String", "#4EC9B0"},
            {"(", "#569CD6"},
            {")", "#569CD6"},
            {"[", "#569CD6"},
            {"]", "#569CD6"},
            {"args", "#9CDCFE"},
            {"System", "#4EC9B0"},
            {"out", "#4FC1FF"},
            {"println", "#DCDCAA"},
            {"\"Hello,", "#9CDCFE"},
            {"World!\"", "#9CDCFE"},
            {"{", "#569CD6"},
            {"}", "#569CD6"}
        };

    static void Main(string[] args)
    {
        //split text into jagged array
        splitText();

        //colorize text
        startColorizeText();

        //show text from jagged array
        showText();
    }

    //split text into jagged array
    static void splitText(){
        for (int i = 0; i < linelist.Length; i++)
        {
            string[] parts = Regex.Split(linelist[i], "(?<=\\ )|(?<=\\.)|(?=\\.)|(?<=\\()|(?=\\()|(?=\\))|(?=\\[)|(?=\\])|(?=\\;)");
            arr_text[i] = new string[parts.Length];
            for (int j = 0; j < parts.Length; j++)
            {
                arr_text[i][j] = parts[j];
            }
        }
    }

    //Loop through jagged array and colorize text
    static void startColorizeText()
    {
        for (int i = 0; i < arr_text.Length; i++)
        {
            for (int j = 0; j < arr_text[i].Length; j++)
            {
                arr_text[i][j] = ColorizeText(arr_text[i][j]);
            }
        }
    }

    //Show text in console
    static void showText()
    {
        for (int i = 0; i < arr_text.Length; i++)
        {
            for (int j = 0; j < arr_text[i].Length; j++)
            {
                Console.Write(arr_text[i][j]);
            }
            Console.WriteLine();
        }
    }

    //colorize text
    static string ColorizeText(string text)
    {
        string input = text;
        input = input.TrimEnd();

        string? color;
        if (keywordColors.TryGetValue(input, out color))
        {
            return "<color=" + color + ">" + text + "</color>";
        }
        else
        {
            return "<color=#white>" + text + "</color>";
        }
    }
}
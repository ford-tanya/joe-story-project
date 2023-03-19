using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Typer : MonoBehaviour
{
    // UI text to show code
    public Text playerInputLine0 = null;
    public Text playerInputLine1 = null;
    public Text playerInputLine2 = null;
    public Text playerInputLine3 = null;
    public Text playerInputLine4 = null;

    // UI text to show score
    public Text completedScore;

    private string remainingWord = string.Empty;    // Keep track of the remaining words
    private string colorWord = string.Empty;        // Keep track of the typed words
    private string showWord = string.Empty;         // Words for set text in UI text
    private string colorizeText = string.Empty;
    private static string[] parts = { };
    private static string[] linelist = {"public class Test {",
                             "public static void main(String[] args) {",
                             "System.out.println(\"Hello, World!\");",
                             "}",
                             "}"};

    // Dictionary to map keywords to their respective colors
    private static Dictionary<string, string> keywordColors = new Dictionary<string, string>{
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

    private int currentLine = 0;    // Keep track of the current line being typed
    private int score = 0;          // Keep track of the score

    public AudioClip type_sound;
    private AudioSource audioSource;


    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        remainingWord = linelist[0];
        showWord = linelist[0];
        SetCurrentWord();
    }

    // Sets the current word to be typed
    private void SetCurrentWord()
    {
        SetText(showWord);
    }

    // This method sets the text of the current line based on the value of the 'currentLine' variable
    private void SetText(string word)
    {
        switch (currentLine)
        {
            case 0: playerInputLine0.text = word; break;
            case 1: playerInputLine1.text = word; break;
            case 2: playerInputLine2.text = word; break;
            case 3: playerInputLine3.text = word; break;
            case 4: playerInputLine4.text = word; break;
            default: Debug.Log("Error out of case: SetRemainingWord"); break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
    }

    // This method checks for keyboard input.
    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string keyPressed = Input.inputString;

            //Only one key per frame
            if (keyPressed.Length == 1)
                EnterLetter(keyPressed);
        }
    }

    // This method is called when the user types a letter and checks if the letter is correct.
    private void EnterLetter(string typedLetter)
    {
        if (IsCorrectLetter(typedLetter))
        {
            RemoveLetter(typedLetter);
            audioSource.PlayOneShot(type_sound);

            // Check if the user has completed the word
            if (IsWordComplete())
            {
                // If the user has not completed the last line of text, move to the next line
                if (currentLine != 4) {
                    currentLine++;
                    remainingWord = linelist[currentLine];
                    showWord = linelist[currentLine];
                    colorWord = string.Empty;
                    SetCurrentWord();
                }
                // If the user has completed the last line of text, start a new game
                else
                {
                    score++;
                    completedScore.text = "score: " + score.ToString() + " (for debug)";
                    currentLine = 0;
                    remainingWord = linelist[currentLine];
                    showWord = linelist[currentLine];
                    colorWord = string.Empty;
                    NewGame();
                    SetCurrentWord();
                }
                
            }
        }
    }

    // This method checks if the user typed the correct letter.
    private bool IsCorrectLetter(string letter)
    {
        return remainingWord.IndexOf(letter) == 0;
    }

    private void RemoveLetter(string typedLetter)
    {
        colorWord += remainingWord[0];
        string[] textlist = splitText(colorWord);
        string colorize = startColorizeText(textlist);

        Debug.Log("Color word: " + colorWord);
        Debug.Log("colorize: " + colorize);
        Debug.Log("colorizeText: " + colorizeText);

        string newString = remainingWord.Remove(0, 1);
        remainingWord = newString;
        Debug.Log("Remaining word: " + remainingWord);

        showWord = colorize + remainingWord;
        Debug.Log("text: " + colorize + remainingWord);
        
        
        Debug.Log("showWord var:" + showWord);
        SetText(showWord);
    }

    private string[] splitText(string textToSplit)
    {
        parts = Regex.Split(textToSplit, "(?<=\\ )|(?<=\\.)|(?=\\.)|(?<=\\()|(?=\\()|(?=\\))|(?=\\[)|(?=\\])|(?=\\;)");
        return parts;
    }

    private string startColorizeText(string[] textlist)
    {
        string linetextcolor = string.Empty;
        for (int i = 0; i < parts.Length; i++)
        {
            //colorize text
            string colorizedText = ColorizeText(parts[i]);
            linetextcolor += colorizedText;
        }

        return linetextcolor;
    }

    static string ColorizeText(string text)
    {
        string input = text;
        input = input.TrimEnd();

        string color;
        if (keywordColors.TryGetValue(input, out color))
        {
            return "<color=" + color + ">" + text + "</color>";
        }
        else
        {
            return "<color=#white>" + text + "</color>";
        }
    }

    private bool IsWordComplete()
    {
        return remainingWord.Length == 0;
    }

    private void NewGame()
    {
        playerInputLine0.text = linelist[0];
        playerInputLine1.text = linelist[1];
        playerInputLine2.text = linelist[2];
        playerInputLine3.text = linelist[3];
        playerInputLine4.text = linelist[4];
    }
}

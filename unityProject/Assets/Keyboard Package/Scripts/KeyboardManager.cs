using System;
using UnityEngine;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] TextMeshProUGUI printBox;
    [SerializeField] private string correctWord;

    private void Start()
    {
        Instance = this;
        printBox.text = "";
        textBox.text = "";
        
    }

    private void OnEnable()
    {
        Client.Instance.PuzzleSolved = false;
    }

    public void DeleteLetter()
    {
        if(textBox.text.Length != 0) {
            textBox.text = textBox.text.Remove(textBox.text.Length - 1, 1);
        }
    }

    public void AddLetter(string letter)
    {
        textBox.text = textBox.text + letter;
    }

    public void SubmitWord()
    {
        printBox.text = textBox.text;
        textBox.text = "";

        CheckInput(printBox.text);
        // Debug.Log("Text submitted successfully!");
    }

    public void CheckInput(string input)
    {
        if (input == correctWord)
        {
            Debug.Log("word is correct!");
            Client.Instance.PuzzleSolved = true;
            Client.Instance.LockCorrect = true;
        }
        else
        {
            Debug.Log("word is incorrect.");
        }
    }
}

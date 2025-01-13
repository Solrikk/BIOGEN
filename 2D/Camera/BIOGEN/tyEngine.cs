
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

public class TerminalController : MonoBehaviour
{
    public TMP_Text outputText;
    public TMP_InputField inputField;

    [SerializeField]
    private int maxLines = 13;

    private const string prompt = "PS> ";
    private bool isProcessing = false;
    private List<string> commandHistory = new List<string>();
    private int historyIndex = -1;
    private DateTime startTime;
    private List<string> welcomeMessages;
    private int currentMessageIndex = 0;
    private string currentMessage = "";
    private float typingSpeed = 0.05f;
    private float nextCharTime = 0f;
    private bool isTypingMessage = false;
    private bool isAuthenticated = false;
    private string correctLogin = "admin";
    private string correctPassword = "biogen";
    private bool isEnteringLogin = false;
    private bool isEnteringPassword = false;

    void Start()
    {
        startTime = DateTime.Now;
        InitializeTerminal();
        inputField.interactable = true;
        RequestLogin();
    }

    private void RequestLogin()
    {
        AppendOutput("Enter login:");
        isEnteringLogin = true;
        isEnteringPassword = false;
    }

    private void InitializeTerminal()
    {
        if (inputField != null)
        {
            inputField.text = prompt;
            inputField.interactable = false;
            inputField.onSelect.AddListener(OnInputFieldSelected);
            inputField.onSubmit.AddListener(ProcessCommand);
            inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            inputField.ActivateInputField();
            outputText.enableWordWrapping = true;
            outputText.overflowMode = TextOverflowModes.Overflow;
            welcomeMessages = new List<string>
            {
                "=== CORPORATION BIOGEN ===",
                "ATTENTION: HIGHEST ACCESS LEVEL",
                "Operator, you have been appointed to the position of civil activity inspector.",
                "Under conditions of terrorist threat from 'Red Crystal'",
                "and growing social instability, your role is critically important.",
                "BIOGEN takes control of key infrastructure objects.",
                "Remember: we are the last bastion of order in a dying state.",
                "Your lifetime contract is activated. Welcome to the system.",
                "Enter 'help' to get a list of available commands.",
                "======================================"
            };
            isTypingMessage = true;
        }
        else
        {
            Debug.LogError("InputField not assigned in inspector.");
        }

        if (outputText == null)
        {
            Debug.LogError("OutputText not assigned in inspector.");
        }
    }

    void OnInputFieldSelected(string text)
    {
        inputField.caretPosition = inputField.text.Length;
    }

    void OnInputFieldValueChanged(string text)
    {
        if (!text.StartsWith(prompt))
        {
            inputField.text = prompt;
            inputField.caretPosition = inputField.text.Length;
        }
    }

    void ProcessCommand(string userInput)
    {
        if (isProcessing) return;

        try
        {
            isProcessing = true;
            string input = userInput.Substring(prompt.Length).Trim();

            string command = input.ToLower();
            string[] args = command.Split(' ');

            if (string.IsNullOrEmpty(command)) return;

            if (command == "help")
            {
                ShowHelp();
                return;
            }

            if (isEnteringLogin)
            {
                if (command == correctLogin.ToLower())
                {
                    AppendOutput("Enter password:");
                    isEnteringLogin = false;
                    isEnteringPassword = true;
                }
                else
                {
                    AppendOutput("Invalid login. Try again:");
                }
                return;
            }

            if (isEnteringPassword)
            {
                if (input == correctPassword)
                {
                    isAuthenticated = true;
                    isEnteringPassword = false;
                    AppendOutput("Access granted. Welcome to the system.");
                }
                else
                {
                    AppendOutput("Invalid password. Access denied.");
                    RequestLogin();
                }
                return;
            }

            if (!isAuthenticated)
            {
                AppendOutput("Please log in to the system.");
                return;
            }

            AppendOutput(prompt + command);
            commandHistory.Add(command);
            historyIndex = commandHistory.Count;

            switch (args[0])
            {
                case "clear":
                    ClearOutput();
                    break;
                case "hello":
                    AppendOutput("Hello! This is an enhanced terminal in Unity.");
                    break;
                case "time":
                    ShowTime();
                    break;
                case "uptime":
                    ShowUptime();
                    break;
                case "echo":
                    Echo(args);
                    break;
                case "history":
                    ShowHistory();
                    break;
                default:
                    AppendOutput($"Error: command '{args[0]}' not found. Enter 'help' for list of commands.");
                    break;
            }
        }
        catch (Exception ex)
        {
            AppendOutput($"Command execution error: {ex.Message}");
        }
        finally
        {
            inputField.text = prompt;
            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
            isProcessing = false;
        }
    }

    private void ShowHelp()
    {
        AppendOutput("Available commands:");
        AppendOutput("  help    - show list of commands");
        AppendOutput("  clear   - clear screen");
        AppendOutput("  hello   - greeting");
        AppendOutput("  time    - show current time");
        AppendOutput("  uptime  - show terminal uptime");
        AppendOutput("  echo    - output text");
        AppendOutput("  history - show command history");
        AppendOutput("Login credentials:");
        AppendOutput("  Login: <color=yellow>admin</color>");
        AppendOutput("  Password: <color=yellow>biogen</color>");
    }

    private void ShowTime()
    {
        AppendOutput($"Current time: {DateTime.Now:HH:mm:ss}");
    }

    private void ShowUptime()
    {
        TimeSpan uptime = DateTime.Now - startTime;
        AppendOutput($"Uptime: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s");
    }

    private void Echo(string[] args)
    {
        if (args.Length > 1)
        {
            AppendOutput(string.Join(" ", args, 1, args.Length - 1));
        }
        else
        {
            AppendOutput("Usage: echo <text>");
        }
    }

    private void ShowHistory()
    {
        if (commandHistory.Count == 0)
        {
            AppendOutput("Command history is empty");
            return;
        }

        AppendOutput("Command history:");
        for (int i = 0; i < commandHistory.Count; i++)
        {
            AppendOutput($"{i + 1}. {commandHistory[i]}");
        }
    }

    void AppendOutput(string message)
    {
        string[] lines = outputText.text.Split('\n');
        if (lines.Length >= maxLines)
        {
            lines = lines.Skip(lines.Length - maxLines + 1).ToArray();
            outputText.text = string.Join("\n", lines);
        }
        outputText.text += message + "\n";
    }

    void ClearOutput()
    {
        outputText.text = "";
    }

    void Update()
    {
        if (inputField.isFocused)
        {
            HandleKeyboardInput();
        }

        if (isTypingMessage && Time.time >= nextCharTime)
        {
            if (currentMessageIndex < welcomeMessages.Count)
            {
                if (currentMessage.Length < welcomeMessages[currentMessageIndex].Length)
                {
                    currentMessage += welcomeMessages[currentMessageIndex][currentMessage.Length];
                    outputText.text = string.Join("\n", welcomeMessages.Take(currentMessageIndex))
                        + (currentMessageIndex > 0 ? "\n" : "")
                        + currentMessage;
                    nextCharTime = Time.time + typingSpeed;
                }
                else
                {
                    currentMessage = "";
                    currentMessageIndex++;
                }
            }
            else
            {
                isTypingMessage = false;
                inputField.interactable = true;
            }
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (commandHistory.Count > 0 && historyIndex > 0)
            {
                historyIndex--;
                SetInputFieldText(commandHistory[historyIndex]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (commandHistory.Count > 0 && historyIndex < commandHistory.Count - 1)
            {
                historyIndex++;
                SetInputFieldText(commandHistory[historyIndex]);
            }
            else
            {
                historyIndex = commandHistory.Count;
                SetInputFieldText("");
            }
        }
    }

    void SetInputFieldText(string command)
    {
        inputField.text = prompt + command;
        inputField.caretPosition = inputField.text.Length;
    }
}

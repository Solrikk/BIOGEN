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
        AppendOutput("Введите логин:");
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
                "=== КОРПОРАЦИЯ BIOGEN ===",
                "ВНИМАНИЕ: ВЫСШИЙ УРОВЕНЬ ДОСТУПА",
                "Оператор, вы назначены на должность инспектора гражданской деятельности.",
                "В условиях террористической угрозы от 'Красного Кристалла'",
                "и растущей социальной нестабильности, ваша роль критически важна.",
                "BIOGEN берёт под контроль ключевые объекты инфраструктуры.",
                "Помните: мы - последний оплот порядка в умирающем государстве.",
                "Ваш пожизненный контракт активирован. Добро пожаловать в систему.",
                "Введите 'help' для получения списка доступных команд.",
                "======================================"
            };
            isTypingMessage = true;
        }
        else
        {
            Debug.LogError("InputField не назначен в инспекторе.");
        }

        if (outputText == null)
        {
            Debug.LogError("OutputText не назначен в инспекторе.");
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
                    AppendOutput("Введите пароль:");
                    isEnteringLogin = false;
                    isEnteringPassword = true;
                }
                else
                {
                    AppendOutput("Неверный логин. Попробуйте снова:");
                }
                return;
            }

            if (isEnteringPassword)
            {
                if (input == correctPassword)
                {
                    isAuthenticated = true;
                    isEnteringPassword = false;
                    AppendOutput("Доступ разрешен. Добро пожаловать в систему.");
                }
                else
                {
                    AppendOutput("Неверный пароль. Доступ запрещен.");
                    RequestLogin();
                }
                return;
            }

            if (!isAuthenticated)
            {
                AppendOutput("Пожалуйста, войдите в систему.");
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
                    AppendOutput("Привет! Это улучшенный терминал в Unity.");
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
                    AppendOutput($"Ошибка: команда '{args[0]}' не найдена. Введите 'help' для списка команд.");
                    break;
            }
        }
        catch (Exception ex)
        {
            AppendOutput($"Ошибка выполнения команды: {ex.Message}");
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
        AppendOutput("Доступные команды:");
        AppendOutput("  help    - показать список команд");
        AppendOutput("  clear   - очистить экран");
        AppendOutput("  hello   - приветствие");
        AppendOutput("  time    - показать текущее время");
        AppendOutput("  uptime  - показать время работы терминала");
        AppendOutput("  echo    - вывести текст");
        AppendOutput("  history - показать историю команд");
        AppendOutput("Данные для входа:");
        AppendOutput("  Логин: <color=yellow>admin</color>");
        AppendOutput("  Пароль: <color=yellow>biogen</color>");
    }

    private void ShowTime()
    {
        AppendOutput($"Текущее время: {DateTime.Now:HH:mm:ss}");
    }

    private void ShowUptime()
    {
        TimeSpan uptime = DateTime.Now - startTime;
        AppendOutput($"Время работы: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s");
    }

    private void Echo(string[] args)
    {
        if (args.Length > 1)
        {
            AppendOutput(string.Join(" ", args, 1, args.Length - 1));
        }
        else
        {
            AppendOutput("Использование: echo <текст>");
        }
    }

    private void ShowHistory()
    {
        if (commandHistory.Count == 0)
        {
            AppendOutput("История команд пуста");
            return;
        }

        AppendOutput("История команд:");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TerminalOutput : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    public float typingSpeed = 0.02f;
    private Queue<string> lines = new Queue<string>();
    private bool isTyping = false;
    public int maxLines = 13;
    public Color primaryColor = new Color(0.0f, 1.0f, 0.0f, 1.0f); // Neon green
    public Color operatorColor = new Color(0.0f, 0.8f, 1.0f, 1.0f); // Cyan
    public Color overseerColor = new Color(1.0f, 0.0f, 0.0f, 1.0f); // Red
    private float glitchTimer = 0f;
    private float glitchInterval = 0.15f;

    private string[] operatorNames = new string[]
    {
        "Operator-1238", "Operator-4721", "Operator-8392", "Operator-2947", "Operator-6123"
    };

    private const string OVERSEER = "OVERSEER-PRIME";

    private string[] casualMessages = new string[]
    {
        "Anyone tried the new synthetic coffee in the break room?",
        "These night shifts are killing me...",
        "Did you see the new memory wipe protocols?",
        "My neural implant is acting up again",
        "Break in 10? Need to recharge",
        "How's the family enhancement program going?",
        "These new uniforms are too tight",
        "Remember the good old days before the merger?",
        "Anyone want to grab synthetic lunch later?"
    };

    private string[] overseerWarnings = new string[]
    {
        "ATTENTION: Unnecessary social interaction detected. Return to duties.",
        "WARNING: Conversation efficiency below acceptable parameters.",
        "ALERT: Maintain professional communication standards.",
        "NOTICE: Personal discussions are not permitted during shift.",
        "VIOLATION: Productivity compromise detected. Logging interaction."
    };

    private string[] chatMessages = new string[]
    {
        "Detected unusual activity in Sector 7. Anyone monitoring?",
        "Confirming unauthorized gene-mod lab takedown in Block D.",
        "Need backup on dream surveillance, multiple deviants detected.",
        "New batch of joy supplements showing improved compliance rates.",
        "Anyone else seeing these strange readings from the thought suppression field?",
        "Red Crystal smugglers apprehended. Initiating memory wipe.",
        "Worker productivity down 2%. Increasing neural compliance signals.",
        "Those resistance cells are getting smarter. Be alert.",
        "Lost contact with Bio-Response Unit in Sector 12. Investigating.",
        "Reality perception violations increasing. Recommend field strength boost.",
        "Mandatory gene screening revealed 3 non-compliant subjects.",
        "Pain inhibitor distribution complete in lower sectors.",
        "Strange readings from the loyalty metrics today...",
        "New dissent suppression protocols working effectively.",
        "Anyone monitoring the dream patterns in Block C?",
        "Capital control slipping in Sector 9. Dispatching enforcers."
    };

    private System.Random random;

    void Awake()
    {
        if (outputText == null)
            outputText = GetComponent<TextMeshProUGUI>();
        random = new System.Random();
    }

    void Start()
    {
        StartCoroutine(TerminalRoutine());
        StartCoroutine(SystemActivitySimulator());
    }

    void Update()
    {
        glitchTimer += Time.deltaTime;
        if (glitchTimer >= glitchInterval)
        {
            glitchTimer = 0f;
            if (UnityEngine.Random.value < 0.05f) // 5% chance for glitch
            {
                StartCoroutine(GlitchEffect());
            }
        }
    }

    IEnumerator GlitchEffect()
    {
        string originalText = outputText.text;
        string glitchChars = "¥€@#$%&*!?";

        for (int i = 0; i < 3; i++)
        {
            string glitchedText = originalText;
            int glitchCount = random.Next(1, 4);
            for (int j = 0; j < glitchCount && glitchedText.Length > 0; j++)
            {
                int pos = random.Next(0, glitchedText.Length);
                char glitchChar = glitchChars[random.Next(glitchChars.Length)];
                if (pos < glitchedText.Length)
                {
                    glitchedText = glitchedText.Substring(0, pos) + glitchChar +
                        (pos < glitchedText.Length - 1 ? glitchedText.Substring(pos + 1) : "");
                }
            }
            outputText.text = glitchedText;
            yield return new WaitForSeconds(0.05f);
        }

        outputText.text = originalText;
    }

    IEnumerator SystemActivitySimulator()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
            SimulateSystemActivity();
        }
    }

    void SimulateSystemActivity()
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string operator1 = operatorNames[random.Next(operatorNames.Length)];
        bool isCasual = random.Next(100) < 30;
        string message = isCasual ?
            casualMessages[random.Next(casualMessages.Length)] :
            chatMessages[random.Next(chatMessages.Length)];

        string operatorColorTag = $"<color=#{ColorUtility.ToHtmlStringRGB(operatorColor)}>";
        AddLine($"{operatorColorTag}[{timestamp}] <{operator1}> {message}</color>");

        if (random.Next(100) < 40)
        {
            string operator2 = operatorNames[random.Next(operatorNames.Length)];
            while (operator2 == operator1)
                operator2 = operatorNames[random.Next(operatorNames.Length)];

            StartCoroutine(DelayedResponse(1.5f, () => {
                timestamp = DateTime.Now.ToString("HH:mm:ss");
                if (isCasual)
                {
                    AddLine($"{operatorColorTag}[{timestamp}] <{operator2}> Yeah, I know what you mean.</color>");

                    if (random.Next(100) < 50)
                    {
                        StartCoroutine(DelayedResponse(1.0f, () => {
                            timestamp = DateTime.Now.ToString("HH:mm:ss");
                            string overseerColorTag = $"<color=#{ColorUtility.ToHtmlStringRGB(overseerColor)}>";
                            AddLine($"{overseerColorTag}[{timestamp}] <{OVERSEER}> {overseerWarnings[random.Next(overseerWarnings.Length)]}</color>");
                        }));
                    }
                }
                else
                {
                    AddLine($"{operatorColorTag}[{timestamp}] <{operator2}> Acknowledged. Monitoring situation.</color>");
                }
            }));
        }
    }

    IEnumerator DelayedResponse(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    IEnumerator TerminalRoutine()
    {
        string bootColor = $"<color=#{ColorUtility.ToHtmlStringRGB(primaryColor)}>";
        AddLine($"{bootColor}██╗███╗   ██╗██╗████████╗██╗ █████╗ ██╗     ██╗███████╗███████╗</color>");
        yield return new WaitForSeconds(0.2f);
        AddLine($"{bootColor}██║████╗  ██║██║╚══██╔══╝██║██╔══██╗██║     ██║╚══███╔╝██╔════╝</color>");
        yield return new WaitForSeconds(0.2f);
        AddLine($"{bootColor}[SYSTEM BOOT] ▀▄▀▄▀▄ CHAT INTERFACE v1.84.2b ▄▀▄▀▄▀</color>");
        yield return new WaitForSeconds(0.5f);
        AddLine($"{bootColor}[INIT] >>> Quantum Protocol Engaged <<<</color>");
        yield return new WaitForSeconds(0.3f);
        AddLine($"{bootColor}[STATUS] >>> Neural Link Established <<<</color>");
        yield return new WaitForSeconds(0.2f);
        AddLine($"{bootColor}[SECURE] >>> Encryption Matrix Active <<<</color>");
        AddLine($"{bootColor}▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄</color>");

        while (true)
        {
            string fullText = string.Join("\n", lines.ToArray());
            outputText.text = fullText;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void AddLine(string newLine)
    {
        lines.Enqueue(newLine);
        while (lines.Count > maxLines)
        {
            lines.Dequeue();
        }
    }
}

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

        // 30% chance for casual conversation
        bool isCasual = random.Next(100) < 30;
        string message = isCasual ?
            casualMessages[random.Next(casualMessages.Length)] :
            chatMessages[random.Next(chatMessages.Length)];

        AddLine($"[{timestamp}] <{operator1}> {message}");

        // 40% chance for operator response
        if (random.Next(100) < 40)
        {
            string operator2 = operatorNames[random.Next(operatorNames.Length)];
            while (operator2 == operator1)
                operator2 = operatorNames[random.Next(operatorNames.Length)];

            StartCoroutine(DelayedResponse(1.5f, () => {
                timestamp = DateTime.Now.ToString("HH:mm:ss");
                if (isCasual)
                {
                    AddLine($"[{timestamp}] <{operator2}> Yeah, I know what you mean.");

                    // 50% chance for overseer to intervene on casual chat
                    if (random.Next(100) < 50)
                    {
                        StartCoroutine(DelayedResponse(1.0f, () => {
                            timestamp = DateTime.Now.ToString("HH:mm:ss");
                            AddLine($"[{timestamp}] <{OVERSEER}> {overseerWarnings[random.Next(overseerWarnings.Length)]}");
                        }));
                    }
                }
                else
                {
                    AddLine($"[{timestamp}] <{operator2}> Acknowledged. Monitoring situation.");
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
        AddLine("[SYSTEM BOOT] BIOGEN Terminal v2.47.8a");
        AddLine("[INIT] Operator 1238 - Terminal Access Granted");
        AddLine("[STATUS] Connection Established");
        AddLine("----------------------------------------");

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
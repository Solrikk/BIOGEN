
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SystemTerminalOutput : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    public float typingSpeed = 0.02f;
    private Queue<string> lines = new Queue<string>();
    private bool isTyping = false;
    public int maxLines = 13;
    public Color primaryColor = new Color(0.0f, 1.0f, 0.0f, 1.0f); // Neon green
    public Color warningColor = new Color(1.0f, 0.5f, 0.0f, 1.0f); // Orange
    public Color alertColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);   // Red
    private float glitchTimer = 0f;
    private float glitchInterval = 0.1f;

    private string[] systemActions = new string[]
    {
        "ACCESS: User.1238 maintaining connection...",
        "ALERT: Subject #4721 detained - Red Crystal possession",
        "DEPLOY: Enhanced Bio-Response Unit dispatched to Sector 8",
        "INTEL: Illegal medicine trade detected in Lower Districts",
        "STATUS: Capital control maintained at 87% efficiency",
        "WARNING: Resistance cell activity in Industrial Zone",
        "ENFORCE: Mandatory bio-scanning checkpoints activated",
        "UPDATE: New genetic compliance laws in effect",
        "ALERT: Unauthorized genetic modification lab discovered",
        "DEPLOY: Memory wipe protocol initiated in Block C",
        "STATUS: Thought suppression field operating normally",
        "SCAN: Detecting unregistered bio-signatures",
        "ALERT: Black market augmentation surgery prevented",
        "UPDATE: Social compliance score adjustments complete",
        "WARNING: Unauthorized assembly in Worker District",
        "ENFORCE: Curfew violations - deploying enforcement drones",
        "ALERT: Dissident propaganda detected in Sector 12",
        "DEPLOY: Loyalty enforcement squads to residential Block F",
        "UPDATE: Mass genetic purification scheduled for Zone 7",
        "SCAN: Illegal emotional amplification devices detected",
        "WARNING: Unregistered births in worker compounds",
        "DEPLOY: Neural compliance agents to education centers",
        "ALERT: Anti-corporate thoughts detected in dream monitoring",
        "STATUS: Behavioral modification chambers at full capacity",
        "ENFORCE: Mandatory joy supplements distribution",
        "UPDATE: Memory alteration success rate: 94.7%",
        "ALERT: Rogue AI sympathizers identified in tech sector",
        "DEPLOY: Organ harvest collection in progress - Block D",
        "WARNING: Unauthorized reality perception detected",
        "STATUS: Pain inhibitor production exceeded quotas"
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
        string randomAction = systemActions[random.Next(systemActions.Length)];
        string colorTag = randomAction.StartsWith("ALERT") ? $"<color=#{ColorUtility.ToHtmlStringRGB(alertColor)}>" :
                         randomAction.StartsWith("WARNING") ? $"<color=#{ColorUtility.ToHtmlStringRGB(warningColor)}>" :
                         $"<color=#{ColorUtility.ToHtmlStringRGB(primaryColor)}>";

        AddLine($"{colorTag}[{timestamp}] {randomAction}</color>");

        if (random.Next(100) < 20) // 20% chance for system metrics
        {
            string metrics = $"[{timestamp}] METRICS: Power usage: {random.Next(75, 98)}% | Neural load: {random.Next(40, 95)}%";
            AddLine($"<color=#{ColorUtility.ToHtmlStringRGB(primaryColor)}>{metrics}</color>");
        }
    }

    void Update()
    {
        glitchTimer += Time.deltaTime;
        if (glitchTimer >= glitchInterval)
        {
            glitchTimer = 0f;
            if (random.Next(100) < 5) // 5% chance for glitch effect
            {
                StartCoroutine(GlitchEffect());
            }
        }
    }

    IEnumerator GlitchEffect()
    {
        string originalText = outputText.text;
        string glitchChars = "@#$%&*!?";

        for (int i = 0; i < 3; i++)
        {
            string glitchedText = originalText;
            int glitchCount = random.Next(1, 4);
            for (int j = 0; j < glitchCount; j++)
            {
                if (glitchedText.Length > 0)
                {
                    int pos = random.Next(glitchedText.Length);
                    char glitchChar = glitchChars[random.Next(glitchChars.Length)];
                    if (pos < glitchedText.Length - 1)
                    {
                        glitchedText = glitchedText.Substring(0, pos) + glitchChar + glitchedText.Substring(pos + 1);
                    }
                }
            }
            outputText.text = glitchedText;
            yield return new WaitForSeconds(0.05f);
        }

        outputText.text = originalText;
    }

    IEnumerator TerminalRoutine()
    {
        string bootColor = $"<color=#{ColorUtility.ToHtmlStringRGB(primaryColor)}>";
        AddLine($"{bootColor}[SYSTEM BOOT] ██████████ BIOGEN Terminal v2.47.8a ██████████</color>");
        yield return new WaitForSeconds(0.5f);
        AddLine($"{bootColor}[INIT] >>> Operator 1238 - Terminal Access Granted <<<</color>");
        yield return new WaitForSeconds(0.3f);
        AddLine($"{bootColor}[STATUS] ### Connection Established ###</color>");
        AddLine($"{bootColor}██████████████████████████████████████████████████</color>");

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

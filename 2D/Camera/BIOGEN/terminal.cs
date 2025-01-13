
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
        AddLine($"[{timestamp}] {randomAction}");

        if (random.Next(100) < 20) // 20% chance for system metrics
        {
            AddLine($"[{timestamp}] METRICS: Power usage: {random.Next(75, 98)}%");
            AddLine($"[{timestamp}] METRICS: Neural load: {random.Next(40, 95)}%");
        }
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

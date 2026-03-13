using UnityEngine;
using NativeWebSocket;
using System;
using UnityEngine.SceneManagement;

public class WebSocketClientExample : MonoBehaviour
{
    private WebSocket websocket;
    public string serverIP = "10.204.0.67"; 
    public string serverPort = "8081"; 

    [Range(0, 255)]
    public int ledIntensity = 0;

    // This "Flag" is the secret to making it work!
    private bool shouldRestart = false;

    async void Start()
    {
        websocket = new WebSocket("ws://" + serverIP + ":" + serverPort);

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            IncomingMessageParser(message);
        };

        await websocket.Connect();
    }

   void Update()
{
    // Added a safety check: "&& websocket != null"
    #if !UNITY_WEBGL || UNITY_EDITOR 
        if (websocket != null) 
        {
            websocket.DispatchMessageQueue();
        }
    #endif

    // The rest remains the same
    if (shouldRestart)
    {
        shouldRestart = false;
        Debug.Log("RESTARTING SCENE NOW...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
    public void IncomingMessageParser(string msg)
    {
        if (!msg.Contains(":")) return;
        string value = msg.Substring(msg.IndexOf(":") + 1).Trim();

        if (msg.Contains("button") && value == "1") 
        {
            Debug.Log("ESP32 Button Pressed signal received!");
            shouldRestart = true; // Tell the Update loop to restart next frame
        }
    }

    // --- These methods keep your Editor Buttons working ---
    public async void SendLedON() { await SendIntensity(255); }
    public async void SendLedOFF() { await SendIntensity(0); }
    public async void SendLedIntensity() { await SendIntensity(ledIntensity); }
    public async void SendHello() { if(websocket.State == WebSocketState.Open) await websocket.SendText("Hello"); }

    private async System.Threading.Tasks.Task SendIntensity(int val)
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
            await websocket.SendText("LED_INTENSITY:" + val);
    }

    async void OnDestroy()
    {
        if (websocket != null) await websocket.Close();
    }
}

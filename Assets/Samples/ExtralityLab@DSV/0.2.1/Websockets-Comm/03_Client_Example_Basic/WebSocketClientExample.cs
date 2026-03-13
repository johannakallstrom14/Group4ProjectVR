using UnityEngine;
using NativeWebSocket;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class WebSocketClientExample : MonoBehaviour
{
    private WebSocket websocket;

    public string serverIP = "10.204.0.59";
    public string serverPort = "8081";

    [Range(0, 255)]
    public int ledIntensity = 0;

    private bool shouldRestart = false;

    async void Start()
    {
        websocket = new WebSocket("ws://" + serverIP + ":" + serverPort + "/");

        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connected!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Received message: " + message);
            IncomingMessageParser(message);
        };

        websocket.OnError += (error) =>
        {
            Debug.LogError("WebSocket error: " + error);
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket closed with code: " + code);
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
#endif

        if (shouldRestart)
        {
            shouldRestart = false;
            Debug.Log("RESTARTING SCENE NOW...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void IncomingMessageParser(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg)) return;

        msg = msg.Trim();

        if (msg.Equals("RESTART", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("ESP32 restart signal received!");
            shouldRestart = true;
            return;
        }

        if (msg.Contains(":"))
        {
            string key = msg.Substring(0, msg.IndexOf(":")).Trim().ToLower();
            string value = msg.Substring(msg.IndexOf(":") + 1).Trim();

            if (key == "button" && value == "1")
            {
                Debug.Log("ESP32 Button Pressed signal received!");
                shouldRestart = true;
            }
        }
    }

    // Keep these so Unity inspector/buttons do not break
    public async void SendLedON()
    {
        await SendIntensity(255);
    }

    public async void SendLedOFF()
    {
        await SendIntensity(0);
    }

    public async void SendLedIntensity()
    {
        await SendIntensity(ledIntensity);
    }

    public async void SendHello()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
            await websocket.SendText("Hello");
    }

    private async System.Threading.Tasks.Task SendIntensity(int val)
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
            await websocket.SendText("LED_INTENSITY:" + val);
    }

    async void OnDestroy()
    {
        if (websocket != null)
            await websocket.Close();
    }
}
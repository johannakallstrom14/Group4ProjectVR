using UnityEngine;
using NativeWebSocket;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement; // Added for scene loading

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WebSocketClientExample : MonoBehaviour
{
    private WebSocket websocket;
    public string serverIP = "XXX.XXX.XXX.XXX"; 
    public string serverPort = "8081"; 

    [Range(0, 255)]
    public int ledIntensity = 0;

    async void Start()
    {
        websocket = new WebSocket("ws://" + serverIP + ":" + serverPort);

        websocket.OnOpen += async () =>
        {
            Debug.Log("Connected to WebSocket server");
            string UUID = SystemInfo.deviceUniqueIdentifier;
            await websocket.SendText("Device (Unity):" + SystemInfo.deviceName + " ... UUID: " + UUID);
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received: " + message);
            IncomingMessageParser(message);
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket closed");
        };

        await websocket.Connect();
    }

    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR 
            websocket.DispatchMessageQueue();
        #endif
    }

    async void OnDestroy()
    {
        if (websocket != null)
            await websocket.Close();
    }

    // --- BUTTON PARSING AND SCENE RELOAD ---
    public void IncomingMessageParser(string msg)
    {
        // Safety check to ensure the message contains a colon
        if (!msg.Contains(":")) return;

        string valueParsed = msg.Substring(msg.IndexOf(":") + 1).Trim();

        if (msg.Contains("button")) 
        {
            if (valueParsed == "1") 
            {
                Debug.Log("ESP32 Button Pressed - Restarting Scene...");
                RestartCurrentScene();
            }
        }
    }

    private void RestartCurrentScene()
    {
        // Get the currently active scene and reload it by name
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    // --- LED CONTROL METHODS ---
    public async void SendLedIntensity()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("LED_INTENSITY:" + ledIntensity);
        }
    }
    public async void SendLedON()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("LED_INTENSITY:255");
            Debug.Log("Sent: LED_INTENSITY:255");
        }
    }

    public async void SendLedOFF()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("LED_INTENSITY:0");
            Debug.Log("Sent: LED_INTENSITY:0");
        }
    }

    public async void SendHello()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("Hello from Unity");
            Debug.Log("Sent: Hello from Unity");
        }
    }
}


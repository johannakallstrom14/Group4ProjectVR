using UnityEngine;
using NativeWebSocket;
using UnityEngine.SceneManagement;
using System.Text;

public class RestartWebsocket : MonoBehaviour
{
    private WebSocket websocket;

    public string serverIP = "10.204.0.59";
    public string serverPort = "8081";

    private bool shouldRestart = false;

    async void Start()
    {
        websocket = new WebSocket($"ws://{serverIP}:{serverPort}/");

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Message received: " + message);

            if (message == "RESTART")
            {
                shouldRestart = true;
            }
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif

        if (shouldRestart)
        {
            shouldRestart = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    async void OnApplicationQuit()
    {
        if (websocket != null)
            await websocket.Close();
    }
}
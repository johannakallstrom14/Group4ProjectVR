using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class ESP32ResetManager : MonoBehaviour
{
    // These MUST be public to show up in the Inspector
    public string portName = "COM3"; 
    public int baudRate = 115200;
    SerialPort sp = new SerialPort("COM3", 115200); // Replace COM3 with your ESP32 port

    void Start() {
        try {
            sp.Open();
            sp.ReadTimeout = 1;
        } catch {
            Debug.LogError("Could not open Serial Port. Check your COM port number!");
        }
    }

    void Update() {
        if (sp.IsOpen) {
            try {
                string data = sp.ReadLine().Trim();
                if (data == "RESET_SCENE") {
                    RestartExperience();
                }
            } catch {
                // Timeout is normal when no data is sent
            }
        }
    }

    public void RestartExperience() {
        // This reloads the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
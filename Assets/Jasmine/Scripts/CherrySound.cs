using UnityEngine;
using UnityEngine.InputSystem;
public class CherrySound : MonoBehaviour
{

    public AudioSource audio;
    public AudioClip grabableSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayCherrySound()
    {
        audio.PlayOneShot(grabableSound);
    }
}
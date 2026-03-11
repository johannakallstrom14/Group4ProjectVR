using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour

{

    [Header("References")]

    public AudioSource bookAudio;

    public Transform playerTransform;

    public Transform targetLocation;

    public Material nextSkybox;



    private bool sequenceStarted = false;



    // Denna funktion anropas när man trycker på knappen

    public void StartTeleportSequence()

    {

        if (!sequenceStarted)

        {

            sequenceStarted = true;

            bookAudio.Play();

            StartCoroutine(WaitAndTeleport());

        }

    }



    IEnumerator WaitAndTeleport()

    {

        // Vänta tills ljudet spelat klart

        yield return new WaitWhile(() => bookAudio.isPlaying);



        // Utför teleportation

        TeleportPlayer();

        ChangeEnvironment();

    }



    void TeleportPlayer()

    {

        CharacterController cc = playerTransform.GetComponent<CharacterController>();

        if (cc != null) cc.enabled = false;

        playerTransform.position = targetLocation.position;

        playerTransform.rotation = targetLocation.rotation;

        if (cc != null) cc.enabled = true;

    }



    void ChangeEnvironment()

    {

        if (nextSkybox != null)

        {

            RenderSettings.skybox = nextSkybox;

            DynamicGI.UpdateEnvironment();

        }

    }

}
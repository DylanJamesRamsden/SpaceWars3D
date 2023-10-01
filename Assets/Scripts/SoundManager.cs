using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance = null;

    public AudioSource ShipDestroyedAudioSource;
    public AudioSource PickupRecievedAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
        {
            Instance = this;
            Debug.Log("Sound Manager registered");
        }
        else 
        {
            Destroy(this);
            return;
        }
    }

    public void PlayShipDestroyedSound()
    {
        ShipDestroyedAudioSource.Play();
    }

    public void PlayPickupRecievedSound()
    {
        PickupRecievedAudioSource.Play();
    }
}

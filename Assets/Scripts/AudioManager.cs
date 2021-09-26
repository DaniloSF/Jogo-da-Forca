using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource[] sounds;
    //Sound indexes
    const int RIGHT_ANSWER = 0;
    const int ERROR = 1;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string nome)
    {
        AudioSource audio = System.Array.Find(sounds, sound => sound.clip.name == nome);
        if (audio == null) return;
        audio.PlayOneShot(audio.clip);
    }
}

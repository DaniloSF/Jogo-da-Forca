using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource[] sounds;

    public void PlaySound(string nome)//Achar som na lista de efeito sonoros e tocar o audio uma vez
    {
        AudioSource audio = System.Array.Find(sounds, sound => sound.clip.name == nome);
        if (audio == null) return;
        audio.PlayOneShot(audio.clip);
    }
}

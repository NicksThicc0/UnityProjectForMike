using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundScript
{
    public Sound[] Sounds;
    public AudioSource Source;

    public void playSound(string soundName)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i].soundName == soundName)
            {
                //Volume
                Source.volume = Sounds[i].Volume;
                //Pitch
                Source.pitch = UnityEngine.Random.Range(1, Sounds[i].pitch);
                //Playing sound
                Source.PlayOneShot(Sounds[i].soundClip[UnityEngine.Random.Range(0, Sounds[i].soundClip.Length)]);
            }
        }
    }

}

[Serializable]
public class Sound
{
    public string soundName;
    public AudioClip[] soundClip;
    public float Volume = 1;
    public float pitch = 1;
}

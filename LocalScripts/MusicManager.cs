using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public playList[] allSongs;

    [Header("Stats")]
    public bool alreadyPlaying;
    public bool stopSong;

    [SerializeField] AudioSource musicSource;

    private void Start()
    {
        StartCoroutine(Play(allSongs[0]));
    }


    IEnumerator Play(playList currentPlayList)
    {
        if (!alreadyPlaying)
        {
            musicSource.clip = currentPlayList.Intro;
            musicSource.Play();
            yield return new WaitForSeconds(currentPlayList.Intro.length);
            musicSource.clip = currentPlayList.Loop;
            musicSource.Play();
        }
        if (!stopSong)
        {
            alreadyPlaying = true;
            yield return new WaitForSeconds(currentPlayList.Loop.length);
            StartCoroutine(Play(currentPlayList));
            Debug.Log("Looped");
        }
        else
        {
            musicSource.clip = currentPlayList.End;
            musicSource.Play();
            yield return new WaitForSeconds(currentPlayList.End.length);
            alreadyPlaying = false;
            stopSong = false;
        }
    }

}

[System.Serializable]
public class playList
{
    [SerializeField] string songName;

    public AudioClip Intro;
    public AudioClip Loop;
    public AudioClip End;
}

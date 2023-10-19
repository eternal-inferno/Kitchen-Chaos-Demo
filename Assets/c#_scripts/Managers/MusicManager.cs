using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource gameMusic;

    [SerializeField] private float volume = 0.3f;

    private void Awake()
    {
        Instance = this;
        gameMusic = GetComponent<AudioSource>();
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
            gameMusic.volume = volume;
        }
        gameMusic.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }
}

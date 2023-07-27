using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource mainJump;
    public AudioSource otherClick;
    public AudioSource popClick;
    public AudioSource slimeHop;
    public AudioSource slimSplat;
    public AudioSource slimJump;
    public AudioSource crumple;
    
    private void Awake() => DontDestroyOnLoad(gameObject);
}

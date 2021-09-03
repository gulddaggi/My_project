using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    static public BackgroundMusic instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip bgm;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        PlayBGM(bgm);
    }

    public void PlayBGM(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

}

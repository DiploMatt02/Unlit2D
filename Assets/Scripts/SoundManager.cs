using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //One shared SoundManager any script can reach
    public static SoundManager Instance;
    [SerializeField] private AudioSource source;

    void Awake()
    {
        //Register this as the shared SoundManager
        Instance = this;
    }

    //Play a sound without cutting off others; skips empty slots
    public void Play(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            source.PlayOneShot(clip, volume);
        }
    }
}

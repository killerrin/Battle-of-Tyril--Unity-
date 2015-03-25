using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
	public static AudioManager me;
	public AudioSource audioSource;

	public AudioClip MenuMusic;
	public AudioClip PlayMusic;
	public AudioClip PauseMusic;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = true;

		me = this;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PlayMenu()
	{
        if (audioSource.clip == MenuMusic) { return; }

        audioSource.clip = MenuMusic;
        Play();
    }
	public void PlayGame()
    {
        if (audioSource.clip == PlayMusic) { return; }

        audioSource.clip = PlayMusic;
        Play();
    }
	public void PlayPause()
	{
        if (audioSource.clip == PauseMusic) { return; }

        audioSource.clip = PauseMusic;
        Play();
    }

    public void Play() { audioSource.Play(); }
    public void Pause() { audioSource.Pause(); }
    public void Stop() { audioSource.Stop(); }
}

using DefaultNamespace;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private AudioVisualizer audioVisualizer;
    [SerializeField] private AudioPlayerList audioPlayerList;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject stopButton;
    [SerializeField] private GameObject rewindButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject forwardButton;

    private float maxVolume = 0.3f;
    private float currentVolume;

    private float lastClickTime;
    private const float DOUBLE_CLICK_TIME = .2f;
    private bool playing;
    private SongEntry currentSelected;
    private void OnEnable()
    {
        MessageHandler.ButtonClicked += UpdateButtonUI;
        MessageHandler.SongEntrySelected += GetSongEntry;
        MessageHandler.DoubleClick += ChangeSong;
    }
    
    private void OnDisable()
    {
        MessageHandler.ButtonClicked -= UpdateButtonUI;
        MessageHandler.SongEntrySelected -= GetSongEntry;
        MessageHandler.DoubleClick -= ChangeSong;
    }
    private void Update()
    {
        if(audioVisualizer._audioSource.time <= 0 && audioVisualizer._audioSource.clip != null && audioPlayerList.GetIndexOfCurrentSong() + 1 < audioPlayerList.playList.Count)
        {
            ChangeSong(audioVisualizer._audioSource, audioPlayerList.playList[audioPlayerList.GetIndexOfCurrentSong() + 1]);
        }
        
        if(Input.GetKey(KeyCode.Delete))
        {
            MessageHandler.OnDeleteSongEntry(currentSelected, null);
        }
        
        if (!Input.GetMouseButtonDown(0)) return;

        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= DOUBLE_CLICK_TIME)
        {
            
            MessageHandler.OnDoubleClick(audioVisualizer._audioSource, currentSelected);
        }

        lastClickTime = Time.time;
    }

    void UpdateButtonUI(AudioSource audioSource)
    {
        pauseButton.SetActive(playing);
        playButton.SetActive(!playing);
    }

    public async void PauseClicked(AudioSource audioSource)
    {
        Debug.Log("Pause clicked!");
        if (!audioSource.isPlaying) return;

        playing = false;
        MessageHandler.OnButtonClicked(audioSource);
        
        await FadeOut(audioSource, 0.2f);
        audioSource.Pause();
        
    }
    
    public async void PlayClicked(AudioSource audioSource)
    {
        if (audioSource.isPlaying) return;
        
        if (audioSource.clip == null && audioPlayerList.playList.Count > 0)
            audioSource.clip = audioPlayerList.playList[0].AudioClip;
       
        if(audioPlayerList.playList[0] == null)
            Debug.LogError("There is no audio in list. Drag and drop a file to populate the list."); 

        playing = true;
        MessageHandler.OnButtonClicked(audioSource);

        audioSource.Play();
        await FadeIn(audioSource, 0.2f);
    }

    public async void StopClicked(AudioSource audioSource)
    {
        if(!audioSource.isPlaying) return;

        playing = false;
        MessageHandler.OnButtonClicked(audioSource);
        await FadeOut(audioSource, 0.2f);
        audioSource.Stop();
    }
    
    public async void Previous(AudioSource audioSource)
    {
        if(audioPlayerList.playList.Count <= 0) return;
        
        var nextSongIndex = audioPlayerList.GetIndexOfCurrentSong() - 1;
        if(nextSongIndex < 0) return;

        await FadeOut(audioSource, 0.1f);
        audioSource.clip = audioPlayerList.playList[nextSongIndex].AudioClip;
        MessageHandler.OnButtonClicked(audioSource);
        audioSource.Play();
        await FadeIn(audioSource, 0.1f);
        
    }

    public async void Next(AudioSource audioSource)
    {
        if(audioPlayerList.playList.Count <= 0) return;
        
        var nextSongIndex = audioPlayerList.GetIndexOfCurrentSong() + 1;
        if(audioPlayerList.playList.Count - 1 < nextSongIndex) return;
        

        await FadeOut(audioSource, 0.1f);
        audioSource.clip = audioPlayerList.playList[nextSongIndex].AudioClip;
        MessageHandler.OnButtonClicked(audioSource);
        audioSource.Play();
        await FadeIn(audioSource, 0.1f);

    }

    async void ChangeSong(AudioSource audioSource, SongEntry songEntry)
    {
        await FadeOut(audioSource, 0.1f);
        audioSource.clip = songEntry.AudioClip;
        MessageHandler.OnButtonClicked(audioSource);
        audioSource.Play();
        await FadeIn(audioSource, 0.1f);
    }


    async Task FadeOut(AudioSource audioSource, float fadeTime)
    {
        float timeElapsed = 0f;
        if (!audioSource.isPlaying) await Task.CompletedTask;
        
        while (timeElapsed < fadeTime && audioSource.volume > 0)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, timeElapsed / fadeTime);
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        }
    }

    async Task FadeIn(AudioSource audioSource, float fadeTime)
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeTime && audioSource.volume < maxVolume)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume, timeElapsed / fadeTime);
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        } 
    }

    void GetSongEntry(SongEntry songEntry, AudioSource source) => currentSelected = songEntry;

    void ResetVolume(AudioSource audioSource)
    {
        audioSource.volume = maxVolume;
    }
}

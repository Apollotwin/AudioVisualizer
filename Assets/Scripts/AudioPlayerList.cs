using System;
using System.Collections;
using System.Collections.Generic;
using AudioImporter.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayerList : MonoBehaviour
{
    [SerializeField] private AudioVisualizer audioVisualizer;
    [SerializeField] private RectTransform playerListRect;
    [SerializeField] private GameObject songEntryPrefab;
    [SerializeField] private GameObject contentObject;
    [SerializeField] private Text currentSongName;
    [SerializeField] private Text currentSongTime;
    private AudioClip audioClip;
    public List<SongEntry> playList = new List<SongEntry>();

    void OnEnable ()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += AddFileToPlayList;
        MessageHandler.ButtonClicked += UpdateCurrentSongUI;
        MessageHandler.PlayListChanged += UpdatePlayList;
        MessageHandler.FileImported += AddNewSongEntry;
    }
    
    void OnDisable()
    {
        UnityDragAndDropHook.OnDroppedFiles -= AddFileToPlayList;
        UnityDragAndDropHook.UninstallHook();
        MessageHandler.ButtonClicked -= UpdateCurrentSongUI;
        MessageHandler.PlayListChanged -= UpdatePlayList;
        MessageHandler.FileImported -= AddNewSongEntry;
    }
    
    public int GetIndexOfSong(SongEntry songEntry)
    {
        return playList.IndexOf(playList.Find(current => current.AudioClip == songEntry.AudioClip));
    }

    public int GetIndexOfCurrentSong()
    {
        return playList.IndexOf(playList.Find(current => current.AudioClip == audioVisualizer._audioSource.clip));
    }

    void UpdateCurrentSongUI(AudioSource source, SongEntry songEntry)
    {
        if(source.clip == null) return;
        currentSongName.text = source.clip.name;
    }

    private void Update()
    {
        if (audioVisualizer._audioSource.clip == null) return;
        
        var ts = TimeSpan.FromSeconds(audioVisualizer._audioSource.time);
        currentSongTime.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
    }

    void AddFileToPlayList(List<string> aFiles, POINT aPos)
    {
        if (!IsMouseOverPlayList(Input.mousePosition)) return;
        
        foreach (var path in aFiles)
        {
            Debug.LogError("Importing: " + path);
            if(CheckIfSongExists(GetFileName(path))) continue;
            StartCoroutine(Import(path));
        }
    }
    
    bool IsMouseOverPlayList(Vector2 mousePos)
    {
        Vector2 localMousePosition = playerListRect.InverseTransformPoint(Input.mousePosition);
        return playerListRect.rect.Contains(localMousePosition);
    }

    public void Test(string path)
    {
        if(CheckIfSongExists(GetFileName(path))) return;
        StartCoroutine(Import(path));
    }

    void AddNewSongEntry(AudioClip audioClip)
    {
        var newEntry = Instantiate(songEntryPrefab, contentObject.transform);
        newEntry.TryGetComponent(out SongEntry entry);
        
        if(audioClip != null) 
            entry.Init(audioClip);
        else
            Debug.LogError("No AudioClip to assign!");
        
        playList.Add(entry);
        
        MessageHandler.OnPlayListChanged(entry);

        Debug.LogError($"List contains {playList.Count} objects: ");
        
        foreach (var songEntry in playList)
        {
            Debug.LogError($"Name: {songEntry.Name}");
            if(songEntry.AudioClip == null) 
                Debug.LogError($"Audioclip: null");
            else
            {
                Debug.LogError($"Audioclip: {songEntry.AudioClip.name}");
                Debug.LogError($"Length: {songEntry.AudioClip.length}");
            }
        }
    }

    void UpdatePlayList(SongEntry songEntry)
    {
        for (int i = 0; i < playList.Count; i++)
        {
            playList[i].NameText = $"{i + 1}. {playList[i].Name}";
            var ts = TimeSpan.FromSeconds(playList[i].AudioClip.length);
            playList[i].TimeText = $"{ts.Minutes:00}:{ts.Seconds:00}";
        }
    }

    string GetFileName(string path)
    {
        string fileName = "";
            
        for (int i = path.LastIndexOf('\\') + 1; i < path.Length; i++)
        {
            fileName += path[i];
        }

        return fileName;
    }
    
    IEnumerator Import(string path)
    {
        //importer.Import(path);

        var go = new GameObject("Importing: " + GetFileName(path));
        var newImporter = go.AddComponent<NAudioImporter>();
        
        newImporter.Import(path);

        while (!newImporter.isInitialized && !newImporter.isError)
            yield return null;

        if (newImporter.isError)
            Debug.LogError(newImporter.error);

        newImporter.audioClip.name = GetFileName(path);
        
        MessageHandler.OnFileImported(newImporter.audioClip);
        
        if(newImporter.isDone) Destroy(go);
    }

    bool CheckIfSongExists(string fileName)
    {
        if (playList.Count < 0) return false;
        
        foreach (var songEntry in playList)
        {
            if (songEntry.Name == fileName)
            {
                Debug.LogError(fileName + " already exist is list!");
                return true;
            }
        }

        return false;
    }

    void DeleteEntry(SongEntry songEntry, AudioSource audioSource)
    {
        playList.RemoveAt(GetIndexOfSong(songEntry));
    }

    
}

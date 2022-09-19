using UnityEngine;
using UnityEngine.EventSystems;

public static class MessageHandler
{
    public delegate void SongEntryHandler (SongEntry songEntry , AudioSource audioSource);
    public static event SongEntryHandler SongEntrySelected;
    public static event SongEntryHandler DeleteSongEntry;
    public static event SongEntryHandler SongEntryDeselected;
        
    public delegate void PlayList(SongEntry songEntry);
    public static event PlayList PlayListChanged;
    public static event PlayList SongEntryDoubleClicked;

    public delegate void Importer(AudioClip audioClip);
    public static event Importer FileImported;

    public delegate void ButtonClick(AudioSource audioSource, SongEntry songEntry);
    public static event ButtonClick ButtonClicked;

    public delegate void MouseAction(AudioSource audioSource, SongEntry songEntry);
    public static event MouseAction DoubleClick;
        
    public static void OnSongEntrySelected(SongEntry songEntry , AudioSource audioSource)
    {
        SongEntrySelected?.Invoke(songEntry, audioSource);
    }

    public static void OnSongEntryDeselected(SongEntry songEntry , AudioSource audioSource)
    {
        SongEntryDeselected?.Invoke(songEntry,audioSource);
    }
        
    public static void OnSongEntryDoubleClicked(SongEntry songEntry)
    {
        SongEntryDoubleClicked?.Invoke(songEntry);
    }
        
    public static void OnPlayListChanged(SongEntry songEntry)
    {
        PlayListChanged?.Invoke(songEntry);
    }
        
    public static void OnButtonClicked(AudioSource audioSource, SongEntry songEntry)
    {
        ButtonClicked?.Invoke(audioSource, songEntry);
    }

    public static void OnDoubleClick(AudioSource audioSource, SongEntry songEntry)
    {
        DoubleClick?.Invoke(audioSource,songEntry);
    }

    public static void OnFileImported(AudioClip audioClip)
    {
        FileImported?.Invoke(audioClip);
    }

    public static void OnDeleteSongEntry(SongEntry songEntry, AudioSource audiosource)
    {
        DeleteSongEntry?.Invoke(songEntry, audiosource);
    }
}
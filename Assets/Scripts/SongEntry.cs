using System;
using UnityEngine;
using UnityEngine.UI;

public class SongEntry : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Text nameText;
    [SerializeField] private Text timeText;
    [SerializeField] private Image image;
    private string name;

    private Color textSelected = new Color(0.2f, 0.2f, 0.2f, 1f);
    private Color textColorDefualt = Color.yellow;
    private Color imageSelected = Color.yellow;
    private Color imageHovered = Color.gray;
    private Color imageDefaultColor = Color.clear;

    private bool selected = false;
    private bool loadInitialized = false;
    private bool highLight;
    

    public AudioClip AudioClip
    {
        get => audioClip;
        set => audioClip = value;
    }
    public string Name
    {
        get => name;
        private set => name = value;
    }

    public string TimeText
    {
        get => timeText.text;
        set => timeText.text = value;
    }
    
    public string NameText
    {
        set => nameText.text = value;
    }

    

    private void OnEnable()
    {
        MessageHandler.ButtonClicked += HighLight;
        MessageHandler.DoubleClick += DoubleClickEntry;
    }

    private void OnDisable()
    {
        MessageHandler.ButtonClicked -= HighLight;
        MessageHandler.DoubleClick -= DoubleClickEntry;
    }

    public void Init(AudioClip audioClip)
    {
        this.audioClip = audioClip;
        Name = audioClip.name;
        nameText.color = textColorDefualt;
        image.color = imageDefaultColor;
    }

    private void Update()
    {
        if(highLight) return;
        
        if(!selected)
        {
            image.color = IsMouseOverSongEntry(Input.mousePosition) ? imageHovered : imageDefaultColor;
        }

        if (IsMouseOverSongEntry(Input.mousePosition) && Input.GetKey(KeyCode.Mouse0) && !selected)
        {
            SelectEntry(this);
        }
        
        if(!IsMouseOverSongEntry(Input.mousePosition) && Input.GetKey(KeyCode.Mouse0) && selected)
        {
            DeselectEntry(this);
        }
        
        if(!IsMouseOverSongEntry(Input.mousePosition) && Input.GetKey(KeyCode.Mouse0))
        {
            DeselectEntry(this);
        }
    }

    void SelectEntry(SongEntry songEntry)
    {
        selected = true;
        image.color = imageSelected;
        nameText.color = textSelected;
        
        MessageHandler.OnSongEntrySelected(songEntry,null);
    }

    void DeselectEntry(SongEntry songEntry)
    {
        selected = false;
        nameText.color = textColorDefualt;
        
        MessageHandler.OnSongEntryDeselected(songEntry, null);
    }

    void DoubleClickEntry(AudioSource audioSource, SongEntry songEntry)
    {
        if(songEntry != this) DeselectEntry(this);
        
        if (IsMouseOverSongEntry(Input.mousePosition))
        {
            MessageHandler.OnSongEntryDoubleClicked(this);

        }
    }

    bool IsMouseOverSongEntry(Vector2 mousePos)
    {
        Vector2 localMousePosition = image.rectTransform.InverseTransformPoint(Input.mousePosition);
        return image.rectTransform.rect.Contains(localMousePosition);
    }

    public void HighLight(AudioSource source, SongEntry songEntry)
    {
        highLight = source.clip == audioClip;
        image.color = highLight ? imageSelected : imageDefaultColor;
        nameText.color = highLight ? textSelected : textColorDefualt;
        timeText.color = highLight ? textSelected : textColorDefualt;
    }

}

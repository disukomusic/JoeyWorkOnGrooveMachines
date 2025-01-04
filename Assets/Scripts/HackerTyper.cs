using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq; // For Contains method
using System.Text.RegularExpressions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random; // For splitting words

public class HackerTyper : MonoBehaviour
{
    public float TypeSpeed { get; set; }

    [SerializeField] public CameraSnapper camSnapper;
    [SerializeField] public GameObject camSnapObject;
    [SerializeField] public TMP_Text textMeshPro;
    [SerializeField] public ScrollRect scrollRect;
    [SerializeField] private HaxorMeter haxorMeter;
    [SerializeField] private LevelUpEffect levelUpEffect;
    [SerializeField] private BugSpawner bugSpawner; 
    public FlashColor methodCompleteEffect;
    public bool IDECrashed;
    public GameObject crashPanel;
    [FormerlySerializedAs("autoTypeInput")] public bool autoCodeInput;

    [Header("Typing Settings")]
    public Method[] typingMethods;
    public int bugMaximum;
    private string _codeToType = "";

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private float volumeMultiplier = 10f; // Multiplier for adjusting volume based on typing speed
    [SerializeField] private float volumeSmoothSpeed = 5f; // Speed at which volume adjusts to typing speed
    [SerializeField] public float idleFadeSpeed = 1f; // Speed at which the volume fades to zero when idle
    private float _lastKeyPressTime;
    private float _targetVolume; // The target volume based on typing speed
    public bool isHyperTyping;
    
    private string[] _wordsToType; 
    private int _wordIndex;    
    public int charIndex; 
    
    private KeytypeSound _keytypeSound;

    public int chunkSize = 5; // Add this line to make chunkSize adjustable in the Inspector
    public int scoreMultipler = 1;

    void Start()
    {
        _keytypeSound = gameObject.GetComponent<KeytypeSound>();
        PickRandomMethod();
        _wordsToType = Regex.Split(_codeToType, @"(\s+|\W)"); // Split code into words and non-word characters
        _lastKeyPressTime = Time.time; // Initialize with the current time
        _targetVolume = musicAudioSource.volume; // Start with the initial volume
    }

    void Update()
    {
        // Calculate time since last key press
        float timeSinceLastPress = Time.time - _lastKeyPressTime;
        
        if (((isHyperTyping ? Input.anyKey : Input.anyKeyDown) || autoCodeInput)
            && _wordIndex < _wordsToType.Length 
            && (camSnapper.currentCamera == camSnapObject || autoCodeInput)
            && bugSpawner.GetBugCount() < bugMaximum 
            && !IDECrashed 
            && GameManager.instance.isGamePlaying)
        {
            _keytypeSound.PlayKeySound();
            
            float typingSpeed = Mathf.Clamp(1f / timeSinceLastPress, 0f, 1f);
            TypeSpeed = typingSpeed; 

            _targetVolume = Mathf.Lerp(0f, 0.5f, typingSpeed * volumeMultiplier);
            _lastKeyPressTime = Time.time;
            musicAudioSource.volume = Mathf.Lerp(musicAudioSource.volume, _targetVolume, Time.deltaTime * volumeSmoothSpeed);

            string token = _wordsToType[_wordIndex];

            if (string.IsNullOrWhiteSpace(token) || !Regex.IsMatch(token, @"\w"))
            {
                string highlightedToken = ApplySyntaxHighlighting(token);
                textMeshPro.text += highlightedToken;
                _wordIndex++;
                charIndex = 0; 
            }
            else
            {
                if (charIndex < token.Length)
                {
                    string chunk = token.Substring(charIndex, Mathf.Min(chunkSize, token.Length - charIndex));
                    string highlightedChunk = ApplySyntaxHighlighting(chunk);

                    textMeshPro.text += highlightedChunk;

                    charIndex += chunkSize;

                    Canvas.ForceUpdateCanvases();
                    scrollRect.verticalNormalizedPosition = 0f;
                }

                if (charIndex >= token.Length)
                {
                    _wordIndex++;
                    charIndex = 0;
                }
            }
            haxorMeter.UpdateHaxorMeterWidth();
        }
        else
        {
            if (timeSinceLastPress > 0.2f) 
            {
                _targetVolume = 0f;
            }

            musicAudioSource.volume = Mathf.Lerp(musicAudioSource.volume, _targetVolume, Time.deltaTime * idleFadeSpeed);
        }

        if (_wordIndex >= _wordsToType.Length)
        {
            _keytypeSound.PlayKeySound();
            _keytypeSound.PlayMethodCompleteSound();
            levelUpEffect.PlayLevelUpEffect();
            GameManager.instance.UpdateHaxorScore(_codeToType.Length * scoreMultipler);
            _wordIndex = 0;
            methodCompleteEffect.FlashOpacityEffect();
            PickRandomMethod();
            _wordsToType = Regex.Split(_codeToType, @"(\s+|\W)");
        }
    }

    private void PickRandomMethod()
    {
        textMeshPro.text = "";
        Method randomMethod = typingMethods[Random.Range(0, typingMethods.Length)];
        _codeToType = randomMethod.methodDescription;
    }

    public void ResetActiveMethodProgress()
    {
        textMeshPro.text = "";
        charIndex = 0;
        _wordIndex = 0; 
        _wordsToType = Array.Empty<string>(); 
        haxorMeter.UpdateHaxorMeterWidth();
    }

    private string ApplySyntaxHighlighting(string token)
    {
        if (isHyperTyping || autoCodeInput)
        {
            string matrixText = $"<color=#00FF00>{token}</color>"; 
            return matrixText;
        }

        // Existing syntax highlighting
        string[] keywords = { "public", "private", "protected", "static", "void", "string", "int", "float", "double", "class" };
        string[] types = { "List", "Console", "Characters" };
        string[] controlFlow = { "if", "else", "for", "while", "return", "switch" };

        if (keywords.Contains(token))
            return $"<color=#1E90FF><b>{token}</b></color>"; 
        if (types.Contains(token))
            return $"<color=#00FFCC>{token}</color>"; 
        if (controlFlow.Contains(token))
            return $"<color=#FFD700>{token}</color>";
        if (token.StartsWith("\"") && token.EndsWith("\""))
            return $"<color=#FF6347>{token}</color>"; 
        if (token.StartsWith("//"))
            return $"<color=#32CD32>{token}</color>";

        return token;
    }


    public float TypingCompletionPercentage
    {
        get
        {
            if (_wordsToType == null || _wordsToType.Length == 0)
                return 0f;

            float wordProgress = (float)_wordIndex / _wordsToType.Length;
            float charProgress = charIndex > 0 && _wordIndex < _wordsToType.Length
                ? (float)charIndex / _wordsToType[_wordIndex].Length / _wordsToType.Length
                : 0f;

            return Mathf.Clamp01(wordProgress + charProgress);
        }
    }

    public void StartIDECrash()
    {
        ResetActiveMethodProgress();
        GameManager.instance.UpdateHaxorScore(- GameManager.instance.HaxorScore);
        haxorMeter.UpdateHaxorMeterWidth();
        crashPanel.SetActive(true);
    }

    public void StopIDECrash()
    {
        crashPanel.SetActive(false);
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour {
    #region SerializeFields
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundsAudioSource;
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private AudioClip _buttonClip;
    [SerializeField] private AudioClip _coinClip;
    [SerializeField] private AudioClip _magicClip;
    [SerializeField] private AudioClip _magicBGClip;
    [SerializeField] private AudioClip _glassClip;
    [SerializeField] private AudioClip _natureClip;
    [SerializeField] private AudioClip _strikeClip;
    [SerializeField] private AudioClip _sliderClip;
    #endregion

    public static AudioManager Instanse { get; private set; }
    public static AudioSource MusicAudioSource { get => Instanse._musicAudioSource; }
    public static AudioSource SoundsAudioSource { get => Instanse._soundsAudioSource; }

    #region Properties
    public AudioClip MagicClip { get => _magicClip; private set => _magicClip = value; }
    public AudioClip GlassClip { get => _glassClip; private set => _glassClip = value; }
    public AudioClip NatureClip { get => _natureClip; private set => _natureClip = value; }
    public AudioClip StrikeClip { get => _strikeClip; private set => _strikeClip = value; }
    public AudioClip SliderClip { get => _sliderClip; private set => _sliderClip = value; }
    #endregion

    public void PlaySound(AudioClip clip) => SoundsAudioSource.PlayOneShot(clip);
    public static void PlayButtonSound() => SoundsAudioSource.PlayOneShot(Instanse._buttonClip);
    public static void PlayCoinSound() => SoundsAudioSource.PlayOneShot(Instanse._coinClip);

    private void Awake() => Instanse = this;

    private void Start() {
        if (_musicAudioSource != null && _musicClip != null) {
            _musicAudioSource.clip = _musicClip;
            _musicAudioSource.Play(); 
        }

        InvokeRepeating(nameof(PlayEnvironmentSounds), 1f, 3f);
    }

    public void ChacheBGMusic() {
        if (GameManager.Instance.State == GameState.BonusFreese) {
            _musicAudioSource.clip = _magicBGClip;
            _musicAudioSource.Play();
        }
        else {
            _musicAudioSource.clip = _musicClip;
            _musicAudioSource.Play();
        }
    }

    private void PlayEnvironmentSounds() {
        if (GameManager.Instance.State != GameState.BonusFreese) {
            _soundsAudioSource.PlayOneShot(NatureClip); 
        }
    }

    private void OnValidate() {
        _musicAudioSource.playOnAwake = true;
        _musicAudioSource.loop = true;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
    public static AudioSettings Instance { get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] private Slider _music, _sounds;
    private float _musicVolume, _soundsVolume;

    /// <summary>
    /// Drfault = 1f (full volume)
    /// </summary>
    /// <param name="music"></param>
    /// <param name="sound"></param>
    public void LoadSettings(float music = 0.6f, float sound = 1f) {
        _music.value = _musicVolume = music;
        _sounds.value = _soundsVolume = sound;
        AudioManager.MusicAudioSource.volume = music;
        AudioManager.SoundsAudioSource.volume = sound;
    }

    public void OnEnable() {
        _music.onValueChanged.RemoveAllListeners();
        _music.onValueChanged.AddListener((v) => {
            AudioManager.MusicAudioSource.volume = v;
            _musicVolume = v;
            SavesProvider.Instance.SaveSettings(_musicVolume, _soundsVolume);
        });

        _sounds.onValueChanged.RemoveAllListeners();
        _sounds.onValueChanged.AddListener((v) => {
            AudioManager.SoundsAudioSource.volume = v;
            _soundsVolume = v;
            SavesProvider.Instance.SaveSettings(_musicVolume, _soundsVolume);
        });
    }
}
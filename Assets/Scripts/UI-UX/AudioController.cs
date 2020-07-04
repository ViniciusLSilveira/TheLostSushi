using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{

    public AudioMixer m_Mixer;
    public Slider m_MasterSlider;
    public Slider m_MusicSlider;
    public Slider m_SoundSlider;

    private float m_MasterVolume;
    private float m_MusicVolume;
    private float m_SoundVolume;

    private void Start() {
        m_MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        m_MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        m_SoundVolume = PlayerPrefs.GetFloat("SoundVolume");

        float masterVol = m_MasterVolume == 0 ? 0.50f : m_MasterVolume;
        float musicVol = m_MusicVolume == 0 ? 0.50f : m_MusicVolume;
        float soundVol = m_SoundVolume == 0 ? 0.50f : m_SoundVolume;
        
        if (m_MasterSlider) {
            m_MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", masterVol);
        }

        if (m_MusicSlider) {
            m_MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", musicVol);
        }

        if (m_SoundSlider) {
            m_SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume", soundVol);
        }

        m_Mixer.SetFloat("MasterVol", Mathf.Log10(m_MasterVolume) * 20);
        m_Mixer.SetFloat("MusicVol", Mathf.Log10(m_MusicVolume) * 20);
        m_Mixer.SetFloat("SoundVol", Mathf.Log10(m_SoundVolume) * 20);
    }

    public void SetMasterVolume(float sliderValue) {
        m_Mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);

        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue) {
        m_Mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);

        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSoundVolume(float sliderValue) {
        m_Mixer.SetFloat("SoundVol", Mathf.Log10(sliderValue) * 20);

        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManu : MonoBehaviour
{
    [SerializeField] Slider musicVolSlider;
    [SerializeField] Slider musicSFXSlider;
    float _musicVol;
    float _sfxVol;

    public void OnEnable()
    {
        GameData.LoadPlayerMusicPrefs(out _musicVol, out _sfxVol);
        musicVolSlider.value = _musicVol;
        musicSFXSlider.value = _sfxVol;
    }
    public void OnMusicVolChange(float val)
    {
        _musicVol = val;
        AudioManager.Instance.SetMusicVol(_musicVol);
    }

    public void OnSFXVolChange(float val)
    {
        _sfxVol = val;
        AudioManager.Instance.SetSFXVol(_sfxVol);
    }

    public void OnDisable()
    {
        GameData.SavePlayerMusicPrefs( _musicVol, _sfxVol);

    }
}

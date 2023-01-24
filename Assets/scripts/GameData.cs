using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Scenes
{
    GameScene,
}
public static class GameData
{
    internal class StringsConsts
    {
        public static string PPMusicVolName { get; } = "MusicVol";
        public static string PPSFXVolName { get; } = "SFXVol";

    }

    public static void SavePlayerMusicPrefs(float musicVol, float sfxVol)
    {
        PlayerPrefs.SetFloat(StringsConsts.PPMusicVolName, musicVol);
        PlayerPrefs.SetFloat(StringsConsts.PPSFXVolName, sfxVol);
    }

    public static void LoadPlayerMusicPrefs(out float musicVol, out float sfxVol)
    {
        musicVol = PlayerPrefs.GetFloat(StringsConsts.PPMusicVolName, 1);
        sfxVol = PlayerPrefs.GetFloat(StringsConsts.PPSFXVolName, 1);
    }
}

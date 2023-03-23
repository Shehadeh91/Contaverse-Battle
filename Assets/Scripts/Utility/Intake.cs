using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intake : MonoBehaviour
{
    // Layer Mask definitions: 
    public static int lmInteractables = 1 << 11;
    public static int lmEnemies = 1 << 12;
    public static int lmGround = 1 << 10;
    public static int lmPlayer = 1 << 8;
    public static int lmWalls = 1 << 15;
    public static int lmCrystal = 1 << 16;

    // Generalize tags by making them static strings, so you can always reference them instead of typing stupid f***ing strings
    // public static string interactableTag = "Interactable";
    // public static string enemyTag = "Enemy";
    // public static string hudTag = "IngameHUD";
    // public static string playerTag = "Player";
    // public static string windZoneTag = "WindZone";

    // public static string prefSFX = "SFXVolume";
    // public static string prefMusic = "MusicVolume";
    // public static string prefMaster = "MasterVolume";
    // public static string prefHPCurrent = "HPCurrent";
    // public static string prefHPMax = "HPMax";
    // public static string prefCorruptionCurrent = "CorruptionCurrent";
    // public static string prefCorruptionMax = "CorruptionMax";

    // public static List<int> scores = new List<int> { 0, 0, 0, 0, 0 }; 

    // public static int score;
    // public static List<int> highScore = new List<int>(); 

    // public enum Difficulty { easy, hard};
    // public static Difficulty gameDifficulty;

    // private void Start()
    // {
    //     if (PlayerPrefs.HasKey("GameDifficulty"))
    //     {
    //         gameDifficulty = PlayerPrefs.GetInt("GameDifficulty") == 0 ? Difficulty.easy : Difficulty.hard;
    //     }
    //     else
    //     {
    //         gameDifficulty = Difficulty.hard;
    //     }
    // }
    // public static float GetDifficultyMultiplier()
    // {
    //     if (gameDifficulty == Difficulty.easy)
    //     {
            
    //         return 0.5f;
    //     }
    //     else
    //     {
    //         return 1;
    //     }
    // }
}
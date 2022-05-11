using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    // // Settings settings;
    void Awake() {
        // settings = FindObjectOfType<Settings>();
        // if(settings == null){
        //     Debug.LogError("null settings");
        //     return;
        // }
        // settings.GetComponent<Canvas>().enabled = false;

    }

    void Start(){
        FindObjectOfType<AudioManager>().Play("Theme");
    }

    public void PlayGame(){
        // AudioManager am = FindObjectOfType<AudioManager>();
        // int currTime = am.GetSoundTime("MenuTheme");
        // am.Stop("MenuTheme");
        // am.Play("ArenaTheme");
        // //test
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Main");
    }

    // public void EnableOptions(){
    //     settings.GetComponent<Canvas>().enabled = true;
    // }
    // public void DisableOptions(){
    //     settings.GetComponent<Canvas>().enabled = false;
    // }

    // public void ButtonSound(){
    //     audioManager.Play("ButtonPress");
    // }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    void Start(){ FindObjectOfType<AudioManager>().Play("Theme"); }

    public void PlayGame(){
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Main");
    }

    public void PlayTutorial(){
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Tutorial");
    }
}
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayValue : MonoBehaviour 
{
    private TextMeshPro txt { get; set; } 
    private Animator animator { get; set; } 

	void Awake () {
        txt = GetComponent<TextMeshPro>();
        animator = GetComponent<Animator>();
    }
    /**
    * This method Displays the ball's point value when a ball breaks a target
    */
    public void ShowTime(Vector3 position, ClockType time){
        txt.text = time.hour + ":" + (time.min * 5);
        animator.Play("Show Time", 0, 0f);
    }
}
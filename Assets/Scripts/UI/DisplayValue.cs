using UnityEngine;
using TMPro;

public class DisplayValue : MonoBehaviour {
    private TextMeshPro txt; 
    private Animator animator;
    private ParticleSystem confetti;

	void Awake() {
        txt = GetComponentInChildren<TextMeshPro>();
        animator = GetComponentInChildren<Animator>();
        confetti = GetComponentInChildren<ParticleSystem>();
    }

    public void ShowTime(Vector3 pos, ClockType time){
        transform.position = new Vector3(pos.x, pos.y, -5f);

        string min = (time.min).ToString("00");
        txt.text = $"{time.hour}:{min}";
        animator.Play("Show Time", 0, 0f);
    }

    public void ScoreTime(Vector3 pos, ClockType time){
        confetti.Play();
        transform.position = new Vector3(pos.x, pos.y, -5f);

        string min = (time.min).ToString("00");
        txt.text = $"{time.hour}:{min}";
        animator.Play("Show Time", 0, 0f);
    }
}
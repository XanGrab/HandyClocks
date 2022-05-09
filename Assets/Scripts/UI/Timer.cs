using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour {
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int duration;
    private int remaining;
    private bool pause;

    private void Start() {
        pause = false;
        Being(duration);
    }

    private void Being(int Second){
        remaining = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer(){
        while(remaining >= 0){
            if (!pause) {
                uiText.text = $"{remaining / 60:00}:{remaining % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, duration, remaining);
                remaining--;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd(){
        Debug.Log("Time Up!");
    }
}

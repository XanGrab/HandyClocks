using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour {
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int _duration;
    private int _timeRemaining;
    private bool pause;

    private void Start() {
        pause = false;
        _timeRemaining = _duration;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer(){
        while(_timeRemaining >= 0){
            if (!pause) {
                uiText.text = $"{_timeRemaining / 60:00}:{_timeRemaining % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, _duration, _timeRemaining);
                _timeRemaining--;
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

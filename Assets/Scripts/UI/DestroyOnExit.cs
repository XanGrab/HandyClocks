// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour {
    override
    public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        Destroy(animator.transform.gameObject);
    }
}

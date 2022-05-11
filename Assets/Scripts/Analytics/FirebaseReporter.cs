#if UNITY_WEBGL && !UNITY_EDITOR
#define FIREBASE
#endif

using System.Runtime.InteropServices;
// using UnityEngine;

public static class FirebaseReporter {
    // Game Start
    [DllImport("__Internal")]
    private static extern void FBGameStart();

    public static void GameStart() {
        #if FIREBASE
        FBGameStart();
        #endif
    }
}
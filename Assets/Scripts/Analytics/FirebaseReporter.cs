#if UNITY_WEBGL && !UNITY_EDITOR
#define FIREBASE
#endif

using System.Runtime.InteropServices;
using UnityEngine;

namespace FBAnalytics {
    public static class FirebaseReporter {

        // wrong clock made
        [DllImport("__Internal")]
        private static extern void FBWrongTime();

        public static void WrongTime(string time) {
            #if FIREBASE
            FBWrongTime(time);
            #endif
        }
    }
}
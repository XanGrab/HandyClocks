#if UNITY_WEBGL && !UNITY_EDITOR
#define FIREBASE
#endif

using System.Runtime.InteropServices;

namespace FBAnalytics {
    public static class Reporter {

        // game start
        [DllImport("__Internal")]
        private static extern void FBGameStart();

        public static void GameStart() {
            #if FIREBASE
            FBGameStart();
            #endif
        }

        // wrong clock made
        [DllImport("__Internal")]
        private static extern void FBWrongTime(string report);

        public static void WrongTime(string report) {
            #if FIREBASE
            FBWrongTime(report);
            #endif
        }

        // player scores
        [DllImport("__Internal")]
        private static extern void FBScoreTime(string report);

        public static void ScoreTime(string report) {
            #if FIREBASE
            FBWrongTime(report);
            #endif
        }

        // player scores
        [DllImport("__Internal")]
        private static extern void FBFinalScore(string report);

        public static void FinalScore(string report) {
            #if FIREBASE
            FBWrongTime(report);
            #endif
        }
    }
}
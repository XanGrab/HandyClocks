using UnityEngine;
using FBAnalytics;

public static class Boot {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadSystems() {
        Reporter.GameStart();
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
    }
}

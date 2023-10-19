using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common;
using StormStudio.Common.UI;
using StormStudio.Common.Utils;
using StormStudio.GameOps;

public partial class GameFlow : MonoBehaviour
{
    public string UserName { get { return PlayerPrefs.GetString("KEY_USER_NAME", "DNguyen"); } set { PlayerPrefs.SetString("KEY_USER_NAME", value); PlayerPrefs.Save(); } }
    public int TurnAmount { get { return PlayerPrefs.GetInt("KEY_AMOUNT", 50); } set { PlayerPrefs.SetInt("KEY_AMOUNT", value); PlayerPrefs.Save(); } }
    public int Interval { get { return PlayerPrefs.GetInt("KEY_INTERVAL", 1); } set { PlayerPrefs.SetInt("KEY_INTERVAL", value); PlayerPrefs.Save(); } }


    public static GameFlow Instance { get; private set; }

    public enum GameState
    {
        Init,
        Home,
        Gameplay,
        Tutorial
    }

    private GSMachine _gsMachine = new GSMachine();
    private StormStudioApp _stormApp;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupStormApp();

#if !BUILD_DEV || DISABLE_LOGS
        Debug.unityLogger.logEnabled = false;
#endif
        Input.multiTouchEnabled = false;

#if UNITY_STANDALONE && !UNITY_EDITOR
        Screen.SetResolution(1920, 1080, false);
#endif

        if (Application.isEditor)
            Application.runInBackground = true;

        Application.targetFrameRate = 60;
    }

    void SetupStormApp()
    {
        _stormApp = new GameObject("StormApp", typeof(StormStudioApp)).GetComponent<StormStudioApp>();
        Object.DontDestroyOnLoad(_stormApp.gameObject);
    }

    IEnumerator Start()
    {
        yield return null;

        SoundManager.Instance.LoadSoundSettings();

        // Start game state machine
        _gsMachine.Init(OnStateChanged, GameState.Init);
        SoundManager.Instance.OnEnableMusic += onEnableMusic;
        while (true)
        {
            _gsMachine.StateUpdate();
            yield return null;
        }
    }

    void onEnableMusic(bool enabled)
    {
        if (enabled)
        {
            switch ((GameState)_gsMachine.CurrentState)
            {
                case GameState.Home:
                    SoundManager.Instance.PlayBgmHome();
                    break;
                case GameState.Gameplay:
                    SoundManager.Instance.PlayBgmGameplay();
                    break;
            }
        }
    }

    public void SceneTransition(System.Action onSceneOutFinished)
    {
        UIManager.Instance.SetUIInteractable(false);
        SceneDirector.Instance.Transition(new TransitionFade()
        {
            duration = 0.667f,
            tweenIn = TweenFunc.TweenType.Sine_EaseInOut,
            tweenOut = TweenFunc.TweenType.Sine_EaseOut,
            onStepOutDidFinish = () =>
            {
                onSceneOutFinished.Invoke();
            },
            onStepInDidFinish = () =>
            {
                UIManager.Instance.SetUIInteractable(true);
            }
        });
    }

    #region GSMachine
    GSMachine.UpdateStateDelegate OnStateChanged(System.Enum state)
    {
        switch (state)
        {
            case GameState.Init:
                return GameState_Init;
            case GameState.Home:
                return GameState_Home;
            case GameState.Gameplay:
                return GameState_Gameplay;
        }

        return null;
    }
    #endregion
}

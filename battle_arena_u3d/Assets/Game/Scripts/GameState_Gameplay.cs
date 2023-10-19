using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using static StormStudio.Common.GSMachine;

public partial class GameFlow : MonoBehaviour
{
    PlayUI _playUI = null;

    void GameState_Gameplay(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            _playUI = UIManager.Instance.ShowUIOnTop<PlayUI>("PlayUI");
            _playUI.Setup(UserName, 2000);
            _playUI.OnSetting = OnSetting;
        }
        else if (stateEvent == StateEvent.Exit)
        {
            UIManager.Instance.ReleaseUI(_playUI, true);
            _playUI = null;
        }
    }

    public void OnGameInfoChanged(GameInfo gameInfo)
    {
        if (_playUI != null) _playUI.UpdateGameInfo(gameInfo);
    }

    public void OnSkillReload(float currentValue, float totalTime)
    {
        if (_playUI != null) _playUI.UpdateSkillReload((totalTime - currentValue) / totalTime);
    }

    public void OnUserHP(int hp, int totalHP)
    {
        if (_playUI != null) _playUI.UpdateHP(hp, totalHP);
    }

    public void OnSetting()
    {
        Time.timeScale = 0;
        var settingUI = UIManager.Instance.ShowUIOnTop<SettingUI>("SettingUI");
        settingUI.Setup(GameFlow.Instance.TurnAmount, GameFlow.Instance.Interval, (newAmount) =>
        {
            TurnAmount = newAmount;
            var infoSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameInfoUpdateSystem>();
            infoSystem.ForceUpdate();
        }, (newInterval) =>
        {
            Interval = newInterval;
        });
        settingUI.OnClosed = () => Time.timeScale = 1;
        settingUI.OnReset = () =>
        {
            Time.timeScale = 1;
            var infoSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameInfoUpdateSystem>();
            infoSystem.ForceReset();
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
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
            _playUI.Setup(UserName, GameFlow.Instance.MaximumHP, onAttack, onSkill);
        }
        else if (stateEvent == StateEvent.Exit)
        {
            UIManager.Instance.ReleaseUI(_playUI, true);
            _playUI = null;
        }
    }

    void onAttack()
    {

    }

    void onSkill()
    {

    }

    public void OnGameInfoChanged(GameInfo gameInfo)
    {
        if (_playUI != null) _playUI.UpdateAmountEnemies(gameInfo.CountMelee, gameInfo.CountRange);
    }
}

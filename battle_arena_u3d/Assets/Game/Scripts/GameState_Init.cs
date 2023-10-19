using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StormStudio.Common.GSMachine;

public partial class GameFlow : MonoBehaviour
{
    void GameState_Init(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            _gsMachine.ChangeState(GameState.Gameplay);
        }
        else if (stateEvent == StateEvent.Exit)
        {

        }
    }
}

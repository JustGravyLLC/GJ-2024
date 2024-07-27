using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEventInteractable : EventInteractable
{
    protected override void EnterEvent()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                _gameController.timer.AddTime(-3f);
                _gameController.EnterEvent("Shivering rats bite your feet. You lose time.");
                break;

            case 1:
                _gameController.EnterEvent("A quiet moment for a good stretch. You gain time.");
                _gameController.timer.AddTime(5f);
                break;

            case 2:
                _gameController.EnterEvent("You find a crude map. You gain time.");
                _gameController.timer.AddTime(10f);
                break;

            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPanel : MonoBehaviour
{
    private bool _initialized = false;
    private GameController _gameController;
    
    public GameObject panelObject;

    public void Initialize()
    {
        if (_initialized) return;

        _gameController = FindFirstObjectByType<GameController>();
        panelObject.SetActive(false);

        _initialized = true;
    }

    public void OpenPanel()
    {
        panelObject.SetActive(true);
    }

    public void ClosePanel()
    {
        panelObject.SetActive(false);
        _gameController.ExitCheckpoint();
    }
}

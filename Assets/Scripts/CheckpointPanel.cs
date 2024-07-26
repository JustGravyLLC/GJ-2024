using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPanel : MonoBehaviour
{
    private GameController _gameController;
    
    public GameObject panelObject;

    public void Initialize()
    {
        _gameController = FindFirstObjectByType<GameController>();
        panelObject.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    private GameController _gameController;
    private bool _initialized = false;

    [SerializeField]
    private GameObject _eventScreen;
    [SerializeField]
    private TextMeshProUGUI _eventText;
    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private GameObject _loseScreen;

    public void Initialize()
    {
        _gameController = FindFirstObjectByType<GameController>();
        _eventScreen.SetActive(false);
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
    }

    public void ShowEventPanel()
    {
        _eventScreen.SetActive(true);
    }
    public void ShowEventPanel(string text)
    {
        ShowEventPanel();
        _eventText.text = text;
    }

    public void CloseEventPanel()
    {
        _eventScreen.SetActive(false);
        _gameController.ExitEvent();
    }

    public void ShowEndPanel(LevelEndReason reason)
    {
        if(reason == LevelEndReason.OUT_OF_TIME)
        {
            _loseScreen.SetActive(true);
        }else
        {
            _winScreen.SetActive(true);
        }
    }

    public void CloseEndPanel()
    {
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
    }
}

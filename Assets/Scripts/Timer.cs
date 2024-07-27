using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private GameController _gameController;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    private bool initialized = false;

    private float timeLeft = 60f;

    public void Initialize()
    {
        _gameController = FindFirstObjectByType<GameController>();

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!initialized) return;
        if (_gameController.currentState != GameState.RUNNING) return;

        if(timeLeft < 0)
        {
            _gameController.EndLevel(LevelEndReason.OUT_OF_TIME);
        }

        timeLeft -= Time.deltaTime;
        UpdateTimeText();
    }

    public void SetCheckpoint(Checkpoint cp)
    {
        AddTime(cp.time);
        this.transform.position = new Vector2(cp.location, this.transform.position.y);
    }
    private void SetTimer(float t)
    {
        timeLeft = t;
    }
    public void AddTime(float t)
    {
        timeLeft += t;
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        textMesh.text = (timeLeft + "").Substring(0, 4);
    }
}

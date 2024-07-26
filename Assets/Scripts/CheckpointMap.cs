using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMap : MonoBehaviour
{
    [SerializeField]
    private Transform startMount, finishMount;
    [SerializeField]
    private Transform avatar;
    private GameController _gameController;

    public GameObject checkpointMarker;
    [SerializeField]
    public List<Checkpoint> checkpoints;
    
    public int nextCheckpoint = 0;
    private bool initialized = false;

    public void Initialize()
    {
        _gameController = GameObject.FindFirstObjectByType<GameController>();

        float lastCheckpoint = checkpoints[checkpoints.Count - 1].distance;

        //for each checkpoint, make a marker
        foreach(Checkpoint checkpoint in checkpoints)
        {
            GameObject marker = Instantiate<GameObject>(checkpointMarker);
            marker.transform.parent = this.transform;

            float x = checkpoint.distance / lastCheckpoint;
            x = x * (finishMount.position.x - startMount.position.x) + startMount.position.x;
            checkpoint.location = x;

            marker.transform.position = new Vector2(x, this.transform.position.y);
        }

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;
        if (_gameController.currentState != GameState.RUNNING) return;

        //update avatar position based on distance traveled
        float l = _gameController.distanceTraveled / checkpoints[checkpoints.Count - 1].distance;
        avatar.position = new Vector2(l * (finishMount.position.x - startMount.position.x) + startMount.position.x, this.transform.position.y) ;

        if(_gameController.distanceTraveled > checkpoints[nextCheckpoint].distance)
        {
            if (nextCheckpoint == checkpoints.Count - 1)
            {
                WinLevel();
            }
            else
            {
                _gameController.EnterCheckpoint();
            }            
        }
    }

    private void WinLevel()
    {
        _gameController.EndLevel(LevelEndReason.LAST_CHECKPOINT);
    }
}

[System.Serializable]
public class Checkpoint
{
    [SerializeField]
    public float distance;
    [SerializeField]
    public float time;
    public float location;
}

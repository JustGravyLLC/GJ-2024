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
    public List<float> checkpoints;
    private int nextCheckpoint = 0;

    public void Initialize()
    {
        _gameController = GameObject.FindFirstObjectByType<GameController>();

        float lastCheckpoint = checkpoints[checkpoints.Count - 1];

        //for each checkpoint, make a marker
        foreach(float f in checkpoints)
        {
            GameObject marker = Instantiate<GameObject>(checkpointMarker);
            marker.transform.parent = this.transform;

            float x = f / lastCheckpoint;
            x = x * (finishMount.position.x - startMount.position.x);

            marker.transform.position = new Vector2(x + startMount.position.x, this.transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update avatar position based on distance traveled
        float l = _gameController.distanceTraveled / checkpoints[checkpoints.Count - 1];
        avatar.position = new Vector2(l * (finishMount.position.x - startMount.position.x) + startMount.position.x, this.transform.position.y) ;

        if(_gameController.distanceTraveled > checkpoints[nextCheckpoint])
        {
            Debug.Log("Checkpoint " + nextCheckpoint + " hit.");
            nextCheckpoint++;
        }
    }
}

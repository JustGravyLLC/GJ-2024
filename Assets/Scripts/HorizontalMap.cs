using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMap : MonoBehaviour
{
    [SerializeField]
    private Transform startMount, finishMount;
    [SerializeField]
    private Transform avatar;
    private GameController _gameController;

    public GameObject checkpointMarker;
    public List<float> checkpoints;

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

            marker.transform.position = new Vector2(x, this.transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update avatar position based on distance traveled

    }
}

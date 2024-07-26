using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	private PlayerCharacter _player;
	[SerializeField]
	private Spawner _spawner;
	[SerializeField]
	private CheckpointMap _map;
	[SerializeField]
	private Timer _timer;
	[SerializeField]
	private List<MeshRenderer> _scrollingTerrainMeshes;

	private const float _terrainSpeedScalar = 0.1f;

	public float distanceTraveled { get; private set; } = 0f;

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	private void Start() {
		InitGame();
		StartLevel();
	}

	private void Update() {
		if (_player.movementEnabled) {
			foreach (MeshRenderer mesh in _scrollingTerrainMeshes) {
				mesh.material.mainTextureOffset += new Vector2(0f, -_player.forwardVelocity * Time.deltaTime * _terrainSpeedScalar);
			}
			distanceTraveled += _player.forwardVelocity * Time.deltaTime;
		}
	}

	private void InitGame() {
		_spawner.Initialize();
		_timer.Initialize();
		_map.Initialize();
		NextCheckpoint(_map.checkpoints[0]);
    }

	private void StartLevel() {
		_player.movementEnabled = true;
		distanceTraveled = 0f;
	}
	public void EndLevel(LevelEndReason reason)
    {

    }

	private void EnterShop() {
		_player.movementEnabled = false;
		//Show shop UI
	}

	public void NextCheckpoint(Checkpoint nextCP){
		_timer.SetCheckpoint(nextCP);
	}
}

public enum LevelEndReason
{
	OUT_OF_TIME,
	LAST_CHECKPOINT
}
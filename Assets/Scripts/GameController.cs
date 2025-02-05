using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	private PlayerCharacter _player;
	public PlayerCharacter playerCharacter => _player;
	[SerializeField]
	private Spawner _spawner;
	[SerializeField]
	private UIController _UI;
	[SerializeField]
	private CheckpointMap _map;
	[SerializeField]
	private CheckpointPanel _checkpointPanel;
	[SerializeField]
	private Timer _timer;
	public Timer timer => _timer;
	[SerializeField]
	private List<MeshRenderer> _scrollingTerrainMeshes;
	private GameState _currentState = GameState.LOADING;
	public GameState currentState => _currentState;
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
		_UI.Initialize();
		_map.Initialize();
		_checkpointPanel.Initialize();
	}

	private void StartLevel() {
		_timer.StartLevel(_map.checkpoints[0]);
		_player.movementEnabled = true;
		distanceTraveled = 0f;
		_currentState = GameState.RUNNING;
	}
	public void EndLevel(LevelEndReason reason){
		PauseRunner();
		_UI.ShowEndPanel(reason);
	}

	private void PauseRunner(GameState state = GameState.MENU) {
		_player.movementEnabled = false;
		_currentState = state;
	}

	private void UnpauseRunner() {
		_player.movementEnabled = true;
		_currentState = GameState.RUNNING;
	}

	public void EnterEvent(string text)
    {
		PauseRunner();
		_UI.ShowEventPanel(text);
    }
	public void EnterEvent()
	{
		PauseRunner();
		_UI.ShowEventPanel();
	}

	public void ExitEvent()
    {
		UnpauseRunner();
    }

	public void EnterCheckpoint() {
		PauseRunner();
		_checkpointPanel.OpenPanel();
    }

	public void ExitCheckpoint() {
		_map.nextCheckpoint++;
		_timer.NewCheckpoint(_map.checkpoints[_map.nextCheckpoint]);
		UnpauseRunner();
	}

	public void RestartGame()
    {
		_spawner.Restart();
		_UI.Restart();
		_map.Restart();
		StartLevel();
    }
}

public enum GameState
{
	LOADING,
	RUNNING,
	EVENT,
	MENU
}
public enum LevelEndReason
{
	OUT_OF_TIME,
	LAST_CHECKPOINT
}
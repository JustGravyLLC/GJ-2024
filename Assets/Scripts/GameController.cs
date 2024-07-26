using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	private PlayerCharacter _player;
	[SerializeField]
	private List<MeshRenderer> _scrollingTerrainMeshes;

	private const float _terrainSpeedScalar = 0.1f;

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	private void Start() {
		StartLevel();
	}

	private void Update() {
		foreach (MeshRenderer mesh in _scrollingTerrainMeshes) {
			mesh.material.mainTextureOffset += new Vector2(0f, -_player.forwardVelocity * Time.deltaTime * _terrainSpeedScalar);
		}
	}

	private void StartLevel() {
		_player.movementEnabled = true;
	}

	private void EnterShop() {
		_player.movementEnabled = false;
		//Show shop UI
	}
}

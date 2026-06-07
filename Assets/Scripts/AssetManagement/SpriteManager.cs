using System.Collections.Generic;
using UnityEngine;

public class SpriteManager {

	private static SpriteManager _instance;
	public static SpriteManager Instance {
		get {
			if(_instance == null) {
				_instance = new();

				AssetManager.Instance.LoadSprites("enemy_sprites", _instance.RegisterEnemySprites);
				AssetManager.Instance.LoadSprites("enemy_sprites", _instance.RegisterPlayerSprites);
				AssetManager.Instance.LoadSprites("spell_sprites", _instance.RegisterRelicSprites);
				AssetManager.Instance.LoadSprites("spell_sprites", _instance.RegisterSpellSprites);
			}

			return _instance;
		}
	}

	// TODO - find a better way to do this. currently we're
	// hardcoding the indices of the sprites that we want to
	// load for the spells.
	private static int[] _spellSpriteSheetIndices = {
		1910,
		1908,
		1915,
		1911,
		1906,
		2002,
		1951,
		1998,
		2005,
		2027,
		2031,
		2037,
		2039,
		2041,
		2130,
		2132,
		2198,
	};

	private static int[] _playerSpriteSheetIndices = {
		65,
		101,
		109,
	};

	private static int[] _relicSpriteSheetIndices = {
		823,
		865,
		1879,
		1896,
		2388,
		2343,
		828
	};

	private Sprite[] _enemySprites;
	private Sprite[] _playerSprites;
	private Sprite[] _relicSprites;
	private Sprite[] _spellSprites;

	private void RegisterEnemySprites(Sprite[] sprites) {
		_enemySprites = sprites;
	}

	private void RegisterPlayerSprites(Sprite[] sprites) {
		List<Sprite> selectedSprites = new();
		foreach(int index in _playerSpriteSheetIndices) {
			selectedSprites.Add(sprites[index]);
		}

		_playerSprites = selectedSprites.ToArray();
	}

	private void RegisterRelicSprites(Sprite[] sprites) {
		List<Sprite> selectedSprites = new();
		foreach(int index in (_relicSpriteSheetIndices)) {
			selectedSprites.Add(sprites[index]);
		}

		_relicSprites = selectedSprites.ToArray();
	}

	private void RegisterSpellSprites(Sprite[] sprites) {
		List<Sprite> selectedSprites = new();
		foreach(int index in _spellSpriteSheetIndices) {
			selectedSprites.Add(sprites[index]);
		}

		_spellSprites = selectedSprites.ToArray();
	}

	public Sprite RetrieveEnemySprite(int index) {
		return _enemySprites[index];
	}

	public Sprite RetrievePlayerSprite(int index) {
		return _playerSprites[index];
	}

	public Sprite RetrieveRelicSprite(int index) {
		return _relicSprites[index];
	}

	public Sprite RetrieveSpellSprite(int index) {
		return _spellSprites[index];
	}

}


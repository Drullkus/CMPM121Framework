using UnityEngine;

public class SpriteManager {

	private static SpriteManager _instance;
	public static SpriteManager Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	private void Awake() {
		AssetManager.Instance.LoadSprites("enemy_sprites", RegisterEnemySprites);
		AssetManager.Instance.LoadSprites("player_sprites", RegisterPlayerSprites);
		AssetManager.Instance.LoadSprites("relic_sprites", RegisterRelicSprites);
		AssetManager.Instance.LoadSprites("spell_sprites", RegisterSpellSprites);
	}

	private Sprite[] _enemySprites;
	private Sprite[] _playerSprites;
	private Sprite[] _relicSprites;
	private Sprite[] _spellSprites;

	private void RegisterEnemySprites(Sprite[] sprites) {
		_enemySprites = sprites;
	}

	private void RegisterPlayerSprites(Sprite[] sprites) {
		_playerSprites = sprites;
	}

	private void RegisterRelicSprites(Sprite[] sprites) {
		_relicSprites = sprites;
	}

	private void RegisterSpellSprites(Sprite[] sprites) {
		_spellSprites = sprites;
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


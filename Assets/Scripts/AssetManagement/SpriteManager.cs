using UnityEngine;

public class SpriteManager : MonoBehaviour {

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

	// TODO
	private void RegisterEnemySprites(Sprite[] sprites) { }

	// TODO
	private void RegisterPlayerSprites(Sprite[] sprites) { }

	// TODO
	private void RegisterRelicSprites(Sprite[] sprites) { }

	// TODO
	private void RegisterSpellSprites(Sprite[] sprites) { }

	// TODO
	public Sprite RetrieveEnemySprite(int index) { return null; }

	// TODO
	public Sprite RetrievePlayerSprite(int index) { return null; }

	// TODO
	public Sprite RetrieveRelicSprite(int index) { return null; }

	// TODO
	public Sprite RetrieveSpellSprite(int index) { return null; }

}


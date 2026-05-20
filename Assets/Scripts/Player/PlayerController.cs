using UnityEngine;
using UnityEngine.InputSystem;
using Player;

public class PlayerController : MonoBehaviour {

    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUIContainer SpellUIContainer;
    public SpellUI spellui;
    public GameObject defeatScreen;

    public PlayerClassData data;

    public int speed;

    public Unit unit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;

        this.data = new();
        
        spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spell);

        gameObject.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrievePlayerSprite(data.sprite);
    }

    public void OnNextWave(int wave) {
        data.CalculatePlayerStatsForWave(wave, out int health, out int mana, out int mana_reg, out int spellpower, out int speed); // FIXME spellpower unused, needs wiring into spellcasting
        
        hp.hp = health;
        spellcaster.mana = mana;
        spellcaster.mana_reg = mana_reg;
        this.speed = speed;
    }

    void OnAttack(InputValue value) {
        // FIXME remove GameManager.GameState.COUNTDOWN from among the checks and find out what's actually null
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.COUNTDOWN || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;

		EventBus.Instance.InvokePlayerShoot();

        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value) {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) { return; }

        unit.movement = value.Get<Vector2>() * speed;
    }

    void OnChangeSpell(InputValue value) {
        SpellUIContainer.ChangeSpell();
    }

    void Die() {
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        defeatScreen.SetActive(true);
    }

}

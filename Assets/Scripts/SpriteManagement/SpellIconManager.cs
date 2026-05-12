using UnityEngine;

public class SpellIconManager : IconManager {

    void Start() {
        GameManager.Instance.spellIconManager = this;
    }

}

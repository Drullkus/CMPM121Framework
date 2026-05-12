using UnityEngine;

public class EnemySpriteManager : IconManager {

    void Start() {
        GameManager.Instance.enemySpriteManager = this;
    }

    void Update() {
        
    }

}

using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteManager : IconManager {

    void Start()
    {
        GameManager.Instance.playerSpriteManager = this;
    }

}


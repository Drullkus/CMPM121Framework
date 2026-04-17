using UnityEngine;

public class LevelLoader
{
    public static void LoadLevels()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("levels");
        Debug.Log(jsonAsset.text);
    }
}
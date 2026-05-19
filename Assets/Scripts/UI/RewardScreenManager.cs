using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardScreenManager : MonoBehaviour
{

    [SerializeField]
    private GameObject rewardUI;

    [SerializeField]
    private GameObject statDisplayPrefab;

    [SerializeField]
    private SpellUIContainer playerSpellContainer;

    [SerializeField]
    private GameObject spellChoicePrefab;

    private List<GameObject> displayObjects = new List<GameObject>();

    void Start() {
        GameManager.Instance.rewardScreenManager = this;
    }

    public void ShowRewardScreen(List<string> stats, bool wavesCompleted)
    {
        if (wavesCompleted)
        {
            GameObject newStatDisplay = Instantiate(statDisplayPrefab, rewardUI.transform);
            newStatDisplay.transform.localPosition = new Vector3(0, 0, 0);

            TextMeshProUGUI textObject = newStatDisplay.GetComponent<TextMeshProUGUI>();
            textObject.text = "U WINNER";

            displayObjects.Add(newStatDisplay);
        }
        else
        {
            float statSpacing = 40; // TODO make this depend on stats.Count
            float statStartingYOffset = 190 - statSpacing * stats.Count;

            for (int i = 0; i < stats.Count; i++)
            {
                GameObject newStatDisplay = Instantiate(statDisplayPrefab, rewardUI.transform);
                newStatDisplay.transform.localPosition = new Vector3(0, statStartingYOffset - statSpacing * i, 0);

                TextMeshProUGUI textObject = newStatDisplay.GetComponent<TextMeshProUGUI>();
                textObject.text = stats[i];

                displayObjects.Add(newStatDisplay);
            }
            
            // display spell reward
            InstantiateSpellReward(-96);
        }

        rewardUI.SetActive(true);

        void InstantiateSpellReward(float xPos)
        {
            var newSpellChoice = Instantiate(spellChoicePrefab, rewardUI.transform);
            newSpellChoice.transform.localPosition = new Vector3(xPos, 0, 0);
            var imageComponent = newSpellChoice.GetComponent<Image>();
            imageComponent.sprite = GameManager.Instance.spellIconManager.Get(0);
                
            displayObjects.Add(newSpellChoice);
        }
    }

    public void HideRewardScreen()
    {
        rewardUI.SetActive(false);

        foreach(GameObject statDisplay in displayObjects)
        {
            Destroy(statDisplay);
        }
    }

}

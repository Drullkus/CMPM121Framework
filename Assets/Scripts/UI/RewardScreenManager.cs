using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RewardScreenManager : MonoBehaviour
{

    [SerializeField]
    private GameObject rewardUI;

    [SerializeField]
    private GameObject statDisplayPrefab;

    [SerializeField]
    private SpellUIContainer playerSpellContainer;

    private List<GameObject> statDisplays = new List<GameObject>();

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

            statDisplays.Add(newStatDisplay);
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

                statDisplays.Add(newStatDisplay);
            }
        }

        rewardUI.SetActive(true);
    }

    public void HideRewardScreen()
    {
        rewardUI.SetActive(false);

        foreach(GameObject statDisplay in statDisplays)
        {
            Destroy(statDisplay);
        }
    }

}

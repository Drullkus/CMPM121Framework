using JetBrains.Annotations;
using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;
    public int activeSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we only have one spell (right now)
        spellUIs[0].SetActive(true);
        for(int i = 1; i< spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(false);
        }
    }

    void EnableSpell(int slot) {
        spellUIs[slot].SetActive(true);
    }

    [CanBeNull]
    public GameObject GetSpellUiElement(int slot) {
        return transform.Find($"spell{slot}")?.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSpell() {
        activeSlot = (activeSlot + 1) % spellUIs.Length;
        if (!spellUIs[activeSlot].activeSelf) {
            activeSlot = 0;
        }
    }

}

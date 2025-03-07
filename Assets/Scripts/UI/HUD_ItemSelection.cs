using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ItemSelection : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;

    [SerializeField] private Image slotBorder1;
    [SerializeField] private Image slotBorder2;
    [SerializeField] private Image slotBorder3;

    [SerializeField] private Image lockEffect1;
    [SerializeField] private Image lockEffect2;
    [SerializeField] private Image lockEffect3;

    [Header("Tool Setup Menu")]
    [SerializeField] private Image menuSlot1;
    [SerializeField] private Image menuSlot2;
    [SerializeField] private Image menuSlot3;

    [SerializeField] private Image menuSlotBorder1;
    [SerializeField] private Image menuSlotBorder2;
    [SerializeField] private Image menuSlotBorder3;

    //Update HUD based on Player Items
    public void UpdateHUD(GameObject item1, GameObject item2, GameObject item3)
    {
        menuSlot1.GetComponent<Animator>().Play("Random", 0, 0f);
        menuSlot2.GetComponent<Animator>().Play("Random", 0, 10f);
        menuSlot3.GetComponent<Animator>().Play("Random", 0, 20f);

        // Menu
        SpriteRenderer menuItem1Renderer = item1.GetComponent<SpriteRenderer>();
        menuSlot1.sprite = menuItem1Renderer.sprite;
        //menuSlot1.color = menuItem1Renderer.color;

        

        SpriteRenderer menuItem2Renderer = item2.GetComponent<SpriteRenderer>();
        menuSlot2.sprite = menuItem2Renderer.sprite;
        menuSlot2.color = menuItem2Renderer.color;

        SpriteRenderer menuItem3Renderer = item3.GetComponent<SpriteRenderer>();
        menuSlot3.sprite = menuItem3Renderer.sprite;
        menuSlot3.color = menuItem3Renderer.color;

        // Gamplay
        SpriteRenderer item1Renderer = item1.GetComponent<SpriteRenderer>();
        slot1.sprite = item1Renderer.sprite;
        slot1.color = item1Renderer.color;

        Debug.Log(item1.name);

        SpriteRenderer item2Renderer = item2.GetComponent<SpriteRenderer>();
        slot2.sprite = item2Renderer.sprite;
        slot2.color = item2Renderer.color;

        Debug.Log(item2.name);

        SpriteRenderer item3Renderer = item3.GetComponent<SpriteRenderer>();
        slot3.sprite = item3Renderer.sprite;
        slot3.color = item3Renderer.color;

        Debug.Log(item3.name);
    }

    public void UpdateCooldownUI(float item1CooldownProgress, float item2CooldownProgress, float item3CooldownProgress)
    {
        lockEffect1.fillAmount = item1CooldownProgress;
        lockEffect2.fillAmount = item2CooldownProgress;
        lockEffect3.fillAmount = item3CooldownProgress;
    }

    public void Start()
    {
        slotBorder1.color = Color.red;
    }

    public void SelectOne()
    {
        slotBorder1.color = Color.red;
        slotBorder2.color = Color.black;
        slotBorder3.color = Color.black;
    }

    public void SelectTwo()
    {
        slotBorder1.color = Color.black;
        slotBorder2.color = Color.red;
        slotBorder3.color = Color.black;
    }

    public void SelectThree()
    {
        slotBorder1.color = Color.black;
        slotBorder2.color = Color.black;
        slotBorder3.color = Color.red;
    }
}

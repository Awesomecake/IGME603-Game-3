using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ItemSelection : MonoBehaviour
{
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;

    [SerializeField] private Image slotBorder1;
    [SerializeField] private Image slotBorder2;
    [SerializeField] private Image slotBorder3;

    //Update HUD based on Player Items
    public void UpdateHUD(GameObject item1, GameObject item2, GameObject item3)
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class HUD_ItemSelection : MonoBehaviour
{
    [Header("Tool Random Effect")]
    [SerializeField] private GameObject[] tools;
    [SerializeField] private float randomizeInterval = 0.1f;
    [SerializeField] private float menuSlot1RandomizeDelay;
    [SerializeField] private float menuSlot2RandomizeDelay;
    [SerializeField] private float menuSlot3RandomizeDelay;
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

    public void StartToolSetup(GameObject item1, GameObject item2, GameObject item3)
    {
        StartCoroutine(ToolSetup(menuSlot1, menuSlotBorder1, menuSlot1RandomizeDelay, item1));
        StartCoroutine(ToolSetup(menuSlot2, menuSlotBorder2, menuSlot2RandomizeDelay, item2));
        StartCoroutine(ToolSetup(menuSlot3, menuSlotBorder3, menuSlot3RandomizeDelay, item3));

    }

    private IEnumerator ToolSetup(Image menuSlot, Image menuSlotBorder, float delay, GameObject finalItem)
    {
        float elapsedTime = 0f;

        GameObject previousTool = null;
        // Randomize sprites until the delay is reached
        while (elapsedTime < delay)
        {
            // Get a random tool from the tools array
            GameObject randomTool = tools[Random.Range(0, tools.Length)];
            do // Make sure it's a different tool
            {
                randomTool = tools[Random.Range(0, tools.Length)];
            } while (randomTool == previousTool);
            previousTool = randomTool;

            SpriteRenderer randomToolRenderer = randomTool.GetComponent<SpriteRenderer>();

            // Set Tool
            menuSlot.sprite = randomToolRenderer.sprite;
            menuSlot.color = randomToolRenderer.color;

            // Wait for the next frame
            yield return new WaitForSeconds(randomizeInterval);

            // Update elapsed time
            elapsedTime += randomizeInterval;
        }

        // After the delay, set the final sprite
        SpriteRenderer finalItemRenderer = finalItem.GetComponent<SpriteRenderer>();
        menuSlot.sprite = finalItemRenderer.sprite;
        menuSlot.color = finalItemRenderer.color;

        /*
        menuSlotBorder.transform.DOScale(1.2f, 0.5f) // Scale up to 1.2x size over 0.5 seconds
        .SetEase(Ease.OutBack) // Add a bounce effect
        .OnComplete(() =>
        {
            // Optional: Scale back down after the animation
            menuSlotBorder.transform.DOScale(1f, 0.3f); // Scale back to normal size
        });
        */
    }

    //Update HUD based on Player Items
    public void UpdateHUD(GameObject item1, GameObject item2, GameObject item3)
    {
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

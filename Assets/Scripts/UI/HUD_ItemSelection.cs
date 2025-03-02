using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ItemSelection : MonoBehaviour
{
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;

    public List<GameObject> items = new List<GameObject>();

    private PlayerController player;

    //Select Three items and display them on the HUD
    void Start()
    {
        //Select the items and update the HUD
        List<GameObject> startSelectionList = new List<GameObject>(items);

        GameObject item1 = startSelectionList[Random.Range(0, startSelectionList.Count)];
        startSelectionList.Remove(item1);
        SpriteRenderer item1Renderer = item1.GetComponent<SpriteRenderer>();
        slot1.sprite = item1Renderer.sprite;
        slot1.color = item1Renderer.color;

        Debug.Log(item1.name);

        GameObject item2 = startSelectionList[Random.Range(0, startSelectionList.Count)];
        startSelectionList.Remove(item2);
        SpriteRenderer item2Renderer = item2.GetComponent<SpriteRenderer>();
        slot2.sprite = item2Renderer.sprite;
        slot2.color = item2Renderer.color;

        Debug.Log(item2.name);

        GameObject item3 = startSelectionList[Random.Range(0, startSelectionList.Count)];
        startSelectionList.Remove(item3);
        SpriteRenderer item3Renderer = item3.GetComponent<SpriteRenderer>();
        slot3.sprite = item3Renderer.sprite;
        slot3.color = item3Renderer.color;

        Debug.Log(item3.name);

        //Find Player and update their items from the selection

        player = FindObjectOfType<PlayerController>();
        player.item1 = item1;
        player.item2 = item2;
        player.item3 = item3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

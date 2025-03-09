using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerItemPool", menuName = "GameData/PlayerItemPool", order = 0)]
public class PlayerItemPool : ScriptableObject
{
    public List<GameObject> itemPrefabs = new();

    public GameObject RandomItem()
    {
        if (itemPrefabs.Count <= 0) return null;
        return itemPrefabs[Random.Range(0, itemPrefabs.Count)];
    }
}
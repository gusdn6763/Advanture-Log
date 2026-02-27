using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Loot
{
    [SerializeField] private int itemId;
    [Range(0f, 100f)]
    [SerializeField] private float dropChance;

    public int ItemId { get => itemId; }
    public float DropChance { get => dropChance; }
}

public class LootTable : MonoBehaviour
{
    [SerializeField] private List<Loot> loots = new List<Loot>();
    private List<int> droppedItems = new List<int>();

    public List<int> DroppedItems { get { return droppedItems; } set { droppedItems = value; } }

    //·çÆÃ È®·ü  
    public void RollLoot()
    {
        foreach (Loot loot in loots)
        {
            int roll = Random.Range(0, 100);

            if (roll <= loot.DropChance)
                DroppedItems.Add(loot.ItemId);
        }
    }
}

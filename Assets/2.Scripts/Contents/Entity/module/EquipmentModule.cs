using System.Collections.Generic;

public class EquipmentModule
{
    private ResourceModule stats;
    private Dictionary<EquipmentSlotType, EquipmentEntity> equipped = new Dictionary<EquipmentSlotType, EquipmentEntity>();
    public EquipmentModule(ResourceModule stats)
    {
        this.stats = stats;
    }

    public bool Equip(EquipmentEntity equipItem)
    {
        // ItemInfo = equipItem.equipItemSO;

        //if (equipped.TryGetValue(ItemInfo.Slot, out EquipmentEntity equippedItem))
        //    Unequip(equippedItem);

        //equipped[ItemInfo.Slot] = equipItem;

        ////1) 스탯 번들(StatData) 적용
        //foreach (KeyValuePair<StatType, ModifierSpec> stat in ItemInfo.Modifiers)
        //    stats.AddModifier(stat.Key, new StatModifier(stat.Value.value, stat.Value.CalculateType, equipItem));

        //foreach (KeyValuePair<ElementStatType, ElementRates> element in ItemInfo.Element)
        //{
        //    stats.AddAffinityModifier(element.Key, new StatModifier(element.Value.affinity, CalculateType.Flat, equipItem));
        //    stats.AddResistanceModifier(element.Key, new StatModifier(element.Value.resistance, CalculateType.Flat, equipItem));
        //}

        return true;
    }

    public bool Unequip(EquipmentEntity equipItem)
    {
        //if (item == null || !_handles.TryGetValue(item, out var handle))
        //    return false;
        //item.MyStatBundle.Remove(_stats, handle);
        //_handles.Remove(item);
        return true;
    }

    public void UnequipAll()
    {
        //foreach (var kv in _handles)
        //    (kv.Key as EquipItemSO)?.MyStatBundle.Remove(_stats, kv.Value);
        //_handles.Clear();
    }
}
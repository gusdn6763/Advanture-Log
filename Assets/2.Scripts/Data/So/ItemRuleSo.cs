using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Rule/ItemRule", fileName = "ItemRuleSo")]
public class ItemRuleSo : ScriptableObject
{
	[Header("등급별 색상")]
	[SerializeField] private SerializedDictionary<ItemRarity, Color> itemRarityColorDic = new SerializedDictionary<ItemRarity, Color>();

	[Header("기본값")]
    [SerializeField] private Color defaultColor = Color.white;

	public Color GetItemNameColor(ItemRarity rarity)
	{
		if (itemRarityColorDic.TryGetValue(rarity, out Color color))
			return color;

		return defaultColor;
	}
}
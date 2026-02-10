using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct TabItem
{
	public UI_TabPage tabPage;
	public GameObject page;
}

public class UI_Setting : MonoBehaviour
{
	[SerializeField] private List<TabItem> tabs = new();

	[Header("Visuals")]
	[SerializeField] private Sprite activeButtonSprite;
	[SerializeField] private Sprite defaultButtonSprite;

	private int currentIndex = -1;

	private void Start()
	{
		for (int i = 0; i < tabs.Count; i++)
		{
            UI_TabPage tabPage = tabs[i].tabPage;

			tabPage.ButtonImage.sprite = defaultButtonSprite;
			tabPage.Button.onClick.RemoveAllListeners();
			int idx = i;
			tabPage.Button.onClick.AddListener(() => Select(idx));
		}
	}


	public void Open()
	{
		gameObject.SetActive(true);

		if (tabs.Count > 0)
			Select(0);
	}

	public void Select(int index)
	{
		if (index == currentIndex)
			return;

		for (int i = 0; i < tabs.Count; i++)
		{
			if (i == index)
			{
				tabs[i].page.gameObject.SetActive(true);
				tabs[i].tabPage.ButtonImage.sprite = activeButtonSprite;
			}
			else
			{
				tabs[i].page.gameObject.SetActive(false);
				tabs[i].tabPage.ButtonImage.sprite = defaultButtonSprite;
			}
		}

		currentIndex = index;
	}
}
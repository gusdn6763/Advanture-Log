using System.Collections.Generic;
using UnityEngine;

public class UI_SkillInfo : MonoBehaviour
{
    [SerializeField] private UI_SkillIcon jobSkillPrefab;

    [SerializeField] private int poolingCount = 3;

    private List<UI_SkillIcon> skillList = new List<UI_SkillIcon>();

    public void Init()
    {
        for (int i = 0; i < poolingCount; i++)
            CreateIcon();
    }

    public void CreateIcon()
    {
        UI_SkillIcon icon = Instantiate(jobSkillPrefab, transform);

        icon.gameObject.SetActive(false);

        skillList.Add(icon);
    }

    public void Refresh()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            UI_SkillIcon icon = skillList[i];

            icon.gameObject.SetActive(false);
        }
    }

    public void ShowSkillInfo(IReadOnlyList<SkillSo> playerSkillList)
    {
        //원래 메뉴보다 많으면 생성
        while (playerSkillList.Count > skillList.Count)
            CreateIcon();

        for (int i = 0; i < playerSkillList.Count; i++)
        {
            UI_SkillIcon icon = skillList[i];

            icon.Init(playerSkillList[i]);
            icon.gameObject.SetActive(true);
        }
    }
}
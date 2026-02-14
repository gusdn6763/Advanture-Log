using TMPro;
using UnityEngine;

public class UI_JobInfo : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI jobDescriptionText;

    [Header("Base Stat")]
    [SerializeField] private UI_SubStatIntroduce subStatIntroduce;

    [Header("Skills")]
    [SerializeField] private UI_SkillInfo skillInfo;

    public void Init()
    {
        subStatIntroduce.Init();
        skillInfo.Init();
    }

    #region 외부 호출 - UI_JobButton버튼
    public void Refresh()
    {
        jobDescriptionText.text = string.Empty;
        subStatIntroduce.Refresh(string.Empty);

        skillInfo.Refresh();
    }

    public void ShowJobInfo(PlayerSo playerSo)
    {
        jobDescriptionText.text = playerSo.JobDescription.GetLocalizedString();

        subStatIntroduce.Refresh(playerSo.BaseSubStatDic);

        skillInfo.ShowSkillInfo(playerSo.BaseSkillList);
    }
    #endregion
}
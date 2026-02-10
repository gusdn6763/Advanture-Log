using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class UI_PlayerNameSetting : MonoBehaviour, ICharacterCreationSection
{
    [SerializeField] private TMP_InputField nameInputText;
    [SerializeField] private TextMeshProUGUI warningText;

    #region ICharacterCreationSection
    public void Refresh()
    {
        nameInputText.text = string.Empty;
        warningText.text = string.Empty;
    }

    public bool IsValid()
    {
        string name = nameInputText.text.Trim();

        if (string.IsNullOrEmpty(name))
            warningText.text = LocalizeUtil.UI(LocalizeKey.UI_NameEmpty.ToString());
        else if (name.Length < 2)
            warningText.text = LocalizeUtil.UI(LocalizeKey.UI_NameTooShort.ToString());
        else if (!Regex.IsMatch(name, @"^[a-zA-Z0-9°¡-ÆR]+$"))
            warningText.text = LocalizeUtil.UI(LocalizeKey.UI_InvalidCharacters.ToString());
        else
            warningText.text = string.Empty;

        if (!string.IsNullOrEmpty(warningText.text))
            return false;

        return true;
    }

    public void Apply(GameStartData data)
    {
        data.playerName = nameInputText.text;
	}
    #endregion
}
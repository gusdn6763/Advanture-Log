using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class UI_PlayerNameSetting : MonoBehaviour, ICharacterCreationSection
{
    [SerializeField] private TMP_InputField nameInputText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private LocalizedString nameEmpty;
    [SerializeField] private LocalizedString nameTooShort;
    [SerializeField] private LocalizedString invalidCharacters;

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
            warningText.text = nameEmpty.GetLocalizedString();
        else if (name.Length < 2)
            warningText.text = nameTooShort.GetLocalizedString();
        else if (!Regex.IsMatch(name, @"^[a-zA-Z0-9°¡-ÆR]+$"))
            warningText.text = invalidCharacters.GetLocalizedString();
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
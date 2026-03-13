using UnityEngine;
using UnityEngine.Localization.Settings;

public class MainSceneStarter : MonoBehaviour
{
    [SerializeField] private UI_Title titlePrefab;
    [SerializeField] private Managers managerPrefab;

    private void Awake()
    {
        LocalizationSettings.InitializationOperation.Completed += _ =>
        {
            Instantiate(managerPrefab).Init();
            Instantiate(titlePrefab).Init();
        };
    }
}
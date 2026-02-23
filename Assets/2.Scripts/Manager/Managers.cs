using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get { Init(); return instance; } }

    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private SettingManager settingManager;
    [SerializeField] private AreaManager areaManager;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UiManager uiManager;

    public static DataManager Data => Instance?.dataManager;
    public static SaveManager Save => Instance?.saveManager;
    public static SettingManager Setting => Instance?.settingManager;
    public static AreaManager Area => Instance?.areaManager;
    public static ObjectManager Object => Instance?.objectManager;
    public static GameManager Game => Instance?.gameManager;
    public static UiManager UI => Instance?.uiManager;

    public static void Init()
    {
        if (instance != null)
            return;

        // 1) 씬에 이미 배치된 Managers를 찾음
        instance = FindFirstObjectByType<Managers>();

        if (instance == null)
        {
            Debug.LogError("Managers 없음");
            return;
        }

        DontDestroyOnLoad(instance.gameObject);
        Application.targetFrameRate = 60;

        Data.Init();
        Save.Init();
        Setting.Init();
        Area.Init();
        Game.Init();
        UI.Init();
    }
}
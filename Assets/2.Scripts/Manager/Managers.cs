using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get { Init(); return instance; } }

    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private KeyManager keyManager;
    [SerializeField] private AreaManager areaManager;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private SaveManager saveManager;

    public static GameManager Game => Instance?.gameManager;
    public static SoundManager Sound => Instance?.soundManager;
    public static UiManager UI => Instance?.uiManager;
    public static KeyManager Key => Instance?.keyManager;
    public static AreaManager Area => Instance?.areaManager;
    public static DataManager Data => Instance?.dataManager;
    public static ObjectManager Object => Instance?.objectManager;
    public static SaveManager Save => Instance?.saveManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
    }

    public static void Init()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<Managers>();

            if (instance != null)
                return;

            Managers managerPrefab = Resources.Load<Managers>("Managers");

            if (managerPrefab == null)
            {
                Debug.LogError("[Managers] Resources.Load failed. Assets/Resources/Managers.prefab");
                return;
            }

            instance = Instantiate(managerPrefab);
            instance.name = "Managers";
        }
    }
}
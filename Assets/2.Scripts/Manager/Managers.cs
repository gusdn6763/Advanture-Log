using System.Collections;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get { Init(); return instance; } }

    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private KeyManager keyManager;

    private PoolManager poolManager = new PoolManager();

    public static GameManager Game => Instance?.gameManager;
    public static SoundManager Sound => Instance?.soundManager;
    public static ObjectManager Object => Instance?.objectManager;
    public static UiManager UI => Instance?.uiManager;
    public static KeyManager Key => Instance?.keyManager;
    public static PoolManager Pool => Instance?.poolManager;

    private void Awake()
    {
        Init();
    }
    private static void Init()
    {
        if (instance != null) 
            return;

        GameObject go = GameObject.Find("Managers");
        if (go == null)
            go = new GameObject("Managers");

        if (!go.activeSelf) 
            go.SetActive(true);

        instance = go.GetOrAddComponent<Managers>();

        DontDestroyOnLoad(go);
        Application.targetFrameRate = 60;
    }
}
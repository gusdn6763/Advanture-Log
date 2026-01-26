using System.Collections;
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
    private PoolManager poolManager = new PoolManager();


    public static GameManager Game => Instance?.gameManager;
    public static SoundManager Sound => Instance?.soundManager;
    public static ObjectManager Object => Instance?.objectManager;
    public static PoolManager Pool => Instance?.poolManager;

    private void Awake()
    {
        Init();
    }
    private static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }
            instance = go.GetComponent<Managers>();
            DontDestroyOnLoad(instance);
            Application.targetFrameRate = 60;
        }
    }
}
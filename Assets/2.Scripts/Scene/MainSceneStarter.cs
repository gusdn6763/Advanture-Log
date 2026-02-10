using UnityEngine;

public class MainSceneStarter : MonoBehaviour
{
    [SerializeField] private UI_Title titlePrefab;

    private void Awake()
    {
        Managers.Init();

        UI_Title title = Instantiate(titlePrefab);
        title.Init();
    }
}
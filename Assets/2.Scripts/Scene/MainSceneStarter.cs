using UnityEngine;

public class MainSceneStarter : MonoBehaviour
{
    [SerializeField] private UI_Title titlePrefab;
    [SerializeField] private Managers managerPrefab;

    private void Awake()
    {
        Instantiate(managerPrefab).Init();
        Instantiate(titlePrefab).Init();
    }
}
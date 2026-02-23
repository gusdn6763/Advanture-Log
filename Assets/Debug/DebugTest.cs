using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DebugStarter : MonoBehaviour, ILoadProgress
{
    [SerializeField] public GameSceneBootstrap b;
    [SerializeField] public GameStartData startData = new GameStartData();
    public AreaSo startArea;

    private void Awake()
    {
#if DEBUG
        Managers.Init();
        StartCoroutine(b.Initialize(this, startData));
#else
        gameObject.SetActive(false);
#endif
    }

    public void UpdateMessage(string message)
    {
        Debug.Log(message);
    }

    public void UpdateProgress(float value01)
    {

    }

    public BaseEntitySo appleSo;
    public void CreateApple() => CreateEntity(appleSo, _ => { });

    public BaseEntitySo goblinSo;
    public void CreateGoblin() => CreateEntity(goblinSo, _ => { });

    public BaseEntitySo player;
    public void CreatePlayer() => CreateEntity(player, _ => { });

    public void CreateEntity(BaseEntitySo so, System.Action<BaseEntity> onCreated)
    {
        BaseEntity entity = Managers.Object.Spawn<BaseEntity>(Vector2.zero, so.Id);

        onCreated?.Invoke(entity);
    }
}
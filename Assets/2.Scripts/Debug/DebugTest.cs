using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DebugTest : MonoBehaviour, ILoadProgress
{
    [SerializeField] public GameSceneBootstrap b;
    public AreaController a;
    public AreaSo startArea;

    public BaseEntitySo player;

    private void Awake()
    {
        StartCoroutine(GameStart());
    }

    public IEnumerator GameStart()
    {
        yield return b.Initialize(this);

        AssetReferenceGameObject aref = player.EntityPrefab;

        AsyncOperationHandle<GameObject> instHandle = aref.InstantiateAsync(Vector3.zero, Quaternion.identity, transform);

        yield return instHandle;

        BaseEntity e = instHandle.Result.GetOrAddComponent<BaseEntity>();

        Managers.Object.Player = e;
        Managers.Object.ActiveEntities.Add(e);

        a.EnterArea(startArea.AreaId);
    }

    public void UpdateMessage(string message)
    {

    }

    public void UpdateProgress(float value01)
    {

    }
}

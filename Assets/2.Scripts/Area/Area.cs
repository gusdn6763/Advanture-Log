using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Area : MonoBehaviour
{
    public AreaSo AreaData { get; private set; }
    public Sprite BackgroundSprite { get; private set; }
    public List<BaseEntity> SpawnedEntities { get; private set; }

    private List<AsyncOperationHandle<GameObject>> defaultInstanceHandles = new List<AsyncOperationHandle<GameObject>>();
    public void Init(AreaSo data)
    {
        AreaData = data;
        SpawnedEntities = new List<BaseEntity>();
    }

    public IEnumerator CreateDefaultObjects()
    {
        // Background
        AsyncOperationHandle<Sprite> bagkgroundHandle = AreaData.BackgroundSpriteRef.LoadAssetAsync<Sprite>();
        yield return bagkgroundHandle;

        BackgroundSprite = bagkgroundHandle.Result;

        // Default Objects
        for (int i = 0; i < AreaData.FixedSpawnEntry.Count; i++)
        {
            FixedSpawnEntry entry = AreaData.FixedSpawnEntry[i];
            AssetReferenceGameObject aref = entry.entitySo.EntityPrefabRef;
  
            AsyncOperationHandle<GameObject> instHandle = aref.InstantiateAsync(entry.localPos, Quaternion.identity, transform);

            yield return instHandle;
            defaultInstanceHandles.Add(instHandle);

            BaseEntity entity = instHandle.Result.GetOrAddComponent<BaseEntity>();

            entity.Init();
            entity.SetInfo(entry.entitySo);

            SpawnedEntities.Add(entity);
        }
    }

    private void OnDestroy()
    {
        Release();
    }

    public void Release()
    {
        AreaData.BackgroundSpriteRef.ReleaseAsset();

        int count = defaultInstanceHandles.Count;
        for (int i = 0; i < count; i++)
            Addressables.ReleaseInstance(defaultInstanceHandles[i]);

        defaultInstanceHandles.Clear();
    }
}
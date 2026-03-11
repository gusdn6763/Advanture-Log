using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface IBootstraper
{
    public IEnumerator Initialize(ILoadProgress progress, GameStartData data);
}

public class GameSceneBootstrap : MonoBehaviour, IBootstraper
{
    [SerializeField] private AreaGraphSo areaGraph;
    [SerializeField] private InteractionController interactionControllerPrefab;
    [SerializeField] private UiController uiControllerPrefab;
    [SerializeField] private AreaSo startArea;

    public IEnumerator Initialize(ILoadProgress progress, GameStartData data)
    {
        progress.UpdateMessage("Creating areas...");
        yield return Managers.Area.CreateAreas(areaGraph, (key, count, totalCount) =>
        {
            progress.UpdateMessage($"Loading Area: {key} {count}/{totalCount}");
        });

        yield return Managers.Area.EnterArea(startArea.Id);

        progress.UpdateMessage("Setting Data...");
        Player player = Managers.Object.SpawnPlayer(Managers.Area.CurrentArea, data, Vector2.zero);

        Instantiate(interactionControllerPrefab).Init(player);
        Instantiate(uiControllerPrefab).Init(player);

        progress.UpdateMessage("Complete");
    }
}  
        
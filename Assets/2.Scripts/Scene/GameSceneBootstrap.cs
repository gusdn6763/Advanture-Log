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

    [SerializeField] private InteractionController interactionController;
    [SerializeField] private UiController uiController;

    public IEnumerator Initialize(ILoadProgress progress, GameStartData data)
    {
        progress.UpdateMessage("Creating areas...");
        yield return Managers.Area.CreateAreas(areaGraph, (key, count, totalCount) =>
        {
            progress.UpdateMessage($"Loading Area: {key} {count}/{totalCount}");
        });

        progress.UpdateMessage("Create Player...");
        Player player = null;
        if (data.load)
        {
            //player = Managers.Object.SpawnPlayer(data);
        }
        else
        {
            //player = Managers.Object.SpawnPlayer(data);
        }
        progress.UpdateMessage("Init UI...");
        uiController.Init(player);

        progress.UpdateMessage("Complete");
    }
}  
        
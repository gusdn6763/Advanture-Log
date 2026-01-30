using System.Collections.Generic;
using UnityEngine;



public class ObjectManager : MonoBehaviour
{
    public BaseEntity Player;

    public List<BaseEntity> activeEntities;
    public List<BaseEntity> ActiveEntities
    {
        get => activeEntities;
        set
        {
            activeEntities = value;
            activeEntities.Add(Player);
        }
    }
}
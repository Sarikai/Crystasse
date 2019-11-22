using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine.Jobs;

[UpdateInGroup(typeof(TransformSystemGroup))]
public class TranslationToTransformSystem : ComponentSystem
{
    private readonly List<Unit> units = new List<Unit>();
    private Crystal[] _crystals;

    protected override void OnCreate()
    {
        base.OnCreate();

        _crystals = GameManager.MasterManager.Crystals;
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        units.Clear();

        foreach(var crystal in _crystals)
            units.AddRange(crystal.Units);
    }

    protected override void OnUpdate()
    {
        var entities = Entities.WithAllReadOnly<TeamID, ID, Translation>();

        entities.ForEach<TeamID, ID, Translation>(
            (TeamID tID, ref ID id, ref Translation translation) =>
            {
                foreach(var unit in units)
                    if(unit.TeamID == tID.Value && unit.ID == id.Value)
                        unit.transform.position = translation.Value;
            });
    }
}

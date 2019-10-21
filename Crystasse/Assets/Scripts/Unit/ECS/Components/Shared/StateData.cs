using Unity.Entities;

public struct StateData : ISharedComponentData
{
    public bool IsDirty;
    public SubStates SubState;
    private UnitCommand _cmd;

    public UnitCommand Command
    {
        get => _cmd;
        set
        {
            _cmd = value;
            IsDirty = true;
        }
    }
}
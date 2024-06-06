

using Godot;

public partial class WalkingAroundInGroupSlaveAIState : FollowToAiState
{
    private WalkingAroundInGroupMasterAIState _masterAIState;
    public WalkingAroundInGroupMasterAIState masterAIState 
    { 
        get
        {
            return _masterAIState;
        } 
        set
        {
            _masterAIState = value;
            this.target = _masterAIState.masterNode3D;
        } 
    }
    public WalkingAroundInGroupSlaveAIState(WalkingAroundInGroupMasterAIState masterAIState, float distance, LocalHumanAI localHumanAI) : base(masterAIState.masterNode3D, distance, localHumanAI)
    {
        masterAIState.AddSlave(this);
    }
}

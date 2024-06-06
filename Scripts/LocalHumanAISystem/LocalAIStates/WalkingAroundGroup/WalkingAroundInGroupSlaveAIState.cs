

using Godot;

public partial class WalkingAroundInGroupSlaveAIState : FollowToAiState
{
    public WalkingAroundInGroupMasterAIState masterAIState { get; set; }
    public WalkingAroundInGroupSlaveAIState(WalkingAroundInGroupMasterAIState masterAIState, float distance, LocalHumanAI localHumanAI) : base(masterAIState.masterNode3D, distance, localHumanAI)
    {
        this.masterAIState.AddSlave(this);
    }
}

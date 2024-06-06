

using System.Collections.Generic;
using Godot;

public partial class WalkingAroundInGroupMasterAIState : WalkingAroundAIState
{
    public Node3D masterNode3D;
    public List<WalkingAroundInGroupSlaveAIState> slaveAIStates = new List<WalkingAroundInGroupSlaveAIState>();
    public WalkingAroundInGroupMasterAIState(Vector3 centralPosition, float distanceOrder, LocalHumanAI localHumanAI, List<WalkingAroundInGroupSlaveAIState> slaveAIStates=null) : base(centralPosition, distanceOrder, localHumanAI)
    {
        this.masterNode3D = localHumanAI;
        if (slaveAIStates != null)
        {
            foreach (WalkingAroundInGroupSlaveAIState slave in this.slaveAIStates)
            {
                this.AddSlave(slave);
            }
        }
    }

    public void AddSlave(WalkingAroundInGroupSlaveAIState slaveAIState)
    {
        this.slaveAIStates.Add(slaveAIState);
        slaveAIState.masterAIState = this;
    }

    ~WalkingAroundInGroupMasterAIState()
    {
        if (this.slaveAIStates.Count != 0)
        {
            WalkingAroundInGroupSlaveAIState selectedAIforMaster = this.slaveAIStates[0];
            this.slaveAIStates.RemoveAt(0);
            selectedAIforMaster.localHumanAI.localAIState = new WalkingAroundInGroupMasterAIState(this.centralPosition, this.distanceOrder, selectedAIforMaster.localHumanAI, this.slaveAIStates);
        }
    }
}
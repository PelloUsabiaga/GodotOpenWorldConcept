using Godot;
using System;
using System.Collections.Generic;

public partial class GameLogic : Node
{
    [Export]
    public Player player;

    [Export]
    public EnvironmentBase environment;

    [Export]
    public Node NPCFolder;

    public NPCFactory nPCFactory { get; set; }
    public TeamsCharacterRegister teamsCharacterRegister { get; set; }

    private Random random;


    public override void _Ready()
    {
        base._Ready();
        this.random = new Random(Guid.NewGuid().GetHashCode());


        this.teamsCharacterRegister = new TeamsCharacterRegister();
        this.nPCFactory = new NPCFactory(this, this.teamsCharacterRegister);

        List<SpawnPoint> spawnPointsValues = new List<SpawnPoint>(this.environment.spawnPointsDict.Values);

        for (int teamID = 0; teamID < 2; teamID++)
        {
            int targetTeamID = 1;
            if (teamID == 1)
            {
                targetTeamID = 0;
            }
            for (int i = 0; i < 6; i++)
            {
                BaseNPC baseNPC = this.nPCFactory.CreateBaseNPC(teamID, spawnPointsValues[teamID]);

                baseNPC.Ready += () => {
                    baseNPC.localHumanAI.localAIState = new FightingWithTeamAIState(targetTeamID, baseNPC.localHumanAI, this);
                };
            }
        }

        this.teamsCharacterRegister.AddMemberToTeam(0, this.player);
        // for (int i = 0; i < 4; i++)
        // {
            
        //     int randomIndex = this.random.Next(spawnPointsValues.Count);
        //     BaseNPC baseNPC = this.nPCFactory.CreateBaseNPC(0, spawnPointsValues[randomIndex]);

        //     if (i == 0)
        //     {
        //         baseNPC.Ready += () => {
        //             baseNPC.localHumanAI.localAIState = new WalkingAroundInGroupMasterAIState(player.GlobalPosition, 20, baseNPC.localHumanAI);
        //         };
        //         master = baseNPC;
        //     }
        //     else
        //     {
        //         baseNPC.Ready += () => {
        //             baseNPC.localHumanAI.localAIState = new WalkingAroundInGroupSlaveAIState((WalkingAroundInGroupMasterAIState)master.localHumanAI.localAIState, 1f, baseNPC.localHumanAI);
        //         };
        //     }
        // }
    }
}

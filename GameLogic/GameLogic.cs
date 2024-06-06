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

    public override void _Ready()
    {
        base._Ready();
        Random rnd = new Random();
        List<SpawnPoint> spawnPointsValues = new List<SpawnPoint>(this.environment.spawnPointsDict.Values);

        TestNPC master = null;
        for (int i = 0; i < 4; i++)
        {
            PackedScene testNPCScene = (PackedScene)GD.Load("res://HumansAndPlayer/Scenes/TestNPC.tscn");
            TestNPC testNPC = testNPCScene.Instantiate<TestNPC>();
            testNPC.Name = String.Format("NPC_{0}", i);
            this.NPCFolder.AddChild(testNPC);
            int randomIndex = rnd.Next(spawnPointsValues.Count);
            spawnPointsValues[randomIndex].AddToSpawnQueue(testNPC);

            if (i == 0)
            {
                testNPC.Ready += () => {
                    testNPC.localHumanAI.localAIState = new WalkingAroundInGroupMasterAIState(player.GlobalPosition, 20, testNPC.localHumanAI);
                };
                master = testNPC;
            }
            else
            {
                testNPC.Ready += () => {
                    testNPC.localHumanAI.localAIState = new WalkingAroundInGroupSlaveAIState((WalkingAroundInGroupMasterAIState)master.localHumanAI.localAIState, 1f, testNPC.localHumanAI);
                };
            }
        }
    }
}

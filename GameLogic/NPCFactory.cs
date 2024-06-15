using System;
using Godot;

public partial class NPCFactory : Node
{
    public GameLogic gameLogic { get; set; }
    private PackedScene baseNPCScene;
    public TeamsCharacterRegister teamsCharacterRegister { get; set; }
    public NPCFactory(GameLogic gameLogic, TeamsCharacterRegister teamsCharacterRegister)
    {
        this.gameLogic = gameLogic;
        this.teamsCharacterRegister = teamsCharacterRegister;
        this.baseNPCScene = (PackedScene)GD.Load("res://HumansAndPlayer/Scenes/BaseNPC.tscn");
    }
    public BaseNPC CreateBaseNPC(int teamId, SpawnPoint spawnPoint, String name = "")
    {
        BaseNPC baseNPC = baseNPCScene.Instantiate<BaseNPC>();
        baseNPC.Setup(teamId, name);
        this.gameLogic.NPCFolder.AddChild(baseNPC);
        this.teamsCharacterRegister.AddMemberToTeam(teamId, baseNPC);
        spawnPoint.AddToSpawnQueue(baseNPC);
        return baseNPC;
    }

}
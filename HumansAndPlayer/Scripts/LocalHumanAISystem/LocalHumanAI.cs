using Godot;
using System;

public partial class LocalHumanAI : Node3D
{
    [Export]
    public HumanCharacter targetHumanCharacter;

    public LocalAIState localAIState{ get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.localAIState = new NoneAIState();
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        this.localAIState.Destroy();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        this.localAIState = this.localAIState.Update(delta, this);
    }
}

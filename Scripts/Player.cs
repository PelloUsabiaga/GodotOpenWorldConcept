using Godot;
using System;

public partial class Player : CharacterBody3D
{
    public HumanCharacter humanCharacter;
    public PlayerSpeakComponent playerSpeakComponent;
    public PlayerCamera3D playerCamera3D;

    private float cameraRotation = 0;

    public override void _Ready()
    {
        base._Ready();
        this.humanCharacter = GetNode<HumanCharacter>("HumanCharacter");
        this.playerSpeakComponent = GetNode<PlayerSpeakComponent>("PlayerSpeakComponent");
        this.playerCamera3D = GetNode<PlayerCamera3D>("PlayerCamera3D");

        this.humanCharacter.weaponSystem.TryEquipWeapon(new SimpleShot());

        this.humanCharacter.DamagedCs += this.DamagedEventHandler;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    private void DamagedEventHandler()
    {
        if (this.humanCharacter.isDead)
        {
            GD.Print("Game over!");
        }
    }
}

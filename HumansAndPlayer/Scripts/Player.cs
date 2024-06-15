using Godot;
using System;

public partial class Player : CharacterBody3D, ITargeteable, ITeamMember
{
    public HumanCharacter humanCharacter;
    public PlayerSpeakComponent playerSpeakComponent;
    public PlayerCamera3D playerCamera3D;
    public CollisionObject3D collisionObject3D { get => this.humanCharacter.collisionObject3D; set => this.humanCharacter.collisionObject3D = value; }

    public Node3D node3D { get {return this.humanCharacter;} }


    private float cameraRotation = 0;

    public override void _Ready()
    {
        base._Ready();
        this.humanCharacter = GetNode<HumanCharacter>("HumanCharacter");
        this.playerSpeakComponent = GetNode<PlayerSpeakComponent>("PlayerSpeakComponent");
        this.playerCamera3D = GetNode<PlayerCamera3D>("PlayerCamera3D");

        // this.humanCharacter.movementSpeed = 10;

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

    public DamageComponent GetDamageComponent()
    {
        return this.humanCharacter.damageComponent;
    }

}

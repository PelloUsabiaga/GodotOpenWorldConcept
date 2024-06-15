using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

public partial class BaseNPC : CharacterBody3D, IAttackable, ITargeteable, ITeamMember
{
    public int teamID { get; set; }
    public String name { get; set; }
    public CollisionObject3D collisionObject3D { get => this.humanCharacter.collisionObject3D; set => this.humanCharacter.collisionObject3D = value; }

    public Node3D node3D { get {return this.humanCharacter;} }


    public SpeakComponent speakComponent;
    public HumanCharacter humanCharacter;

    

    public LocalHumanAI localHumanAI;
    
    public void Setup(int teamID, String name = "")
    {
        this.name = name;
        this.teamID = teamID;
    }

    public override void _Ready()
    {
        base._Ready();
        this.speakComponent = GetNode<SpeakComponent>("SpeakComponent");
        this.humanCharacter = GetNode<HumanCharacter>("HumanCharacter");
        this.localHumanAI = GetNode<LocalHumanAI>("LocalHumanAI");

        Player player = GetNode<Player>("/root/Node3D/Player");

        this.humanCharacter.DamagedCs += this.DamagedEventHandler;

        this.speakComponent.StartConversationCallback = StartConversationTestNPC;
        this.speakComponent.ContinueConversationCallback = ContinueConversationTestNPC;
        this.speakComponent.EndConversationCallback = EndConversationTestNPC;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private SpeakResponse StartConversationTestNPC()
    {
        return new SpeakResponse(String.Format("Hello, I am {0}", this.name), 
                                    new List<string>{"Where are you from?"});
    }
    private SpeakResponse ContinueConversationTestNPC(string message)
    {
        return new SpeakResponse(String.Format("I am from some place, yes.", this.name), 
                                    new List<string>{"Other question."}, endConversation: true);
    }
    private void EndConversationTestNPC(string message)
    {
        
    }

    public DamageComponent GetDamageComponent()
    {
        return this.humanCharacter.damageComponent;
    }

    private void DamagedEventHandler()
    {
        if (this.humanCharacter.isDead)
        {
            this.humanCharacter.QueueFree();
            this.speakComponent.QueueFree();
            this.localHumanAI.QueueFree();
            GetNode<CollisionShape3D>("CollisionShape3D").QueueFree();
            
            Task.Delay(new TimeSpan(0, 0, 15)).ContinueWith(o => 
            { 
                this.GetParent().CallDeferred("remove_child", this); 
            });
        }
    }
}

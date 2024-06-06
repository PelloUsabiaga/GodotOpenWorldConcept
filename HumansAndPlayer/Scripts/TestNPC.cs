using Godot;
using System;
using System.Collections.Generic;

public partial class TestNPC : CharacterBody3D, AttackableColliderInterface
{
    [Export]
    string name;
    public SpeakComponent speakComponent;
    public HumanCharacter humanCharacter;

    public LocalHumanAI localHumanAI;

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
            this.GetParent().RemoveChild(this);
        }
    }
}

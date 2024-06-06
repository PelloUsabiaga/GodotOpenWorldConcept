using Godot;
using System;
using System.Collections.Generic;

public partial class SpeakComponent : Area3D
{
    [Export]
    public Node speakerInterfaceNode;
    private SpeakerInterface speakerInterface;
    public delegate SpeakResponse StartConversationType();
    public delegate SpeakResponse ContinueConversationType(string message);
    public delegate void EndConversationType(string message);

    public StartConversationType StartConversationCallback;
    public ContinueConversationType ContinueConversationCallback;
    public EndConversationType EndConversationCallback;

    public bool IsSpeaking { get; set; } = false;
    
    public override void _Ready()
    {
        base._Ready();

        this.speakerInterface = (SpeakerInterface) this.speakerInterfaceNode;

        this.AreaEntered += this.SpeakComponentEntered;
        this.speakerInterface.SpeakEndedCs += this.AbandonConversation;
    }

    private void SpeakComponentEntered(Area3D area)
    {
        
    }

    public void AbandonConversation()
    {
        if (this.IsSpeaking)
        {
            this.speakerInterface.EndSpeak();
            this.IsSpeaking = false;
        }
    }

    public SpeakResponse StartConversation()
    {
        this.speakerInterface.StartSpeak();
        this.IsSpeaking = true;
        if (this.StartConversationCallback != null)
        {
            SpeakResponse responseWithDefaults = this.StartConversationCallback();
            if (responseWithDefaults.EndConversation)
            {
                this.speakerInterface.EndSpeak();
                this.IsSpeaking = false;
            }
            return responseWithDefaults;
        }
        else
        {
            GD.PrintErr("No callback found for function StartConversationCallback, it was null");
            return new SpeakResponse("No callback found", new List<string>{});
        }
    }
    
    public SpeakResponse ContinueConversation(string message)
    {
        if (this.IsSpeaking)
        {
            if (this.ContinueConversationCallback != null)
            {
                SpeakResponse responseWithDefaults = this.ContinueConversationCallback(message);
                if (responseWithDefaults.EndConversation)
                {
                    this.speakerInterface.EndSpeak();
                    this.IsSpeaking = false;
                }
                return responseWithDefaults;
            }
            else
            {
                GD.PrintErr("No callback found for function ContinueConversationCallback, it was null");
                return new SpeakResponse("No callback found", new List<string>{});
            }
        }
        else
        {
            // If conversation have been abandoned, empty response
            return new SpeakResponse("...", endConversation: true);
        }
    }

    public void EndConversation(string message)
    {
        this.speakerInterface.EndSpeak();
        this.IsSpeaking = false;
        if (this.EndConversationCallback != null)
        { 
            this.EndConversationCallback(message);
        }
        else
        {
            GD.PrintErr("No callback found for function EndConversationCallback, it was null");
        }
    }

    public void DeactivateSpeakComponent()
    {
        this.Monitorable = false;
        this.Monitoring = false;
    }
}

using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerSpeakComponent : Node3D
{
    private Area3D detectionArea;

    [Export]
    public PlayerCamera3D playerCamera3D;

    [Export]
    public Player player;
    [Export]
    public HumanCharacter humanCharacter;


    private Godot.Collections.Dictionary<SpeakComponent, Button> speakComponentToButton;
    private Control uiControl;
    private PackedScene speakButtonScene;

    private Label responseLabel;
    private Button endConversationButton;
    private VBoxContainer conversationContainer;
    private VBoxContainer messageOptionsContainer;
    private List<Button> messageOptionsButtons = new List<Button>();

    private SpeakComponent currentlySpeakingSpeakComponent = null;
    
    private SpeakResponse _lastSpeakResponse;
    private SpeakResponse lastSpeakResponse 
    { 
        get
        {
            return _lastSpeakResponse;
        } 
        set
        {
            _lastSpeakResponse = value;
            NewSpeakConversationReceived();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.detectionArea = GetNode<Area3D>("Area3D");
        this.uiControl = GetNode<Control>("Control");
        this.responseLabel = GetNode<Label>("ConversationContainer/ResponseLabel");
        this.endConversationButton = GetNode<Button>("ConversationContainer/EndConversationButton");
        this.endConversationButton.Pressed += EndConversation;
        this.conversationContainer = GetNode<VBoxContainer>("ConversationContainer");
        this.messageOptionsContainer = GetNode<VBoxContainer>("ConversationContainer/MessageOptionsContainer");
        this.conversationContainer.Visible = false;

        this.humanCharacter.SpeakEnded += this.EndConversation;

        this.detectionArea.AreaEntered += this.SpeakComponentEntered;

        this.speakButtonScene = GD.Load<PackedScene>("res://Scenes/SpeakSystem/SpeakButton.tscn");
        this.speakComponentToButton = new Godot.Collections.Dictionary<SpeakComponent, Button>();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Godot.Collections.Array<SpeakComponent> speakComponentsInRangue = this.GetSpeakComponentsInRangue();
        
        this.DrawSpeakButtons(speakComponentsInRangue);
    }

    private void DrawSpeakButtons(Godot.Collections.Array<SpeakComponent> speakComponentsInRangue)
    {
        float screenCenterX = GetViewport().GetVisibleRect().Size.X / 2;

        foreach (SpeakComponent speakComponent in this.speakComponentToButton.Keys)
        {
            if (!speakComponentsInRangue.Contains(speakComponent))
            {
                this.speakComponentToButton[speakComponent].QueueFree();
                this.speakComponentToButton.Remove(speakComponent);
            }
            else
            {
                Button speakButton = this.speakComponentToButton[speakComponent];
                Vector2 speakComponentPositionOnScreen = this.playerCamera3D.camera3D.UnprojectPosition(speakComponent.GlobalPosition);
               
                speakButton.Position = speakComponentPositionOnScreen 
                                                    - 0.5f*speakButton.Size 
                                                    + Vector2.Up * 70 
                                                    + Vector2.Right * 0.1f * (speakComponentPositionOnScreen.X - screenCenterX);
            }
        }
    }

    private void SpeakButtonClicked(SpeakComponent speakComponent){
        this.StartConversation(speakComponent);
    }

    public void StartConversation(SpeakComponent speakComponent)
    {
        if (this.currentlySpeakingSpeakComponent != null)
        {
            this.EndConversation();
        }
        this.currentlySpeakingSpeakComponent = speakComponent;
        this.player.humanCharacter.StartSpeak();
        this.lastSpeakResponse = currentlySpeakingSpeakComponent.StartConversation();
        this.conversationContainer.Visible = true;
    }

    public void ContinueConversation(string message)
    {
        if (this.currentlySpeakingSpeakComponent != null)
        {
            this.lastSpeakResponse = this.currentlySpeakingSpeakComponent.ContinueConversation(message);
        }
    }

    public void EndConversation()
    {
        if (this.currentlySpeakingSpeakComponent != null)
        {
            this.currentlySpeakingSpeakComponent.EndConversation(this.lastSpeakResponse.EndConversationMessage);
            this.currentlySpeakingSpeakComponent = null;
            this.player.humanCharacter.EndSpeak();
            this.conversationContainer.Visible = false;
        }
    }

    private void NewSpeakConversationReceived()
    {
        if (this.lastSpeakResponse.EndConversation)
        {
            this.EndConversation();
        }
        else
        {
            this.DrawSpeakConversationInGUI();
        }
    }
    private void DrawSpeakConversationInGUI()
    {
        this.responseLabel.Text = this.lastSpeakResponse.Response;
        this.endConversationButton.Text = this.lastSpeakResponse.EndConversationMessage;
        foreach (Button messageButton in this.messageOptionsButtons)
        {
            messageButton.QueueFree();
        }
        this.messageOptionsButtons.Clear();
        foreach (string message in this.lastSpeakResponse.SuggestedMessages)
        {
            Button messageButton = new Button();
            messageButton.Text = message;
            messageButton.ButtonDown += () => ContinueConversation(messageButton.Text);
            this.messageOptionsContainer.AddChild(messageButton);
            this.messageOptionsButtons.Add(messageButton);
        }
    }

    private void SpeakComponentEntered(Area3D area)
    {
        SpeakComponent speakComponent = (SpeakComponent) area;
        Button speakButton = (Button) this.speakButtonScene.Instantiate();
        this.speakComponentToButton.Add(speakComponent, speakButton);
        this.uiControl.AddChild(speakButton);
        speakButton.ButtonDown += () => this.SpeakButtonClicked(speakComponent);
    }

    public Godot.Collections.Array<SpeakComponent> GetSpeakComponentsInRangue()
    {
        Godot.Collections.Array<Area3D> area3Ds = this.detectionArea.GetOverlappingAreas();
        Godot.Collections.Array<SpeakComponent> speakComponents = new Godot.Collections.Array<SpeakComponent>();
        foreach (var area in area3Ds)
        {
            speakComponents.Add((SpeakComponent)area);
        }
        return speakComponents;
    }
}

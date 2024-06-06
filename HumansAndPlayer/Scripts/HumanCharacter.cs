using Godot;
using System;

public partial class HumanCharacter : Node3D, SpeakerInterface, DamageableInterface
{
    [Export]
    public CharacterBody3D targetCharacterBody;
    public HumanState humanState { get; private set; }

    public WeaponSystem weaponSystem;
    public DamageComponent damageComponent;
    private NavigationComponent navigationComponent;

    public bool isWounded { get; set; }
    public bool isDead { get; set; } = false;
    public float movementSpeed {get; set;} = 5;
    public float jumpSpeed {get; set;} = 5;
    public double jumpTime = 0.5;
    private float rotationSpeed = 5;

    private RayCast3D floorRayCast;
    private Vector3 targetVelocity = Vector3.Zero;


    [Signal]
    public delegate void SpeakEndedEventHandler();
    public event SpeakEndedCsEventHandler SpeakEndedCs;
    public event DamagedCsEventHandler DamagedCs;

    [Signal]
    public delegate void NavigationTargetReachedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        this.humanState = new NormalState(this);
        this.weaponSystem = GetNode<WeaponSystem>("WeaponSystem");
        this.damageComponent = GetNode<DamageComponent>("DamageComponent");
        this.navigationComponent = GetNode<NavigationComponent>("NavigationComponent");
        this.floorRayCast = GetNode<RayCast3D>("FloorRayCast");

        this.navigationComponent.NewVelocityAvailable += this.NewVelocityAvailableCallback;
        this.navigationComponent.TargetReached += this.TargetReachedCallback;
    }

    public override void _Process(double delta)
    {
        if (!this.isDead)
        {
            this.humanState = this.humanState.Update(delta, this);
        }
    }

    public void HandleInput(InputAction inputAction)
    {
        if (!this.isDead)
        {
            if (!this.TryDefaultInputHandling(inputAction))
            {
                this.humanState = this.humanState.HandleInput(inputAction);
            }
        }
    }

    private bool TryDefaultInputHandling(InputAction inputAction)
    {
        if (inputAction.ActionType == InputActionType.MoveWASD && this.humanState.CanMove)
        {
            this.HandleMoveWASDInput((MoveWASDAction) inputAction);
            return true;
        }
        else if (inputAction.ActionType == InputActionType.Jump && this.humanState.CanJump)
        {
            this.HandleJumpInput((JumpAction) inputAction);
            return true;
        }
        else if (inputAction.ActionType == InputActionType.Attack && this.humanState.CanAttack)
        {
            this.HandleAttackInput((AttackAction) inputAction);
        }
        else if (inputAction.ActionType == InputActionType.MoveTo && this.humanState.CanMove)
        {
            this.navigationComponent.UpdateTargetPosition(((MoveToAction) inputAction).targetPosition);
        }
        return false;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!this.isDead)
        {
            if (this.humanState.CanMove)
            {
                if (!this.navigationComponent.isNavigationActive)
                {
                    this.Move();
                }
                this.Rotate(delta);
            }
            if (!this.targetCharacterBody.IsOnFloor() && !(this.humanState is JumpingState))
            {
                if (!this.floorRayCast.IsColliding())
                {
                    this.humanState = new JumpingState(0, 0, this);
                }
                else // Fall without jumping
                {
                    Vector3 newVelocity = this.targetCharacterBody.Velocity;
                    newVelocity.Y -= (float)(9.8 * delta);
                    this.targetCharacterBody.Velocity = newVelocity;
                }
            }
            if (!this.navigationComponent.isNavigationActive)
            {
                this.targetCharacterBody.MoveAndSlide();
            }
        }
    }

    private void NewVelocityAvailableCallback(Vector3 newVelocity)
    {
        if (this.navigationComponent.isNavigationActive)
        {
            this.targetCharacterBody.Velocity = newVelocity;
            this.targetCharacterBody.MoveAndSlide();
        }
    }

    private void TargetReachedCallback()
    {
        EmitSignal(SignalName.NavigationTargetReached);
    }
    private void HandleMoveWASDInput(MoveWASDAction inputAction)
    {
        if (inputAction.movementDirection != Vector2.Zero)
        {
            this.navigationComponent.StopOnCurrentPosition();
        }
        Vector2 horizontalVelocity = inputAction.movementDirection * this.movementSpeed;
        Vector3 velocityVector = new Vector3(horizontalVelocity.X, this.targetCharacterBody.Velocity.Y, 
                                            horizontalVelocity.Y);
     
        
        this.targetVelocity = velocityVector;        
    }

    private void Rotate(double delta)
    {
        Vector3 newVelocity = this.targetCharacterBody.Velocity;
        if (newVelocity.X != 0 | newVelocity.Z != 0)
        {
            Vector3 newRotation = this.targetCharacterBody.Rotation;
            newRotation.Y = Mathf.LerpAngle(this.targetCharacterBody.Rotation.Y, 
                                            Mathf.Atan2(newVelocity.X, newVelocity.Z), 
                                            (float)delta*this.rotationSpeed);
            this.targetCharacterBody.Rotation = newRotation;
        }
    }

    private void Move()
    {
        this.targetCharacterBody.Velocity = this.targetCharacterBody.Velocity.MoveToward(this.targetVelocity, 0.25f);
    }

    private void HandleJumpInput(JumpAction jumpAction)
    {
        this.humanState = new JumpingState(this.jumpSpeed, this.jumpTime, this);
    }

    private void HandleAttackInput(AttackAction attackAction)
    {
        this.humanState = new AttackingState(2, 1, this, attackAction);
    }

    public void StartSpeak()
    {
        this.humanState = new SpeakingState();
    }

    // Expect this to be called many times, due to signal callbacks calling it. But signal is only emited once.
    public void EndSpeak()
    {
        if (this.humanState is SpeakingState)
        {
            this.humanState = new NormalState(this);
            EmitSignal(SignalName.SpeakEnded);
            if(SpeakEndedCs != null)
            {
                SpeakEndedCs();
            }
        }
    }

    public void NotifyHealthChanged(int newHealth)
    {
        if (newHealth <= 0)
        {
            this.isDead = true;
        }
        if(DamagedCs != null)
        {
            DamagedCs();
        }
    }

    public Attack ApplyDamageModifiersToAttack(Attack attack)
    {
        return attack;
    }

}

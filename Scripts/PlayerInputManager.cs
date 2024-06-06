using Godot;
using System;

public partial class PlayerInputManager : Node
{
    [Export]
    public Player playerCharacter;

    private Rid rid;

    [Export]
    public Node3D world;

    public override void _Ready()
    {
        base._Ready();
        this.rid = this.world.GetWorld3D().NavigationMap;
    }


    public override void _PhysicsProcess(double delta)
    {
        this.HanldeMovementInput(delta);

        this.HanldeJumpInput(delta);
        
        this.HandleAttackInput(delta);
    }

    private void HanldeMovementInput(double delta)
    {
        Vector2 movementDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown").Normalized();

        MoveWASDAction moveWASDAction = new MoveWASDAction(movementDirection, delta);
        this.playerCharacter.humanCharacter.HandleInput(moveWASDAction);
    }

    private void HanldeJumpInput(double delta){
        if (Input.IsActionJustPressed("Jump"))
        {
            this.playerCharacter.humanCharacter.HandleInput(new JumpAction());
        }
    }

    private void HandleAttackInput(double delta){
        if (Input.IsActionJustPressed("Attack"))
        {
            Godot.Collections.Dictionary collidedElements = this.playerCharacter.playerCamera3D.RaycastFromCamera(GetViewport().GetMousePosition());
            if (collidedElements != null){
                if (collidedElements.Count > 0){
                    bool isAttackable = false;
                    DamageComponent clickedDamageComponent = null;
                    try
                    {
                        clickedDamageComponent = ((AttackableColliderInterface) collidedElements["collider"].AsGodotObject()).GetDamageComponent();
                        isAttackable = true;
                        this.playerCharacter.humanCharacter.HandleInput(new AttackAction(clickedDamageComponent));

                    } catch (Exception)
                    {
                        isAttackable = false;
                    }
                    
                    if (!isAttackable)
                    {
                        Vector3 targetPosition = NavigationServer3D.MapGetClosestPoint(this.rid, (Vector3)collidedElements["position"]); 
                        this.playerCharacter.humanCharacter.HandleInput(new MoveToAction(targetPosition));
                    }
                }
            }
        }
    }
}

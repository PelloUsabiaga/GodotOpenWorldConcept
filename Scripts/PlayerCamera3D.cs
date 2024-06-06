using Godot;
using System;

public partial class PlayerCamera3D : Node3D
{
    public Camera3D camera3D;
    [Export]
    public Player player;
    public override void _Ready()
    {
        base._Ready();
        this.camera3D = GetNode<Camera3D>("Camera3D");
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        this.Position = this.player.GlobalPosition;
    }

    private float rayLenght = 1000;
    public Godot.Collections.Dictionary RaycastFromCamera(Vector2 originPoint){
        PhysicsDirectSpaceState3D stateSpace = this.camera3D.GetWorld3D().DirectSpaceState;

        Vector3 origin = this.camera3D.ProjectRayOrigin(originPoint);
        Vector3 end = origin + this.camera3D.ProjectRayNormal(originPoint) * this.rayLenght;
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithAreas = false;

        return stateSpace.IntersectRay(query);
    }
}

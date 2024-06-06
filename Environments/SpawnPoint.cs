using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnPoint : Node3D
{
    [Export]
    public string name;

    private Queue<Node3D> nodeQueueToSpawn = new Queue<Node3D>();

    private Area3D area3D;

    private double timeSinceLastSpawn = 0;

    public override void _Ready()
    {
        base._Ready();
        this.area3D = GetNode<Area3D>("Area3D");
    }
    public void AddToSpawnQueue(Node3D node3D)
    {
        this.nodeQueueToSpawn.Enqueue(node3D);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        this.timeSinceLastSpawn += delta;
        if (this.nodeQueueToSpawn.Count > 0 && this.timeSinceLastSpawn > 1)
        {
            if (!this.area3D.HasOverlappingBodies())
            {
                this.nodeQueueToSpawn.Dequeue().GlobalPosition = this.GlobalPosition + Vector3.Up;
                this.timeSinceLastSpawn = 0;
                GD.Print("Dequeued!");
            }
        }
    }
}

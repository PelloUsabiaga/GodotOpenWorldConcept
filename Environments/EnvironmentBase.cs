

using System;
using System.Collections.Generic;
using Godot;

public abstract partial class EnvironmentBase : Node3D
{
    public Dictionary<String, SpawnPoint> spawnPointsDict = new Dictionary<string, SpawnPoint>();
    private Node spawnPointFolder;
    public override void _Ready()
    {
        base._Ready();
        this.spawnPointFolder = GetNode<Node>("SpawnPointsFolder");
        Godot.Collections.Array<Node> spawnPoints = this.spawnPointFolder.GetChildren();
        foreach (Node spawnPointCandidate in spawnPoints)
        {
            if (spawnPointCandidate is SpawnPoint)
            {
                SpawnPoint spawnPoint = (SpawnPoint)spawnPointCandidate;
                this.spawnPointsDict[spawnPoint.name] = spawnPoint;
            }
        }
    }
}
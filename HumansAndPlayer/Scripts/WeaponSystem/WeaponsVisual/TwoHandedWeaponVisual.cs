using Godot;
using System;

public partial class TwoHandedWeaponVisual : WeaponVisual
{
    [Export]
    public Skeleton3D skeleton3D;

    private int leftHandBoneIndex;

    public override void _Ready()
    {
        base._Ready();
        this.leftHandBoneIndex = this.skeleton3D.FindBone("mixamorig_LeftHandIndex1");
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        Transform3D localLeftHandBoneTransform = this.skeleton3D.GetBoneGlobalPose(this.leftHandBoneIndex);
        Vector3 globalLeftHandPosition = this.skeleton3D.ToGlobal(localLeftHandBoneTransform.Origin);
        this.LookAt(globalLeftHandPosition);
    }
}

using Godot;
using System;

public partial class SimpleShotVisual : TwoHandedWeaponVisual
{
    private GpuParticles3D smokeEfect;
    public override void _Ready()
    {
        base._Ready();
        this.smokeEfect = GetNode<GpuParticles3D>("SmokeEfect");
    }
    public override void PlayAttackEffect()
    {
        base.PlayAttackEffect();
        this.smokeEfect.Emitting = true;
    }
}

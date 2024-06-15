


using Godot;

public abstract partial class Weapon : Node3D
{
    public virtual bool CanMoveWileAttack { get; set; } = true;

    public virtual string VisualName { get; set; } = "";

    public virtual float AttackDuration { get; set; } = 2;

    public virtual float AttackTime { get; set; } = 1;

    public abstract float AttackDistance { get; set; }
    public Attack? TryAttackToDamageComponent(DamageComponent damageComponent)
    {
        if (damageComponent == null || !IsInstanceValid(damageComponent))
        {
            return null;
        }
        else
        {
            return this.TryAttackToExistingDamageComponent(damageComponent);
        }
    }

    protected abstract Attack? TryAttackToExistingDamageComponent(DamageComponent damageComponent);
}
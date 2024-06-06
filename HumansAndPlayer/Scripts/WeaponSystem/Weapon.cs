


using Godot;

public abstract partial class Weapon : Node3D
{
    public Attack? TryAttackToDamageComponent(DamageComponent damageComponent)
    {
        if (damageComponent == null)
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
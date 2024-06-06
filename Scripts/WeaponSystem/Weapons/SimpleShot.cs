

public partial class SimpleShot : Weapon
{
    protected override Attack? TryAttackToExistingDamageComponent(DamageComponent damageComponent)
    {
        if (this.Position.DistanceSquaredTo(damageComponent.Position) < 9)
        {
            return new Attack(1);
        }
        else
        {
            return null;
        }
    }
}
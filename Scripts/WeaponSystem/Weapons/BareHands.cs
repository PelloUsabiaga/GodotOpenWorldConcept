

public partial class BareHands : Weapon
{
    protected override Attack? TryAttackToExistingDamageComponent(DamageComponent damageComponent)
    {
        if (this.Position.DistanceSquaredTo(damageComponent.Position) < 2)
        {
            return new Attack(1);
        }
        else
        {
            return null;
        }
    }
}
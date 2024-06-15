

public partial class BareHands : Weapon
{
    public override float AttackDistance { get; set; } = 2;

    protected override Attack? TryAttackToExistingDamageComponent(DamageComponent damageComponent)
    {
        if (this.Position.DistanceSquaredTo(damageComponent.Position) < 4)
        {
            return new Attack(1);
        }
        else
        {
            return null;
        }
    }
}


public partial class SimpleShot : Weapon
{
    public override bool CanMoveWileAttack { get; set; } = false;
    public override string VisualName { get; set; } = "SimpleShotVisual";
    public override float AttackTime { get; set; } = 0.5f;

    public override float AttackDistance { get; set; } = 10;
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
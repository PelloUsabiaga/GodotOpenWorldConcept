using Godot;
using System;

public partial class WeaponSystem : Node3D
{
    public Weapon EquipedWeapon { get; private set; } = new BareHands();

    public void TryAttackToDamageComponent(DamageComponent damageComponent)
    {
        Attack? attackTry = this.EquipedWeapon.TryAttackToDamageComponent(damageComponent);
        if (attackTry != null)
        {
            damageComponent.MakeAttack((Attack) attackTry);
        }
    }

    public bool TryEquipWeapon(Weapon newWeapon)
    {
        this.EquipedWeapon = newWeapon;
        return true;
    }
}

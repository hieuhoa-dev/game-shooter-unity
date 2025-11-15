using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] int ammoAmount = 100;

    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        WeaponSO currentWeapon = GunManager.instance.currentWeaponSO;
        if (currentWeapon != null)
        {
            GunManager.instance.AdjustAmmo(currentWeapon, ammoAmount);
        }
    }
}

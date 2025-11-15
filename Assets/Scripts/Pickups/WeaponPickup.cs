using UnityEngine;

public class WeaponPickup : Pickup
{
    [SerializeField] WeaponSO weaponSO;

    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        GunManager.instance.UnlockWeapon(weaponSO);
        GunManager.instance.SwitchToWeapon(weaponSO);
    }
}

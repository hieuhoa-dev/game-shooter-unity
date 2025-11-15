using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    public static GunManager instance;

    [Header("Weapon ScriptableObjects")]
    [SerializeField] WeaponSO pistolSO;
    [SerializeField] WeaponSO sniperSO;
    [SerializeField] WeaponSO machineGunSO;
    [SerializeField] WeaponSO startingWeapon;

    public WeaponSO currentWeaponSO;
    public Weapon currentWeapon;

    [Header("Weapon Availability")]
    public bool isHasPistol;
    public bool isHasSniper;
    public bool isHasMachineGun;

    [Header("Ammo")]
    public int currentPistolAmmo;
    public int currentSniperAmmo;
    public int currentMachineGunAmmo;

    GameObject pistolInstance;
    GameObject sniperInstance;
    GameObject machineGunInstance;

    ActiveWeapon activeWeapon;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        activeWeapon = FindObjectOfType<ActiveWeapon>();

        InitializeWeapons();

        // Set starting weapon
        SwitchToWeapon(startingWeapon);
    }

    void InitializeWeapons()
    {
        // Xóa mọi weapon cũ trong ActiveWeapon
        foreach (Transform child in activeWeapon.transform)
        {
            Destroy(child.gameObject);
        }

        // Khởi tạo sở hữu ban đầu
        isHasPistol = startingWeapon == pistolSO;
        isHasSniper = startingWeapon == sniperSO;
        isHasMachineGun = startingWeapon == machineGunSO;

        // Instantiate vũ khí
        pistolInstance = Instantiate(pistolSO.weaponPrefab, activeWeapon.transform);
        sniperInstance = Instantiate(sniperSO.weaponPrefab, activeWeapon.transform);
        machineGunInstance = Instantiate(machineGunSO.weaponPrefab, activeWeapon.transform);

        pistolInstance.SetActive(false);
        sniperInstance.SetActive(false);
        machineGunInstance.SetActive(false);

        // Khởi tạo đạn
        currentPistolAmmo = pistolSO.MagazineSize;
        currentSniperAmmo = sniperSO.MagazineSize;
        currentMachineGunAmmo = machineGunSO.MagazineSize;
    }

    private void Update()
    {
        HandleWeaponSwitching();
    }

    void HandleWeaponSwitching()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame && isHasPistol)
            SwitchToWeapon(pistolSO);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame && isHasSniper)
            SwitchToWeapon(sniperSO);
        else if (Keyboard.current.digit3Key.wasPressedThisFrame && isHasMachineGun)
            SwitchToWeapon(machineGunSO);
    }

    public void SwitchToWeapon(WeaponSO weapon)
    {
        currentWeaponSO = weapon;

        pistolInstance.SetActive(weapon == pistolSO);
        sniperInstance.SetActive(weapon == sniperSO);
        machineGunInstance.SetActive(weapon == machineGunSO);

        if (weapon == pistolSO)
            currentWeapon = pistolInstance.GetComponent<Weapon>();
        else if (weapon == sniperSO)
            currentWeapon = sniperInstance.GetComponent<Weapon>();
        else if (weapon == machineGunSO)
            currentWeapon = machineGunInstance.GetComponent<Weapon>();

        // cập nhật UI đạn
        activeWeapon.UpdateAmmoUI(GetCurrentAmmo(weapon));
    }

    public void AdjustAmmo(WeaponSO weaponSO, int amount)
    {
        if (weaponSO == pistolSO)
            currentPistolAmmo = Mathf.Clamp(currentPistolAmmo + amount, 0, pistolSO.MagazineSize);

        else if (weaponSO == sniperSO)
            currentSniperAmmo = Mathf.Clamp(currentSniperAmmo + amount, 0, sniperSO.MagazineSize);

        else if (weaponSO == machineGunSO)
            currentMachineGunAmmo = Mathf.Clamp(currentMachineGunAmmo + amount, 0, machineGunSO.MagazineSize);

        // Chỉ update UI nếu là súng đang cầm
        if (weaponSO == currentWeaponSO)
            activeWeapon.UpdateAmmoUI(GetCurrentAmmo(weaponSO));
    }

    public int GetCurrentAmmo(WeaponSO weaponSO)
    {
        if (weaponSO == pistolSO) return currentPistolAmmo;
        if (weaponSO == sniperSO) return currentSniperAmmo;
        if (weaponSO == machineGunSO) return currentMachineGunAmmo;
        return 0;
    }

    public bool UnlockWeapon(WeaponSO weaponSO)
    {
        if (weaponSO == pistolSO) isHasPistol = true;
        else if (weaponSO == sniperSO) isHasSniper = true;
        else if (weaponSO == machineGunSO) isHasMachineGun = true;
        else return false;

        return true;
    }
}

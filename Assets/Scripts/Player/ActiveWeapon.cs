using StarterAssets;
using UnityEngine;
using TMPro;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject zoomVignette;

    public TMP_Text ammoText;

    Animator animator;
    StarterAssetsInputs starterInputs;
    FirstPersonController controller;

    const string SHOOT_ANIM = "Shoot";

    float shootTimer = 0f;
    float defaultFOV;
    float defaultRotationSpeed;


    private void Awake()
    {
        starterInputs = GetComponentInParent<StarterAssetsInputs>();
        controller = GetComponentInParent<FirstPersonController>();
        animator = GetComponent<Animator>();

        defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        defaultRotationSpeed = controller.RotationSpeed;
    }

    private void Start()
    {
        // Đảm bảo GunManager đã khởi tạo xong trước khi sử dụng
        if (GunManager.instance != null)
        {
            UpdateAmmoUI(0); // Initialize UI
        }
    }

    private void Update()
    {
        HandleShoot();
        HandleZoom();
    }

    void HandleShoot()
    {
        shootTimer += Time.deltaTime;

        // Null checks
        if (GunManager.instance == null) return;
        
        WeaponSO weapon = GunManager.instance.currentWeaponSO;
        Weapon weaponClass = GunManager.instance.currentWeapon;

        if (weapon == null || weaponClass == null) return;
        if (!starterInputs.shoot) return;

        // đủ delay bắn chưa
        if (shootTimer >= weapon.FireRate && GunManager.instance.GetCurrentAmmo(weapon) > 0)
        {
            weaponClass.Shoot(weapon);

            animator.Play(SHOOT_ANIM, 0, 0f);

            shootTimer = 0f;

            GunManager.instance.AdjustAmmo(weapon, -1);
        }

        if (!weapon.isAutomatic)
        {
            starterInputs.ShootInput(false);
        }
    }

    void HandleZoom()
    {
        if (GunManager.instance == null) return;
        
        WeaponSO weapon = GunManager.instance.currentWeaponSO;
        if (weapon == null) return;

        if (!weapon.CanZoom) return;

        if (starterInputs.zoom)
        {
            playerFollowCamera.m_Lens.FieldOfView = weapon.ZoomAmount;
            weaponCamera.fieldOfView = weapon.ZoomAmount;

            zoomVignette.SetActive(true);
            controller.ChangeRotationSpeed(weapon.ZoomRotationSpeed);
        }
        else
        {
            playerFollowCamera.m_Lens.FieldOfView = defaultFOV;
            weaponCamera.fieldOfView = defaultFOV;

            zoomVignette.SetActive(false);
            controller.ChangeRotationSpeed(defaultRotationSpeed);
        }
    }

    public void UpdateAmmoUI(int ammo)
    {
        if (ammoText != null)
        {
            ammoText.text = ammo.ToString("D2");
        }
    }
}

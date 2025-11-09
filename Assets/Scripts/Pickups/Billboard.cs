using StarterAssets;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform playerTargetPoint;
    FirstPersonController player;

    void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
        playerTargetPoint = GameObject.FindGameObjectWithTag("CinemachineTarget").transform;
    }
    void LateUpdate()
    {
        if (!player) return;
        transform.LookAt(transform.position + playerTargetPoint.forward);
    }
}
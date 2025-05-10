using System.Collections;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Bullet prefab")]
    [Tooltip("Prefab of the bullet to shoot, should behave as any other bullet")]
    [SerializeField] GameObject bulletPrefab;

    [Tooltip("Where the bullet will spawn")]
    [SerializeField] Transform bulletSpawnPos;

    [Tooltip("Rate of fire in bullets per second")]
    [SerializeField] float rateOfFire = 0.5f;

    private GameObject player;

    [Tooltip("How many degrees the angle can offset to the positive numbers (right)")]
    [SerializeField] private float posAngleClamp = 20f;
    [Tooltip("How many degrees the angle can offset to the negative numbers (left)")]
    [SerializeField] private float negAngleClamp = -20f;

    private float angleOffset;

    private float timestamp;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (posAngleClamp < 0 || negAngleClamp > 0)
        {
            Debug.LogError("posAngleClamp should be positive and negAngleClamp should be negative");
        }
        timestamp = Time.time;
    }

    void Update()
    {
        transform.LookAt(player.transform.position);
        ShootwithCooldown();
    }

    private void ShootwithCooldown()
    {
        if (Time.time - timestamp >= rateOfFire)
        {
            timestamp = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetReference(transform); // when the bullet is more than a distance from its shooter it'll despawn, so it doesn't clutter
        }
    }

    public void SetAngleOffset(float angle)
    {
        if (angle > posAngleClamp) angle = posAngleClamp;
        if (angle < negAngleClamp) angle = negAngleClamp;
        angleOffset = angle;
    }
}

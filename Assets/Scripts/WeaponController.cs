using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public BulletManager bulletManager;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        bulletManager = FindObjectOfType<BulletManager>();
    }

    public void ShootButtonPressed()
    {
        if (bulletManager.CanShoot())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Mantieni la posizione Z fissa per il punto di fuoco
        Vector3 fixedZFirePointPosition = new Vector3(firePoint.position.x, firePoint.position.y, 0);

        GameObject bullet = Instantiate(bulletPrefab, fixedZFirePointPosition, firePoint.rotation);
        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.useGravity = true; // Imposta a true se vuoi che la gravità influenzi il proiettile
        rbBullet.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);

        playerController.AddRecoilAndRotation(bulletForce, firePoint.up);

        bulletManager.ShootBullet(); // Decrementa il conteggio dei proiettili
        FindObjectOfType<GameManager>().ShotFired();
    }
}

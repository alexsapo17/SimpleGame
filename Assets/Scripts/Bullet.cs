using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab; // Assegna il prefab dell'esplosione nell'editor di Unity
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            FindObjectOfType<BulletManager>().ReloadBullet();
            FindObjectOfType<GameManager>().TargetHit(collision.gameObject);

            // Crea l'animazione di esplosione
            GameObject explosion = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(explosion, 2f); // Assicurati di distruggere l'oggetto di esplosione dopo che l'animazione è terminata
        }

        Destroy(gameObject); // Distrugge il proiettile
    }
}

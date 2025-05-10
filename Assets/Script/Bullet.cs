using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Scale")]
    [Tooltip("The scale of the bullet. Will update collider. It's a multiplier")]
    [SerializeField] float size = 1f;
    [Tooltip("How quick it'll travel, in m/s")]
    [SerializeField] float speed = 5f;

    Transform reference;
    [Header("Distance before despawn")]
    [Tooltip("How far away can the bullet travel before despawning")]
    [SerializeField] float distanceBeforeDespawn = 100f;

    void Start()
    {
        transform.localScale.Scale(new Vector3(size, size, size));
    }

    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        if (reference)
        {
            if (Vector3.Distance(transform.position, reference.position) > distanceBeforeDespawn)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetReference(Transform referenceObject)
    {
        reference = referenceObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Player killed by bullet");
        }
    }
}

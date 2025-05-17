using UnityEngine;

public class FlyingTrap : MonoBehaviour
{
    public bool flying = false;
    public Transform parentOfPath; // give it the spawner that spawned the path, it'll follow its children
    private int count = 0;
    private float distance = 1f;
    [SerializeField] float speed = 40f;
    void Update()
    {
        if (flying && parentOfPath)
        {
            Vector3 childPos = parentOfPath.GetChild(count).transform.position;
            if (Vector3.Distance(transform.position, childPos) < distance)
            {
                count++;
                if (count >= parentOfPath.childCount)
                {
                    count = 0;
                }
            }
            Vector3 direction = childPos - transform.position;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Killing the player can be optional if the thing simply pushes it off
    }
}

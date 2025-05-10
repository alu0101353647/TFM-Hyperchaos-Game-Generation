using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class ShuntedHeightLyingTrap : MonoBehaviour
{
    public bool flying = false;
    public Transform parentOfPath; // give it the spawner that spawned the path, it'll follow its children
    private int count = 0;
    private float distance = 0.7f;
    [SerializeField] float speed = 40f;
    [SerializeField] float height = 1f;

    void Update()
    {
        if (flying && parentOfPath)
        {
            Vector3 childPos = parentOfPath.GetChild(count).transform.position;
            if ((childPos.x - transform.position.x) < distance && (childPos.z - transform.position.z) < distance)
            {
                count++;
                if (count >= parentOfPath.childCount)
                {
                    count = 0;
                }
            }
            Vector3 direction = childPos - transform.position;
            direction.Normalize();
            transform.Translate(direction.x * speed * Time.deltaTime, 0, direction.z * speed * Time.deltaTime);
        }
    }

    public float GetHeight()
    {
        return height;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Killing the player can be optional if the thing simply pushes it off
    }
}

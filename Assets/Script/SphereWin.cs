using System;
using UnityEngine;

public class SphereWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Collided with " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            print("You win! :D");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float DestructionTime;

    private bool SecondCollision;

    private void Start()
    {
        Destroy(this.gameObject, DestructionTime);
    }

    private void OnTriggerExit(Collider other)
    {
        SecondCollision = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SecondCollision == true)
        {
            Destroy(this.gameObject, .002f);
        }
    }
}

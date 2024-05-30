using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_System : MonoBehaviour
{
    public float PlayerHP;

    public Transform respawnPoint;

    public int DeathCount;

    void start()
    {
        PlayerHP = 100;
    }

    void Update()
    {
        if (PlayerHP <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
           PlayerHP -= 1;
        }
    }   

    private void Death()
    {
        transform.position = respawnPoint.position;

        PlayerHP = 100;

        DeathCount += 1;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_System : MonoBehaviour
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
            transform.position = respawnPoint.position;

            PlayerHP = 100;

            DeathCount += 1;
        }
    }
}


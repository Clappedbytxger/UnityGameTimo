using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float DestructionTime;

    private void Start()
    {
        Destroy(this.gameObject, .2f);
    }
}

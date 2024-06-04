using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public GameObject Attack;

    public Transform orientation;

    public Transform pos;

    private Vector3 AttackDirection;

    [SerializeField] public bool AttackTrigger;
    [SerializeField] private float AttackRange;

    Rigidbody rb;

    void Update()
    {
        if (AttackTrigger == true)
        {
            Invoke("Attacking", 0.5f);

            AttackTrigger = false;
        }
    }

    private void Attacking()
    {
        AttackDirection = (orientation.forward * 1.2f);

        Instantiate(Attack, pos.position + pos.transform.forward * AttackRange, Quaternion.identity);

        Invoke("Deletion", .2f);
    }

    private void Deletion()
    {
            DestroyImmediate(GameObject.Find("simple_Attack_m(Clone)"), true);
    }


}

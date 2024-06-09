using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringBehavior : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    private Rigidbody TargetRb;

    public GameObject Attack;
    private Rigidbody AttackRb;
    private GameObject AttackClone;

    public Transform orientation;

    public Transform pos;

    private Vector3 AttackDirection;

    [SerializeField] public bool AttackTrigger;
    [SerializeField] private float AttackRange;

    private Vector3 InstantiateVector;

    Rigidbody rb;

    //void Start()
    //{
    //    AttackClone = Attack;
    //}

    void Update()
    {
        if (AttackTrigger == true)
        {
            InstantiateVector[0] = pos.position[0];
            InstantiateVector[1] = pos.position[1];
            InstantiateVector[2] = pos.position[2];

            Instantiate(Attack, pos.position, Quaternion.identity);
            
            AttackTrigger = false;
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            AttackClone = other.gameObject;

            AttackRb = AttackClone.GetComponent<Rigidbody>();

            TargetRb = Target.GetComponent<Rigidbody>();

            Invoke("ProjectileForceAdd", 0.5f);

            AttackTrigger = false;
        }
    }


    private void ProjectileForceAdd()
        {
            AttackDirection = ((pos.position - (Target.transform.position + TargetRb.velocity)) * -1);

            AttackRb.AddForce(AttackDirection[0], AttackDirection[1], AttackDirection[2], ForceMode.Impulse);

            print("1");

            //Invoke("Deletion", .2f);
        }

    //private void Deletion()
    //{
    //    DestroyImmediate(GameObject.Find("simple_Attack_m(Clone)"), true);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body_Rotation : MonoBehaviour
{
    public Vector2 turn;
    public static float mousesens = 0.5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        turn.x += Input.GetAxis("Mouse X") * mousesens;
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
    }
}
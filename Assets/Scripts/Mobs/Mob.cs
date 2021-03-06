using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mob : MonoBehaviour
{
    protected Rigidbody2D rb;

    [SerializeField]
    protected float speed = 400f;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
}

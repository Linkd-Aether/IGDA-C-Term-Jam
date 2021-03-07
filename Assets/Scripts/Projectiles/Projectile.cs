﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
abstract public class Projectile : GFXobject
{   
    // Constants
    const float FADEOUT_TIME = .2f;
    
    // Variables
    public Mob shooter;
    protected float lifetime;

    // Components
    protected Rigidbody2D rb;


    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        SetBaseColor(shooter.baseColor);
    }

    protected virtual void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            StartCoroutine(Disappear());
        }
    }

    protected IEnumerator Disappear() {
        yield return StartCoroutine(FadeOut(FADEOUT_TIME));
        Destroy(this.gameObject);
        yield return null;
    }
}

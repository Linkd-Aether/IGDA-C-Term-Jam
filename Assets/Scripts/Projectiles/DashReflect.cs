using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashReflect : Projectile
{
    // Constants
    const float LIFETIME = .25f;

    // Variables


    protected override void Start() {
        base.Start();

        lifetime = LIFETIME;
    }
}

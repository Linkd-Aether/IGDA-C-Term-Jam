using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    static private float LIFETIME = 1.5f;
    public Mob shooter;
    public Vector2 direction;
    
    private float speed = 6f;
    private float lifetime = LIFETIME;


    void Start() {
        
    }

    private void Update() {
        transform.position += (Vector3) (speed * direction) * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(this.gameObject);
        }
    }

    // Bullet has made contanct with something
    public void Hit() {
        // Explosive SE !!!
        Destroy(this.gameObject);
    }

    
}

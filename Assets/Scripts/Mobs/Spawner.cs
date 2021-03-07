using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Constants
    public static Sprite[] VARIANTS = new Sprite[6];
    public static Color[] COLORS = new Color[] {
        new Color(230f/255f, 50f/255f,  200f/255f,  1),
        new Color(255f/255f, 180f/255f, 50f/255f,   1),
        new Color(255f/255f, 80f/255f,  80f/255f,   1),
        new Color(115f/255f, 100f/255f, 255f/255f,  1),
        new Color(100f/255f, 255f/255f, 100f/255f,  1),
        new Color(30f/255f,  210f/255f, 255f/255f,  1)
    };
    const float SPAWNER_ALPHA = 100/255f;

    // Variables
    static bool[,] existingMobs = new bool[Spawner.VARIANTS.Length, Spawner.COLORS.Length];

    public int variant;
    public int color;
    public bool changes = true;


    void Start()
    {
        for (int i = 0; i < VARIANTS.Length; i++) {
            string spriteName = $"MobSprite{i+1}";
            VARIANTS[i] = (Sprite) Resources.Load($"Sprites/Mobs/{spriteName}", typeof(Sprite));
        }
    }

    // Instantiate a Mob Prefab on the spawner
    public Mob CreateMob(GameObject mobPrefab) {
        GameObject mobObj = Instantiate(mobPrefab);
        mobObj.transform.parent = transform.parent;
        mobObj.transform.position = transform.position;
        mobObj.transform.localScale = Vector3.one;
        
        Mob mob = mobObj.GetComponent<Mob>();
        mob.isAlive = false;
        mob.enabled = false;
        mob.LoadComponents();

        if (changes) SetStyle();

        mob.SetBaseColor(COLORS[color]);
        mob.SetAlpha(0);
        mob.SetSprite(VARIANTS[variant]);

        return mob;
    }

    public void FixStyleForPlayer() {
        SetStyle();
        Color spawnerColor = COLORS[color];
        spawnerColor.a = SPAWNER_ALPHA;
        GetComponent<SpriteRenderer>().color = spawnerColor;
        changes = false;
    }

    private void SetStyle() {
        bool unique = false;
        while (!unique) {
            variant = Random.Range(0, VARIANTS.Length);
            color = Random.Range(0, COLORS.Length);
            unique = !existingMobs[variant, color];
        }
        SetExistence(true);
    }

    public void SetExistence(bool exists) {
        if (changes) {
            // Debug.Log($"V{variant}, C{color}, {exists}");
            existingMobs[variant,color] = exists;
        }
    }
}

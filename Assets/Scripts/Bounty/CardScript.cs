using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    //Bounty Card Resources
    public int Value { get; set; }
    public Mob Target { get; set; }
    SpriteRenderer TargetRenderer;
    Sprite CardBorder;
    Sprite CardBorder1;
    Sprite CardBorder2;

    //Spawn Animation Resources
    public int Position;
    float TargetPos;
    Vector2 TargetVect;

    bool Claimed;

    void Start()
    {
        transform.localPosition = new Vector2(0, -7);
        TargetPos = 10.25F - (Position * 3.25F);
        TargetVect = new Vector2(0, transform.localPosition.y + TargetPos);

        //CardBorder = GetComponent<SpriteRenderer>().sprite;
        CardBorder1 = (Sprite)Resources.Load($"Sprites/BountyCards/BountyCardFinalized", typeof(Sprite));
        CardBorder2 = (Sprite)Resources.Load($"Sprites/BountyCards/BountyCardFinalized2", typeof(Sprite));


        Claimed = false;

        CreateCard();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y < TargetVect.y - 0.1 && !Claimed)
        {
            transform.localPosition = (Vector2.Lerp(transform.localPosition, TargetVect, Time.deltaTime * 3));
        }
    }

    public void AssignTarget(Mob newTarget)
    {
        Target = newTarget;
    }

    void CreateCard()
    {
        if (Target is PlayerMob)
        {
            int streak = Target.GetComponentInParent<PlayerManager>().streak;
            float baseValue = Mathf.Max((streak * streak / 2), 4.5F);
            int unInflatedValue = Mathf.RoundToInt(baseValue + Random.Range(-Mathf.Sqrt(baseValue), Mathf.Sqrt(baseValue) * BountyGen.Intensity));
            Value = unInflatedValue * 1000;

            GetComponent<SpriteRenderer>().sprite = CardBorder2;
        }

        if (Target is EnemyMob)
        {
            int uninflatedValue = Mathf.RoundToInt(Random.Range(4.5F, 10 * BountyGen.Intensity) * BountyGen.Intensity);
            Value = uninflatedValue * 1000;

            GetComponent<SpriteRenderer>().sprite = CardBorder1;
        }

        foreach (Transform child in Target.transform)
        {
            if (child.tag == "GFX")
            {
                 TargetRenderer = child.GetComponent<SpriteRenderer>();
            }
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = TargetRenderer.sprite;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = TargetRenderer.color;
        transform.GetChild(1).GetComponent<DisplayNumber>().DisplayMoney(Value);
    }

}

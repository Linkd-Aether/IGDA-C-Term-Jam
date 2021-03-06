using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyGen : MonoBehaviour
{
    public GameObject CardBoard;
    public GameObject CardPrefab;
    List<PlayerManager> players;
    List<CardScript> cards;
    int FramesSinceLast;
    int FrameTarget;
    float Intensity;
    int IntensityCount;

    // Start is called before the first frame update
    void Start()
    {
        players = GameManager.playerManagers;
        cards = new List<CardScript>();
        FramesSinceLast = 0;
        FrameTarget = 900;
        Intensity = 1;
        IntensityCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(players.Count == 1)
        {
            CardBoard.transform.position = new Vector2(8.5F, 0);
        }
        else
        {
            CardBoard.transform.position = new Vector2(0, 0);
        }


        if (cards.Count < 5)
        {
            foreach (PlayerManager player in players)
            {
                if (player.streak > 10) SummonPl(player);
            }

            if (FramesSinceLast == FrameTarget)
            {
                SummonRand();
                FramesSinceLast = 0;
                FrameTarget = Mathf.RoundToInt(Random.Range(1400, 2200) / (Mathf.Pow(Intensity, 2)));
            }
            FramesSinceLast++;
        }

        if (Intensity < 2)
        {
            IntensityCount++;
            if(IntensityCount == 600)
            {
                Intensity += 0.03333F;
                IntensityCount = 0;
            }
            
        }

        for(int i = 0; i < cards.Count; i++)
        {
            CardScript ComponentTemp = cards[i];
            if(ComponentTemp != null)
            {
                ComponentTemp.Position = i;
            }
        }
    }

    void SummonRand()
    {

        if(Intensity > 1F) //&& Random.value * Intensity > 0.75F)
        {
            int TotalStreaks = 0;
            foreach(PlayerManager player in players)
            {
                TotalStreaks += player.streak; 
            }

            int FullRange = players.Count * 50 + TotalStreaks * 10;
            int result = Random.Range(0, FullRange);
            foreach (PlayerManager player in players)
            {
                result -= 50 + player.streak * 10;
                if (result <= 0) SummonPl(player);
                break;
            }
        }
        else
        {
            //bountyObj = new spawned enemy
        }

    }

    void SummonPl(PlayerManager target)
    {
        GameObject tempCard;
        tempCard = Instantiate(CardPrefab, CardBoard.transform);
        tempCard.GetComponent<CardScript>().Face = target.playerMob.GetComponentInChildren<SpriteRenderer>().sprite;

        float baseValue = Mathf.Max((target.streak * target.streak / 2), 4.5F);
        int unInflatedValue = Mathf.RoundToInt(baseValue + Random.Range(-Mathf.Sqrt(baseValue), Mathf.Sqrt(baseValue) * Intensity));
        tempCard.GetComponent<CardScript>().Value = unInflatedValue * 1000;
    }
    void summonEn(EnemyMob target)
    {

    }
}

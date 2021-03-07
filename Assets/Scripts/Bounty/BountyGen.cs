using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyGen : MonoBehaviour
{
    //Constants
    int FIRST_FRAMES_UNTIL_BOUNTY = 900;

    //Objects + Constructs
    public GameObject CardBoard;
    public GameObject CardPrefab;
    List<PlayerManager> players;
    static List<CardScript> cards;

    //Variables
    public int FramesSinceLast;
    public int FrameTarget;
    public static float Intensity;
    int IntensityCount;

    // Start is called before the first frame update
    void Start()
    {
        players = GameManager.playerManagers;
        cards = new List<CardScript>();
        FramesSinceLast = 0;
        FrameTarget = FIRST_FRAMES_UNTIL_BOUNTY;
        Intensity = 1;
        IntensityCount = 0;
    }

    #region Card Selection
    // Update is called once per frame
    void FixedUpdate()
    {
        //Make sure the bounty card board is in the right spot for the player number
        if(players.Count == 1)
        {
            CardBoard.transform.position = new Vector3(7, -10, -1);
        }
        else
        {
            CardBoard.transform.position = new Vector3(0, -10, -1);
        }

        //Start creating a new card if there is space, and it has reached the frame to do so
        if (cards.Count < 3)
        {
            foreach (PlayerManager player in players)
            {
                if (player.streak > 10 && players.Count > 1) Mark(player);
            }

            if (FramesSinceLast >= FrameTarget)
            {
                MarkRand();
                FramesSinceLast = 0;
                FrameTarget = Mathf.RoundToInt(Random.Range(1400, 2200) / (Mathf.Pow(Intensity, 2)));
            }
            FramesSinceLast++;
        }

        //Raise the intensity value
        if (Intensity < 2)
        {
            IntensityCount++;
            if(IntensityCount == 600)
            {
                Intensity += 0.03333F;
                IntensityCount = 0;
            }
            
        }

        //Update the position of the cards on the board
        for(int i = 0; i < cards.Count; i++)
        {
            CardScript ComponentTemp = cards[i];
            if(ComponentTemp != null)
            {
                ComponentTemp.Position = i;
            }
        }
    }

    
    void MarkRand()
    {
        if(players.Count > 1 && Intensity >= 1F && Random.value * Intensity > 0.75F)
        {
            int TotalStreaks = 0;
            foreach(PlayerManager player in players)
            {
                if (cards.FindIndex(c => c.Target.Equals(player.mob)) == -1)
                TotalStreaks += player.streak;
            }

            int FullRange = players.Count * 50 + TotalStreaks * 10;
            int result = Random.Range(0, FullRange);
            foreach (PlayerManager player in players)
            {
                if (cards.FindIndex(c => c.Target.Equals(player.mob)) == -1)
                {
                    result -= 50 + player.streak * 10;
                    if (result <= 0)
                    {
                        if (player.mob.isAlive)
                            Mark(player);
                        break;
                    }
                }
            }
        }
        else
        {
            int activeEnemyBounties = 0;
            foreach(CardScript card in cards)
            {
                if (card.Target is EnemyMob) activeEnemyBounties++;
            }

            if (activeEnemyBounties < GameManager.GetEnemies().Length && GameManager.GetEnemies().Length > 0)
            {
                int unique = 0;
                EnemyManager enemy = null;

                while (unique != -1)
                {
                    int rand = Random.Range(0, GameManager.GetEnemies().Length);
                    enemy = GameManager.GetEnemies()[rand];
                    if(enemy.mob.isAlive) unique = cards.FindIndex(c => c.Target.Equals(enemy.mob));
                }
                Mark(enemy);
            }
        }

    }
    #endregion
    void Mark(MobManager target)
    {
        GameObject card;
        card = Instantiate(CardPrefab, CardBoard.transform);
        card.GetComponent<CardScript>().AssignTarget(target.GetComponentInChildren<Mob>());
        target.GetComponentInChildren<Mob>().isBounty = true;
        cards.Add(card.GetComponent<CardScript>());

    }

    static public List<CardScript> GetCards()
    {
        return cards;
    }

    static public int ClaimCard(int index)
    {
        CardScript removed = cards[index];
        int Reward = removed.Value;
        cards.RemoveAt(index);
        Destroy(removed.transform.gameObject);
        return Reward;
    }
}

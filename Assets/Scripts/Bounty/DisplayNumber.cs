using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNumber : MonoBehaviour
{
    Sprite[] sprites = new Sprite[10];

    public Sprite Font0;
    public Sprite Font1;
    public Sprite Font2;
    public Sprite Font3;
    public Sprite Font4;
    public Sprite Font5;
    public Sprite Font6;
    public Sprite Font7;
    public Sprite Font8;
    public Sprite Font9;

    public Sprite FontX;
    public Sprite FontS;


    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            string spriteName = $"Font{i}";
            sprites[i] = (Sprite)Resources.Load($"Sprites/Font/{spriteName}", typeof(Sprite));
        }
    }

    private void Display(string toDisplay)
    {
        if (sprites[0] != Font0) Start();

        char[] chars = toDisplay.ToCharArray();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (chars[i] == 'x') transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = FontX;
            if (chars[i] == '$') transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = FontS;
            if (char.IsDigit(chars[i]))
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[int.Parse(char.ToString(chars[i]))];
            }
        }
    }

    public void DisplayMoney(int value)
    {
        string toDisplay = "";
        for (int i = 0; i < transform.childCount - 1 - value.ToString().Length; i++)
            toDisplay += " ";
        toDisplay += "$" + value.ToString();
        Display(toDisplay);
    }

    public void DisplayMultiplier(int value)
    {
        string toDisplay = "x";
        toDisplay += value.ToString();
        for (int i = 0; i < transform.childCount - 1 - value.ToString().Length; i++)
            toDisplay += " ";
        Display(toDisplay);
    }
}

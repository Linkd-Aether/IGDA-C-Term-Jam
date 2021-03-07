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
        if (sprites[0] != Font0) {
            sprites[0] = Font0;
            sprites[1] = Font1;
            sprites[2] = Font2;
            sprites[3] = Font3;
            sprites[4] = Font4;
            sprites[5] = Font5;
            sprites[6] = Font6;
            sprites[7] = Font7;
            sprites[8] = Font8;
            sprites[9] = Font9;
        }
    }

    private void Display(string toDisplay)
    {
        if (sprites[0] != Font0) Start();

        char[] chars = toDisplay.ToCharArray();

        //Debug.Log("numch = " + transform.childCount);
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
        Debug.Log("\""+toDisplay+ "\"");
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

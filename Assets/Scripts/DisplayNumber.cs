using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNumber : MonoBehaviour
{
    static Sprite[] digitSprites;

    public Sprite FontX;
    public Sprite FontS;


    private void Awake()
    {
        digitSprites = FontUtils.getDigitSprites();
    }

    private void Display(string toDisplay)
    {
        char[] chars = toDisplay.ToCharArray();

        //Debug.Log("numch = " + transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (chars[i] == 'x') transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = FontX;
            if (chars[i] == '$') transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = FontS;
            if (char.IsDigit(chars[i]))
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = digitSprites[int.Parse(char.ToString(chars[i]))];
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class FontUtils
{
    static private Sprite[] digitSprites = new Sprite[10];

    static public Sprite[] getDigitSprites() {
        if (digitSprites[0] == null) {
            for (int i = 0; i < 10; i++) {
                digitSprites[i] = (Sprite) Resources.Load($"Sprites/Font/Font{i}", typeof(Sprite));
            }
        }
        return digitSprites;
    }

    static public Sprite DigitToFont(int digit) {
        if (digit < 0 || digit > 9) {
            Debug.LogError("DigitToFont is for converting single digits to the font.");
            return null;
        }
        return getDigitSprites()[digit];
    }
}

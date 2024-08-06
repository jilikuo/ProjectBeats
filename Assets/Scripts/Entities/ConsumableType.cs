using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableType : MonoBehaviour
{
    public enum Category
    {
        Health = 0,
        Mana = 1,
        Exp = 2,
        Gold = 3,
        Card = 4,
    }
    public enum Size
    {
        Small = 1,
        Medium = 2,
        Big = 3,
        Super = 4,
        Legendary = 5
    }

    public Category category;
    public Size size;
}

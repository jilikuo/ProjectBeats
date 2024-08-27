namespace Jili.StatSystem.CardSystem
{
    public enum CardCategory
    {
        None = 0,
        // Weapons 
        Minigun = 1001,
        Shotgun = 1002,

        Stat = 2000,
        Attribute = 3000
    }
    public enum CardRarity
    {
        Inferior,                 //common
        Common,                   //common
        Unusual,                  //bronze
        Rare,                     //bronze
        Epic,                     //silver
        Legendary,                //silver
        Mythic,                   //gold
        Godly                     //gold
    }
    public enum CardLevel
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Max
    }
}  

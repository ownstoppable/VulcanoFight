using System.Collections.Generic;
public class Item {
    Dictionary<StatName, BaseStat> modifiedStats;
    public Dictionary<StatName, BaseStat> GetModifiedStats
    {
        get { return modifiedStats; }
    }

    public Item()
    {
        modifiedStats = new Dictionary<StatName, BaseStat>();
    }

    public Item(ItemName name)
    {
        modifiedStats = new Dictionary<StatName, BaseStat>();
        switch (name)
        {
            case ItemName.Boots:
                modifiedStats.Add(StatName.Speed, new BaseStat(5));
                break;
            case ItemName.Staff:
                modifiedStats.Add(StatName.Damage, new BaseStat(50));
                break;
            case ItemName.Cloak:
                modifiedStats.Add(StatName.HPReg, new BaseStat(5));
                break;
            default:
                break;
        }
    }


}

public enum ItemName { 
    Boots,
    Staff,
    Cloak
}
using System.Collections.Generic;
using UnityEngine;
public class Item {
    private Dictionary<StatName, BaseStat> _modifiedStats;
    public Dictionary<StatName, BaseStat> GetModifiedStats
    {
        get { return _modifiedStats; }
    }

    private Texture2D _icon;
    public Texture2D Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }

    

    public Item()
    {
        _modifiedStats = new Dictionary<StatName, BaseStat>();
    }

    public Item(ItemName name, Texture2D ic)
    {
        _modifiedStats = new Dictionary<StatName, BaseStat>();
        _icon = ic;
        switch (name)
        {
            case ItemName.Boots:
                _modifiedStats.Add(StatName.Speed, new BaseStat(5));
                break;
            case ItemName.Staff:
                _modifiedStats.Add(StatName.Damage, new BaseStat(50));
                break;
            case ItemName.Cloak:
                _modifiedStats.Add(StatName.HPReg, new BaseStat(5));
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
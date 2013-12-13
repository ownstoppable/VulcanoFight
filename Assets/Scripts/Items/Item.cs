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

	private string _name;
	public string Name{
		get{return _name;}
		set{_name = value;}
	}

    private string _tooltip;
    public string Tooltip
    {
        get { return _tooltip; }
        set { _tooltip = value; }
    }


    public Item()
    {
        _modifiedStats = new Dictionary<StatName, BaseStat>();
    }

    public Item(ItemName name, Texture2D ic, string itemName, string tooltip)
    {
        _modifiedStats = new Dictionary<StatName, BaseStat>();
        _icon = ic;
		_name = itemName;
        _tooltip = tooltip;
        switch (name)
        {
            case ItemName.HauntersBoots:
                _modifiedStats.Add(StatName.Speed, new BaseStat(3));
                break;
            case ItemName.CarversStaff:
                _modifiedStats.Add(StatName.Damage, new BaseStat(50));
                break;
            case ItemName.DivineCloak:
                _modifiedStats.Add(StatName.HPReg, new BaseStat(5));
                break;
			case ItemName.VitalityBall:
                _modifiedStats.Add(StatName.HP, new BaseStat(200));
				break;
            case ItemName.ApparatusofMagic:
                _modifiedStats.Add(StatName.CDR, new BaseStat(-0.1f));
                break;
            case ItemName.SturdyPendant:
                _modifiedStats.Add(StatName.CSpeed, new BaseStat(-0.1f));
                break;
            case ItemName.ShieldofCunning:
                _modifiedStats.Add(StatName.KBResist, new BaseStat(15));
                break;
            case ItemName.EvilStrongApparatus:
                _modifiedStats.Add(StatName.KBPower, new BaseStat(20));
                break;
            case ItemName.MeteoricPlateMail:
                _modifiedStats.Add(StatName.Armor, new BaseStat(-0.15f));
                break;
            default:
                break;
        }
    }


}

public enum ItemName { 
    HauntersBoots,
    CarversStaff,
    DivineCloak,
	VitalityBall,
    ApparatusofMagic,
    SturdyPendant,
    ShieldofCunning,
    EvilStrongApparatus,
    MeteoricPlateMail
}
public class BaseStat {
    private float _baseValue;
    private float _buffValue;
    private float _curValue;

    public BaseStat()
    {
        _baseValue = 0;
        _curValue = _baseValue;
        _buffValue = 0;
    }

    public BaseStat(float bV)
    {
        _baseValue = bV;
        _curValue = _baseValue;
        _buffValue = 0;
    }

    public float CurValue
    {
        get {
            if (_curValue > TotalValue) _curValue = TotalValue;
            if (_curValue < 0) _curValue = 0;

            return _curValue; 
        }
        set { _curValue = value; }
    }

    public float TotalValue {
        get { return _baseValue + _buffValue; }
    }

    public float BuffValue
    {
        get { return _buffValue; }
        set { _buffValue = value; }
    }

    public void ChangeCurTotal(float value) {
        _buffValue += value;
        _curValue += value;
    }

    public void ResetCurValue() {
        _curValue = TotalValue;
    }
}

public enum StatName { 
    HP, 
    Speed,
    CDR,
    HPReg,
    CSpeed,
    KBResist, 
    KBPower, 
    Damage, 
    Armor,
    Gold,
    Kills,
    Mass
}

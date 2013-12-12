using UnityEngine;
using System.Collections;

public class Skill {

    protected float cooldown;
    public float getCooldown
    {
        get { return cooldown; }
    }

    protected float lastUsed;
    public float LastUsed
    {
        get { return lastUsed; }
        set { lastUsed = value; }
    }

    protected float castTime;

    public float CastTime
    {
        get { return castTime; }
        set { castTime = value; }
    }

    public Skill() {
        castTime = 0.1f;
    }
}

public enum SkillName
{
    Fireball,
    Teleport,
    Homingball,
    SelfExplosion,
    MeteorBlast,
    EtherealWalk,
    None
}
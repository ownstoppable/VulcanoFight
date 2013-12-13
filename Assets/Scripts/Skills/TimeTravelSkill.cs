using UnityEngine;
using System.Collections;

public class TimeTravelSkill : Skill {
    private float castHP;
    private float castMass;
    private Vector3 castPosition;
    private float duration;

    public TimeTravelSkill(float cd, float dur)
    {
        cooldown = cd;
        lastUsed = Time.time - cd;
        duration = dur;
    }

    public IEnumerator Launch(GameObject character)
    {
        BaseCharacter bc = character.GetComponent<BaseCharacter>();
        GameObject ttEffect = GameObject.Instantiate(Resources.Load("Skills/TimeTravel"), Vector3.zero, Quaternion.identity) as GameObject;
        ttEffect.transform.parent = character.transform;
        ttEffect.transform.localPosition = Vector3.zero;
        castHP = bc.GetBaseStat(StatName.HP).CurValue;
        castMass = bc.GetBaseStat(StatName.Mass).CurValue;
        castPosition = character.transform.position;

        yield return new WaitForSeconds(duration);

        character.transform.position = castPosition;
        bc.GetBaseStat(StatName.HP).CurValue = castHP;
        bc.GetBaseStat(StatName.Mass).CurValue = castMass;
        GameObject.Destroy(ttEffect);
    }
}

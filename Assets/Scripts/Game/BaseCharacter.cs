using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseCharacter : MonoBehaviour {
    protected Transform myTransform;
    protected Transform camTransform;
    protected Vector3 destinationPosition;

    protected Dictionary<StatName, BaseStat> characterStats;

    public SkillName skillBeingCast;

    protected Vector3 impact = Vector3.zero;
    protected CharacterController characterC;

    protected float lavaDamage = 30;
    protected float lastLavaDamage;
    public bool onLava;

    public Transform nameLabel;

    protected Dictionary<SkillName, Skill> skills;

    protected Dictionary<float, GameObject> hitsReceived;

    public bool attackPossible;

    protected Dictionary<ItemName, Item> inventory;


    public enum PrizeType { 
        Kill,
        Assist,
        RoundWin
    }

    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / characterStats[StatName.Mass].CurValue;
        destinationPosition = myTransform.position;
    }

    public float getPercHP()
    {
        BaseStat hp = characterStats[StatName.HP];

        if (hp.CurValue < 0)
            return 0;
        else if (hp.CurValue > hp.TotalValue)
            return 1;

        return hp.CurValue / hp.TotalValue;
    }

    public void ReceiveDamage(float damage, float knockback, string enemy)
    {
        float testDamage = damage;
        knockback = Mathf.Clamp(knockback + characterStats[StatName.KBResist].CurValue, 0.1f, 1);
        damage = Mathf.Clamp(damage * characterStats[StatName.Armor].CurValue, 0, Mathf.Infinity);

        if (characterStats[StatName.HP].CurValue - damage <= 0)
        {
            GameObject killer = LastAssist(5);
            if (killer != null) {
                ReceiveDamage(testDamage, knockback, killer);
                return;
            }
        }
        
        characterStats[StatName.Mass].CurValue *= knockback;
        characterStats[StatName.HP].CurValue -= damage;

        if (characterStats[StatName.HP].CurValue <= 0)
        {
            GameObject killTextObj = Resources.Load("UI/KillText") as GameObject;
            GameObject killText = Instantiate(killTextObj, killTextObj.transform.position, Quaternion.identity) as GameObject;
            killText.GetComponent<GUIText>().text = gameObject.name + " was killed by " + enemy;
            Destroy(killText, 3);
            List<GameObject> assists = new List<GameObject>();
            foreach (KeyValuePair<float, GameObject> pair in hitsReceived)
            {
                if (Time.time - pair.Key <= 10 && !assists.Contains(pair.Value))
                    assists.Add(pair.Value);
            }

            if (assists.Count > 0)
            {
                GameObject assistTextObj = Resources.Load("UI/AssistText") as GameObject;
                GameObject assistText = Instantiate(assistTextObj, assistTextObj.transform.position, Quaternion.identity) as GameObject;
                string assistNames = "Assists: " + assists[0].name;
                assists[0].GetComponent<BaseCharacter>().GoldPrize(PrizeType.Assist);
                for (int i = 1; i < assists.Count; i++)
                {
                    assistNames += ", " + assists[i].name;
                    assists[i].GetComponent<BaseCharacter>().GoldPrize(PrizeType.Assist);
                }
                assistText.GetComponent<GUIText>().text = assistNames;
                Destroy(assistText, 3);
            }

            //Destroy(gameObject);
            if (gameObject.tag == "Player")
            {
                ((PlayerController)this).AimUI.transform.parent = gameObject.transform;
                Time.timeScale = 4;
            }
            GoIdle();

            List<GameObject> characters = new List<GameObject>(GameManager.Instance.GetAliveCharacters());
            characters.Remove(gameObject);

            if (characters.Count == 1) {
                Time.timeScale = 1;
                GameObject victTextObj = Resources.Load("UI/VictoriousText") as GameObject;
                GameObject victText = Instantiate(victTextObj, victTextObj.transform.position, Quaternion.identity) as GameObject;
                victText.GetComponent<GUIText>().text = characters[0].name + " was the winner of this round";
                characters[0].GetComponent<BaseCharacter>().GoldPrize(PrizeType.RoundWin);
                Destroy(victText, 4);
                if (characters[0].tag == "Player") characters[0].GetComponent<PlayerController>().AimUI.transform.parent = characters[0].transform;
                characters[0].GetComponent<BaseCharacter>().GoIdle();
                GameManager.Instance.FinishRoundI();
            }
        }

        
    }

    public void ReceiveDamage(float damage, float knockback, GameObject enemy)
    {
        knockback = Mathf.Clamp(knockback + characterStats[StatName.KBResist].CurValue, 0.1f, 1);
        damage = Mathf.Clamp(damage * characterStats[StatName.Armor].CurValue, 0, Mathf.Infinity);

        characterStats[StatName.Mass].CurValue *= knockback;
        characterStats[StatName.HP].CurValue -= damage;

        hitsReceived.Add(Time.time, enemy);

        

        if (characterStats[StatName.HP].CurValue <= 0)
        {
            GameObject killTextObj = Resources.Load("UI/KillText") as GameObject;
            GameObject killText = Instantiate(killTextObj, killTextObj.transform.position, Quaternion.identity) as GameObject;
            killText.GetComponent<GUIText>().text = gameObject.name + " was killed by " + enemy.name;
            Destroy(killText, 3);

            List<GameObject> assists = new List<GameObject>();
            foreach (KeyValuePair<float, GameObject> pair in hitsReceived)
            {
                if (Time.time - pair.Key <= 10 && !assists.Contains(pair.Value))
                    assists.Add(pair.Value);
            }
            assists.Remove(enemy);
            if (assists.Count > 0) {
                GameObject assistTextObj = Resources.Load("UI/AssistText") as GameObject;
                GameObject assistText = Instantiate(assistTextObj, assistTextObj.transform.position, Quaternion.identity) as GameObject;
                string assistNames = "Assists: " + assists[0].name;
                assists[0].GetComponent<BaseCharacter>().GoldPrize(PrizeType.Assist);
                for (int i = 1; i < assists.Count; i++)
                {
                    assistNames += ", " + assists[i].name;
                    assists[i].GetComponent<BaseCharacter>().GoldPrize(PrizeType.Assist);
                }
                assistText.GetComponent<GUIText>().text = assistNames;
                Destroy(assistText, 3);
            }

            enemy.GetComponent<BaseCharacter>().GetBaseStat(StatName.Kills).ChangeCurTotal(1);
            enemy.GetComponent<BaseCharacter>().GoldPrize(PrizeType.Kill);

            //Destroy(gameObject);
            if (gameObject.tag == "Player")
            {
                ((PlayerController)this).AimUI.transform.parent = gameObject.transform;
                Time.timeScale = 4;
            }
            GoIdle();

            List<GameObject> characters = new List<GameObject>(GameManager.Instance.GetAliveCharacters());
            characters.Remove(gameObject);

            if (characters.Count == 1)
            {
                Time.timeScale = 1;
                GameObject victTextObj = Resources.Load("UI/VictoriousText") as GameObject;
                GameObject victText = Instantiate(victTextObj, victTextObj.transform.position, Quaternion.identity) as GameObject;
                victText.GetComponent<GUIText>().text = characters[0].name + " wins the round";
                characters[0].GetComponent<BaseCharacter>().GoldPrize(PrizeType.RoundWin);
                Destroy(victText, 4);
                if (characters[0].tag == "Player") characters[0].GetComponent<PlayerController>().AimUI.transform.parent = characters[0].transform;
                characters[0].GetComponent<BaseCharacter>().GoIdle();
                GameManager.Instance.FinishRoundI();
            }
        }


    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        switch (hit.gameObject.tag)
        {
            case "Platform":
                if (onLava) onLava = false;
                break;
            case "Lava":
                if (!onLava) onLava = true;
                break;
            default:
                break;
        }
    }

    void HPRegeneration() {
        characterStats[StatName.HP].CurValue += characterStats[StatName.HPReg].CurValue;
    }

    public void HitGold(SkillName skill) {
        switch (skill)
        {
            case SkillName.Fireball:
                characterStats[StatName.Gold].ChangeCurTotal(20);
                break;
            case SkillName.Homingball:
                characterStats[StatName.Gold].ChangeCurTotal(20);
                break;
            case SkillName.SelfExplosion:
                characterStats[StatName.Gold].ChangeCurTotal(10);
                break;
            default:
                break;
        }
    }

    public void GoldPrize(PrizeType type) {
        switch (type)
        {
            case PrizeType.Kill:
                characterStats[StatName.Gold].ChangeCurTotal(80);
                break;
            case PrizeType.Assist:
                characterStats[StatName.Gold].ChangeCurTotal(30);
                break;
            case PrizeType.RoundWin:
                characterStats[StatName.Gold].ChangeCurTotal(100);
                break;
            default:
                break;
        }
    }

    public void GoIdle() {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    public void InitializeSkills() {
        //Fill Skill list
        skills = new Dictionary<SkillName, Skill>();
        skills.Add(SkillName.Fireball, new FireballSkill(2, 200, 10, 10, 0.5f));
        skills.Add(SkillName.SelfExplosion, new SelfExplosionSkill(5, 100, 30, 4, 0.5f, 50, 0.7f));
    }

    public bool HasSkill(SkillName skill) {
        return skills.ContainsKey(skill);
    }

    public void AddSkill(SkillName skill) {
        switch (skill)
        {
            case SkillName.Teleport:
                skills.Add(SkillName.Teleport, new TeleportSkill(10, 10));                
                break;
            case SkillName.Homingball:
                skills.Add(SkillName.Homingball, new HomingBallSkill(1, 200, 10, 5, 10, 0.5f));
                break;
            default:
                break;
        }
    }

    public void AddItem(ItemName item) {
        switch (item)
        {
            case ItemName.Boots:
                break;
            case ItemName.Staff:
                break;
            case ItemName.Cloak:
                break;
            default:
                break;
        }
    }

    public bool HasGold(int gold) {
        if (characterStats[StatName.Gold].CurValue >= gold)
        {
            characterStats[StatName.Gold].CurValue -= gold;
            return true;
        }
        else return false;
    }

    protected void InitializeStats() {
        characterStats = new Dictionary<StatName, BaseStat>();

        characterStats.Add(StatName.HP, new BaseStat(1000));
        characterStats.Add(StatName.Armor, new BaseStat());
        characterStats.Add(StatName.CDR, new BaseStat(1));
        characterStats.Add(StatName.CSpeed, new BaseStat(1));
        characterStats.Add(StatName.Damage, new BaseStat());
        characterStats.Add(StatName.HPReg, new BaseStat(5));
        characterStats.Add(StatName.KBPower, new BaseStat());
        characterStats.Add(StatName.KBResist, new BaseStat());
        characterStats.Add(StatName.Speed, new BaseStat(5));
        characterStats.Add(StatName.Gold, new BaseStat());
        characterStats.Add(StatName.Mass, new BaseStat(3));
        characterStats.Add(StatName.Kills, new BaseStat());
    }

    public BaseStat GetBaseStat(StatName name) {
        return characterStats[name];
    }

    public GameObject LastAssist(float timeSpan) {
        float lastTime = hitsReceived.Keys.OrderByDescending(x => x).First();
        if (Time.time - lastTime <= timeSpan) {
            return hitsReceived[lastTime];
        }
        else return null;
    }

    public bool HasItem(ItemName name) {
        return inventory.ContainsKey(name);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UI : MonoBehaviour {
    public Texture2D hpText;
    private PlayerController pC;
	public Material fireballMat;
	public Material selfExplMat;
	public Material teleportMat;
	public Material homingBallMat;
    public Texture2D emptyIcon;
	public Texture2D selectedIcon;
    public GUISkin mySkin;


	// Use this for initialization
	void Start () {
        pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    void OnGUI() {
        GUI.skin = mySkin;

        //Game Time
        if (GameManager.Instance.GetTimePassed() != -1)
        {
            TimeSpan ts = TimeSpan.FromSeconds(GameManager.Instance.GetTimePassed());
            GUI.Label(new Rect(Screen.width - 100, 20, 60, 30), string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds), "GLabel");

        }
        //Skills
		if(pC.attackPossible){
			if (pC.HasSkill(SkillName.Fireball))
			{
				float skillCD = 1 - pC.SkillCooldown(SkillName.Fireball);
				GUI.DrawTexture(new Rect(20, Screen.height - 84, 64, 64), pC.skillBeingCast == SkillName.Fireball ? selectedIcon : emptyIcon);
				if(Event.current.type.Equals(EventType.Repaint)){
					fireballMat.SetFloat("_Cutoff", skillCD);
					fireballMat.SetColor("_Color", skillCD <= 0 ? Color.white : Color.gray);
				}
				Graphics.DrawTexture(new Rect(22.4f, Screen.height - 81.6f, 59.2f, 59.2f), fireballMat.GetTexture("_MainTex"), fireballMat);
				if(GUI.Button(new Rect(20, Screen.height - 84, 64, 64), "", "GButton"))
					pC.PrepSkill(SkillName.Fireball);
			}
			if(pC.HasSkill(SkillName.SelfExplosion)){
				float skillCD = 1 - pC.SkillCooldown(SkillName.SelfExplosion);
				GUI.DrawTexture(new Rect(20, Screen.height - 158, 64, 64), pC.skillBeingCast == SkillName.SelfExplosion ? selectedIcon : emptyIcon);
				if(Event.current.type.Equals(EventType.Repaint)){
					selfExplMat.SetFloat("_Cutoff", skillCD);
					selfExplMat.SetColor("_Color", skillCD <= 0 ? Color.white : Color.gray);
				}
				Graphics.DrawTexture(new Rect(22.4f, Screen.height - 155.6f, 59.2f, 59.2f), selfExplMat.GetTexture("_MainTex"), selfExplMat);
				if(GUI.Button(new Rect(20, Screen.height - 158, 64, 64), "", "GButton"))
					pC.PrepSkill(SkillName.SelfExplosion);
			}
			if(pC.HasSkill(SkillName.Teleport)){
				float skillCD = 1 - pC.SkillCooldown(SkillName.Teleport);
				GUI.DrawTexture(new Rect(20, Screen.height - 232, 64, 64), pC.skillBeingCast == SkillName.Teleport ? selectedIcon : emptyIcon);
				if(Event.current.type.Equals(EventType.Repaint)){
					teleportMat.SetFloat("_Cutoff", skillCD);
					teleportMat.SetColor("_Color", skillCD <= 0 ? Color.white : Color.gray);
				}
				Graphics.DrawTexture(new Rect(22.4f, Screen.height - 229.6f, 59.2f, 59.2f), teleportMat.GetTexture("_MainTex"), teleportMat);
				if(GUI.Button(new Rect(20, Screen.height - 232, 64, 64), "", "GButton"))
					pC.PrepSkill(SkillName.Teleport);
			}
			if(pC.HasSkill(SkillName.Homingball)){
				float skillCD = 1 - pC.SkillCooldown(SkillName.Homingball);
				GUI.DrawTexture(new Rect(20, Screen.height - 306, 64, 64), pC.skillBeingCast == SkillName.Homingball ? selectedIcon : emptyIcon);
				if(Event.current.type.Equals(EventType.Repaint)){
					homingBallMat.SetFloat("_Cutoff", skillCD);
					homingBallMat.SetColor("_Color", skillCD <= 0 ? Color.white : Color.gray);
				}
				Graphics.DrawTexture(new Rect(22.4f, Screen.height - 303.6f, 59.2f, 59.2f), homingBallMat.GetTexture("_MainTex"), homingBallMat);
				if(GUI.Button(new Rect(20, Screen.height - 306, 64, 64), "", "GButton"))
					pC.PrepSkill(SkillName.Homingball);
			}
		}

        //Player UI
        GUI.Box(new Rect(20, 20, 200, 90), "");

        //Name
        GUI.Label(new Rect(25, 25, 80, 20), "Player", "GLabelSmall");

        //HP
        GUI.Label(new Rect(25, 55, 20, 20), "HP", "GLabelSmall");
        GUI.Box(new Rect(60, 55, 155, 20), "");
        GUI.DrawTexture(new Rect(65, 57.25f, pC.getPercHP() * 145, 15), hpText);

        //Gold
        GUI.Label(new Rect(25, 80, 80, 20), "Gold " + pC.GetBaseStat(StatName.Gold).CurValue, "GLabelSmall");

        //Kills
        GUI.Label(new Rect(120, 80, 80, 20), "Kills " + pC.GetBaseStat(StatName.Kills).CurValue, "GLabelSmall");

        //Enemy UI
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        //GUI.Box(new Rect(Screen.width - 170, Screen.height * 0.25f, 150, 5 + enemies.Count * 55 + 5), "");

        for (int i = 0; i < enemies.Count; i++)
        {
            GUI.Box(new Rect(Screen.width - 165, Screen.height * 0.25f + 5 + i * 55, 140, 50), "");

            //Enemy name
            GUI.Label(new Rect(Screen.width - 160, Screen.height * 0.25f + 5 + i * 55 + 5, 130, 20), enemies[i].name, "GLabelSmall");

            //Enemy HP
            GUI.Box(new Rect(Screen.width - 160, Screen.height * 0.25f + 5 + i * 55 + 5 + 22.5f, 130, 18), "");

            GUI.DrawTexture(new Rect(Screen.width - 155, Screen.height * 0.25f + 5 + i * 55 + 5 + 22.5f + 2f, enemies[i].GetComponent<AI>().getPercHP() * 120, 13), hpText);
        }

        //Inventory UI
        GUI.Box(new Rect(Screen.width - 141, Screen.height - 99, 121, 79), "");
        int f = 0;
        foreach (KeyValuePair<ItemName, Item> item in pC.GetInventory)
        {
            int offsetx = f > 2 ? f - 3 : f;
            int offsety = f > 2 ? 1 : 0;
            GUI.Label(new Rect(Screen.width - 136 + 5 + offsetx * 37, Screen.height - 99 + 5 + offsety * 37, 32, 32), item.Value.Icon);
            f++;
        }
        for (int i = f; i < 6; i++)
        {
            int offsetx = i > 2 ? i - 3 : i;
            int offsety = i > 2 ? 1 : 0;
            GUI.Label(new Rect(Screen.width - 136 + 5 + offsetx * 37, Screen.height - 99 + 5 + offsety * 37, 32, 32), emptyIcon);
        }
    }

}

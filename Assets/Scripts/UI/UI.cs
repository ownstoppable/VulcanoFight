using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UI : MonoBehaviour {
    public Texture2D hpText;
    private PlayerController pC;
    public Texture2D fireballIcon;
    public Texture2D teleportIcon;
    public Texture2D homingBallIcon;
    public Texture2D selfExpIcon;
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
        if (pC.HasSkill(SkillName.Fireball) && pC.attackPossible && GUI.Button(new Rect(20, Screen.height - 84, 64, 64), fireballIcon))
        {
            pC.PrepSkill(SkillName.Fireball);
        }
        if (pC.HasSkill(SkillName.SelfExplosion) && pC.attackPossible && GUI.Button(new Rect(20, Screen.height - 158, 64, 64), selfExpIcon))
        {
            pC.PrepSkill(SkillName.SelfExplosion);
        }
        if (pC.HasSkill(SkillName.Teleport) && pC.attackPossible && GUI.Button(new Rect(20, Screen.height - 232, 64, 64), teleportIcon))
        {
            pC.PrepSkill(SkillName.Teleport);
        }
        if (pC.HasSkill(SkillName.Homingball) && pC.attackPossible && GUI.Button(new Rect(20, Screen.height - 306, 64, 64), homingBallIcon))
        {
            pC.PrepSkill(SkillName.Homingball);
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
    }

}

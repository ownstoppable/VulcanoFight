using UnityEngine;
using System.Collections;
using System;

public class ShopUI : MonoBehaviour {

    private float shopTime;
    private float shopInitTime;
    public GUISkin mySkin;
    private PlayerController pC;
    public Texture2D homingBallIcon;
    public Texture2D teleportIcon;
    public Texture2D staffIcon;
    public Texture2D bootsIcon;

    private enum MenuType
    {
        MainMenu,
        Skills,
        NewSkills,
        LearnedSkills,
        Items
    }
    private MenuType currentMenu;

    void Start() {
        shopTime = 90;
        shopInitTime = Time.time;
        currentMenu = MenuType.MainMenu;
        pC = GameOptions.Instance.playerObj.GetComponent<PlayerController>();
    }

    void OnGUI() {

        if ((shopTime - (Time.time - shopInitTime)) <= 0) {
            Application.LoadLevel(1);
        }

        GUI.skin = mySkin;

        //Shop Time
        TimeSpan ts = TimeSpan.FromSeconds(shopTime - (Time.time - shopInitTime));
        GUI.Label(new Rect(Screen.width - 100, 20, 60, 30), string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds), "GLabel");

        //Player
        GUI.Label(new Rect(20, 20, 100, 40), "Player", "GLabelLeft");
        GUI.Label(new Rect(20, 70, 200, 40), "Gold " + pC.GetBaseStat(StatName.Gold).CurValue, "GLabelLeft");

        switch (currentMenu)
        {
            case MenuType.MainMenu:
                GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.25f));
                    GUILayout.BeginVertical();
                        if (GUILayout.Button("Skills", "GButton")) {
                            currentMenu = MenuType.Skills;
                        }
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Items", "GButton")) {
                            currentMenu = MenuType.Items;
                        }
                        GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                GUILayout.EndArea();
                break;
            case MenuType.Skills:
                GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.25f));
                    GUILayout.BeginVertical();
                        if (GUILayout.Button("New Skills", "GButton")) {
                            currentMenu = MenuType.NewSkills;
                        }
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Learned Skills", "GButton")) {
                            currentMenu = MenuType.LearnedSkills;
                        }
                        GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Back", "GButton"))
                {
                    currentMenu = MenuType.MainMenu;
                }
                GUILayout.EndArea();
                break;
            case MenuType.NewSkills:
                bool first = true;
                GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.4f));
                GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (!pC.HasSkill(SkillName.Homingball))
                    {
                        if (!first) GUILayout.FlexibleSpace();
                            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Homing Ball", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button(homingBallIcon, GUILayout.MaxWidth(64), GUILayout.MaxHeight(64))) {
                                    if (pC.HasGold(200))
                                    {
                                        pC.AddSkill(SkillName.Homingball);
                                    }
                                    else {
                                        Debug.Log("Not enough money");
                                    }
                                }
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("200g", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        if (first) first = false;
                    }
                    if (!pC.HasSkill(SkillName.Teleport))
                    {
                        if (!first) GUILayout.FlexibleSpace();
                        GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Teleport", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button(teleportIcon, GUILayout.MaxWidth(64), GUILayout.MaxHeight(64))) {
                                    if (pC.HasGold(300))
                                    {
                                        pC.AddSkill(SkillName.Teleport);
                                    }
                                    else
                                    {
                                        Debug.Log("Not enough money");
                                    }
                                }
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("300g", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        if (first) first = false;
                    }
                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Back", "GButton"))
                {
                    currentMenu = MenuType.MainMenu;
                }
                GUILayout.EndArea();
                break;
            case MenuType.Items:
                bool firstI = true;
                GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.4f));
                GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (!pC.HasItem(ItemName.Boots))
                    {
                        if (!firstI) GUILayout.FlexibleSpace();
                            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Boots", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button(bootsIcon, GUILayout.MaxWidth(64), GUILayout.MaxHeight(64))) {
                                    if (pC.HasGold(200))
                                    {
                                        //pC.AddSkill(SkillName.Homingball);
                                    }
                                    else {
                                        Debug.Log("Not enough money");
                                    }
                                }
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("200g", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        if (firstI) firstI = false;
                    }
                    if (!pC.HasItem(ItemName.Staff))
                    {
                        if (!firstI) GUILayout.FlexibleSpace();
                        GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Staff", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button(staffIcon, GUILayout.MaxWidth(64), GUILayout.MaxHeight(64))) {
                                    if (pC.HasGold(300))
                                    {
                                        //pC.AddSkill(SkillName.Teleport);
                                    }
                                    else
                                    {
                                        Debug.Log("Not enough money");
                                    }
                                }
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("300g", "GLabelSmall");
                                GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        if (firstI) firstI = false;
                    }
                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Back", "GButton"))
                {
                    currentMenu = MenuType.MainMenu;
                }
                GUILayout.EndArea();
                break;
            default:
                break;
        }

        if (GUI.Button(new Rect(Screen.width - 210, Screen.height - 80, 170, 40), "Finish Round", "GButton")) {
            Application.LoadLevel(1);
        }
    }
}

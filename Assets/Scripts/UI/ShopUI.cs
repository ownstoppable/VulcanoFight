using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShopUI : MonoBehaviour {

    private float shopTime;
    private float shopInitTime;
    public GUISkin mySkin;
    private PlayerController pC;
    public itemUI homingBallUI;
    public itemUI teleportUI;
    public itemUI staffUI;
    public itemUI bootsUI;
    public itemUI cloakUI;

    private Dictionary<SkillName, itemUI> skills;
    private Dictionary<ItemName, itemUI> items;

    private enum MenuType
    {
        MainMenu,
        Skills,
        NewSkills,
        LearnedSkills,
        Items
    }
    private MenuType currentMenu;

    [Serializable]
    public class itemUI {
        public string name;
        public Texture2D texture;
        public int price;
    }

    void Start() {
        shopTime = 90;
        shopInitTime = Time.time;
        currentMenu = MenuType.MainMenu;
        pC = GameOptions.Instance.playerObj.GetComponent<PlayerController>();
        skills = new Dictionary<SkillName, itemUI>();
        skills.Add(SkillName.Homingball, homingBallUI);
        skills.Add(SkillName.Teleport, teleportUI);

        items = new Dictionary<ItemName, itemUI>();
        items.Add(ItemName.Staff, staffUI);
        items.Add(ItemName.Boots, bootsUI);
        items.Add(ItemName.Cloak, cloakUI);
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
                    foreach (var item in Enum.GetValues(typeof(SkillName)))
                    {
                        if ((SkillName)item != SkillName.None && !pC.HasSkill((SkillName)item))
                        {
                            if (!first) GUILayout.FlexibleSpace();
                            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(skills[(SkillName)item].name, "GLabelSmall");
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button(skills[(SkillName)item].texture, GUILayout.MaxWidth(64), GUILayout.MaxHeight(64)))
                            {
                                if (pC.HasGold(skills[(SkillName)item].price))
                                {
                                    pC.AddSkill((SkillName)item);
                                }
                                else
                                {
                                    Debug.Log("Not enough money " + skills[(SkillName)item].price);
                                }
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(skills[(SkillName)item].price + "g", "GLabelSmall");
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                            if (first) first = false;
                        }
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
                    foreach (var item in Enum.GetValues(typeof(ItemName)))
                    {
                        if (!pC.HasItem((ItemName)item))
                        {
                            if (!firstI) GUILayout.FlexibleSpace();
                            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(items[(ItemName)item].name, "GLabelSmall");
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button(items[(ItemName)item].texture, GUILayout.MaxWidth(64), GUILayout.MaxHeight(64)))
                            {
                                if (pC.HasGold(items[(ItemName)item].price))
                                {
                                    pC.AddItem((ItemName)item, new Item((ItemName)item,items[(ItemName)item].texture));
                                }
                                else
                                {
                                    Debug.Log("Not enough money " + items[(ItemName)item].price);
                                }
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(items[(ItemName)item].price + "g", "GLabelSmall");
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                            if (firstI) firstI = false;
                        }
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

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
    public itemUI meteorBallUI;
    public itemUI etherealWalkUI;

    public itemUI staffUI;
    public itemUI bootsUI;
    public itemUI cloakUI;
    public itemUI vitballUI;
    public itemUI apofmagicUI;
    public itemUI stpendUI;
    public itemUI shcunUI;
    public itemUI evilapUI;
    public itemUI mplatemUI;

    private Dictionary<SkillName, itemUI> skills;
    private Dictionary<ItemName, itemUI> items;

    private Vector2 itemScroll = Vector2.zero;
    private int selectedItem = -1;

    private Vector2 skillScroll = Vector2.zero;
    private int selectedSkill = -1;

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
        public string details;
    }

    void Start() {
        shopTime = 90;
        shopInitTime = Time.time;
        currentMenu = MenuType.MainMenu;
        pC = GameOptions.Instance.playerObj.GetComponent<PlayerController>();
        skills = new Dictionary<SkillName, itemUI>();
        skills.Add(SkillName.Homingball, homingBallUI);
        skills.Add(SkillName.Teleport, teleportUI);
        skills.Add(SkillName.MeteorBlast, meteorBallUI);
        skills.Add(SkillName.EtherealWalk, etherealWalkUI);

        items = new Dictionary<ItemName, itemUI>();
        items.Add(ItemName.CarversStaff, staffUI);
        items.Add(ItemName.HauntersBoots, bootsUI);
        items.Add(ItemName.DivineCloak, cloakUI);
        items.Add(ItemName.VitalityBall, vitballUI);
        items.Add(ItemName.ApparatusofMagic, apofmagicUI);
        items.Add(ItemName.SturdyPendant, stpendUI);
        items.Add(ItemName.ShieldofCunning, shcunUI);
        items.Add(ItemName.EvilStrongApparatus, evilapUI);
        items.Add(ItemName.MeteoricPlateMail, mplatemUI);
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
                #region old_ui
                /*bool first = true;
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
                GUILayout.EndArea();*/
                #endregion old_ui
                GUI.BeginGroup(new Rect(Screen.width * 0.2f, Screen.height * 0.3f, Screen.width * 0.6f, Screen.height * 0.4f));
                GUI.Box(new Rect(0, 0, Screen.width * 0.4f, Screen.height * 0.4f), "");
                int maxSkillIcon = (int)Math.Floor((Screen.width * 0.4f - 30) / 74);
                skillScroll = GUI.BeginScrollView(new Rect(0, 0, Screen.width * 0.4f, Screen.height * 0.4f), skillScroll, new Rect(0, 0, Screen.width * 0.4f - 20, 20 + Mathf.Ceil(((Enum.GetValues(typeof(SkillName)).Length - 1) - pC.SkillCount()) / (float)maxSkillIcon) * 74));
                int sklIndex = 0;
                foreach (var item in Enum.GetValues(typeof(SkillName)))
                {
                    if ((SkillName)item != SkillName.None && !pC.HasSkill((SkillName)item))
                    {
                        if (GUI.Button(new Rect(10 + (sklIndex - Mathf.Ceil(sklIndex / maxSkillIcon) * maxSkillIcon) * 74, 10 + Mathf.Ceil(sklIndex / maxSkillIcon) * 74, 64, 64), skills[(SkillName)item].texture, "GButton"))
                        {
                            selectedSkill = (int)item;
                        }
                        sklIndex++;
                    }
                    
                }
                GUI.EndScrollView();
                GUI.Box(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.2f, Screen.height * 0.4f), "");
                    GUI.BeginGroup(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.2f, Screen.height * 0.4f));
                    if (selectedSkill != -1)
                    {
                        GUI.Label(new Rect(10, 10, 100, 30), skills[(SkillName)selectedSkill].name, "GLabelSmall");
                        GUI.TextArea(new Rect(10, 50, Screen.width * 0.2f - 20, Screen.height * 0.25f), skills[(SkillName)selectedSkill].details, "GTextArea");
                        GUI.Label(new Rect(10, Screen.height * 0.4f - 40, 100, 30), skills[(SkillName)selectedSkill].price + "g", "GLabelSmall");
                        if (GUI.Button(new Rect(Screen.width * 0.2f - 50, Screen.height * 0.4f - 40, 50, 30), "Buy", "GLabelSmall")) {
                            if (pC.HasGold(skills[(SkillName)selectedSkill].price))
                            {
                                pC.AddSkill((SkillName)selectedSkill);
                                selectedSkill = -1;
                            }
                            else
                            {
                                Debug.Log("Not enough money " + skills[(SkillName)selectedSkill].price);
                            }
                        }
                    }
                    GUI.EndGroup();
                GUI.EndGroup();
                if (GUI.Button(new Rect(Screen.width * 0.5f - 40, Screen.height * 0.7f  + 20, 80, 30), "Back", "GButton"))
                {
                    currentMenu = MenuType.MainMenu;
                }
                break;
            case MenuType.Items:
                GUI.BeginGroup(new Rect(Screen.width * 0.2f, Screen.height * 0.3f, Screen.width * 0.6f, Screen.height * 0.4f));
                GUI.Box(new Rect(0, 0, Screen.width * 0.4f, Screen.height * 0.4f), "");
                int maxIcon = (int)Math.Floor((Screen.width * 0.4f - 30) / 74);
                itemScroll = GUI.BeginScrollView(new Rect(0, 0, Screen.width * 0.4f, Screen.height * 0.4f), itemScroll, new Rect(0, 0, Screen.width * 0.4f - 20, 20 + Mathf.Ceil((Enum.GetValues(typeof(ItemName)).Length - pC.GetInventory.Count) / (float)maxIcon) * 74));
                int itIndex = 0;
                foreach (var item in Enum.GetValues(typeof(ItemName)))
                {
                    if (!pC.HasItem((ItemName)item))
                    {
                        if (GUI.Button(new Rect(10 + (itIndex - Mathf.Ceil(itIndex / maxIcon) * maxIcon) * 74, 10 + Mathf.Ceil(itIndex / maxIcon) * 74, 64, 64), items[(ItemName)item].texture, "GButton"))
                        {
                            selectedItem = (int)item;
                        }
                        itIndex++;
                    }
                    
                }
                GUI.EndScrollView();
                GUI.Box(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.2f, Screen.height * 0.4f), "");
                    GUI.BeginGroup(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.2f, Screen.height * 0.4f));
                    if (selectedItem != -1) {
                        GUI.Label(new Rect(10, 10, 100, 30), items[(ItemName)selectedItem].name, "GLabelSmall");
                        GUI.TextArea(new Rect(10, 50, Screen.width * 0.2f - 20, Screen.height * 0.25f), items[(ItemName)selectedItem].details, "GTextArea");
                        GUI.Label(new Rect(10, Screen.height * 0.4f - 40, 100, 30), items[(ItemName)selectedItem].price + "g", "GLabelSmall");
                        if (GUI.Button(new Rect(Screen.width * 0.2f - 50, Screen.height * 0.4f - 40, 50, 30), "Buy", "GLabelSmall")) {
                            if (pC.HasGold(items[(ItemName)selectedItem].price))
                            {
                                pC.AddItem((ItemName)selectedItem, new Item((ItemName)selectedItem, items[(ItemName)selectedItem].texture, items[(ItemName)selectedItem].name));
                                selectedItem = -1;
                            }
                            else
                            {
                                Debug.Log("Not enough money " + items[(ItemName)selectedItem].price);
                            }
                        }
                    }
                    GUI.EndGroup();
                GUI.EndGroup();
                if (GUI.Button(new Rect(Screen.width * 0.5f - 40, Screen.height * 0.7f  + 20, 80, 30), "Back", "GButton"))
                {
                    currentMenu = MenuType.MainMenu;
                }
                break;
            default:
                break;
        }

        if (GUI.Button(new Rect(Screen.width - 210, Screen.height - 80, 170, 40), "Finish Round", "GButton")) {
            Application.LoadLevel(1);
        }
    }
}

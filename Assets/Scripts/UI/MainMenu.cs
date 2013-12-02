using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public GUISkin mySkin;

    private enum MenuType { 
        MainMenu,
        StartGame,
        Options
    }
    private MenuType currentMenu;

    private int nRounds;
    private int nEnemies;
    private int nMins;

	// Use this for initialization
	void Start () {
        currentMenu = MenuType.MainMenu;
        nRounds = 3;
        nEnemies = 5;
        nMins = 5;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        GUI.skin = mySkin;

        switch (currentMenu)
        {
            case MenuType.MainMenu:
                if (GUI.Button(new Rect(Screen.width * 0.5f - 50, Screen.height * 0.5f, 100, 30), "New Game", "MMButton")) {
                    currentMenu = MenuType.StartGame;
                }

                if (GUI.Button(new Rect(Screen.width * 0.5f - 50, Screen.height * 0.5f + 60, 100, 30), "Options", "MMButton"))
                {
                    Debug.Log("Options clicked");
                }

                if (GUI.Button(new Rect(Screen.width * 0.5f - 50, Screen.height * 0.5f + 120, 100, 30), "Exit", "MMButton"))
                {
                    Application.Quit();
                }
                break;
            case MenuType.StartGame:
                GUILayout.BeginArea(new Rect(Screen.width*0.25f, Screen.height * 0.4f, Screen.width*0.5f, Screen.height * 0.25f));
                    GUILayout.BeginHorizontal();
                        GUILayout.Label("Number of Rounds ", "GLabel");
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("<", "GButton", GUILayout.MaxWidth(20))) {
                            if (--nRounds < 1) {
                                nRounds = 1;
                            }
                        }
                        GUILayout.Space(50);
                        GUILayout.Label(nRounds.ToString(), "GLabel", GUILayout.Width(50));
                        GUILayout.Space(50);
                        if (GUILayout.Button(">", "GButton", GUILayout.MaxWidth(20))) {
                            if (++nRounds > 20)
                            {
                                nRounds = 20;
                            }
                        }
                        GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Number of Enemies ", "GLabel");
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("<", "GButton", GUILayout.MaxWidth(20))) {
                            if (--nEnemies < 0) {
                                nEnemies = 0;
                            }
                        }
                        GUILayout.Space(50);
                        GUILayout.Label(nEnemies.ToString(), "GLabel", GUILayout.Width(50));
                        GUILayout.Space(50);
                        if (GUILayout.Button(">", "GButton", GUILayout.MaxWidth(20))) {
                            if (++nEnemies > 10)
                            {
                                nEnemies = 10;
                            }
                        }
                        GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Time Limit   (Mins)", "GLabel");
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("<", "GButton", GUILayout.MaxWidth(20))) {
                            if (--nMins < 3) {
                                nMins = 3;
                            }
                        }
                        GUILayout.Space(50);
                        GUILayout.Label(nMins.ToString(), "GLabel", GUILayout.Width(50));
                        GUILayout.Space(50);
                        if (GUILayout.Button(">", "GButton", GUILayout.MaxWidth(20))) {
                            if (++nMins > 30)
                            {
                                nMins = 30;
                            }
                        }
                        GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(30);
                    GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Back", "GButton")) {
                            currentMenu = MenuType.MainMenu;
                        }
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Start Game", "GButton")) {
                            GameOptions.Instance.GameOptionsInit(nRounds, nEnemies, nMins);
                            Application.LoadLevel(1);
                        }
                        GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                GUILayout.EndArea();
                break;
            case MenuType.Options:
                break;
            default:
                break;
        }


    }
}

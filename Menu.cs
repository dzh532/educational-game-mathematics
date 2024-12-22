// 136     время решения примера

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static Cinemachine.DocumentationSortingAttribute;

/*
1. Сохранение: денег, мячей, уровня, положение круга уровня 
 */

public class Menu : MonoBehaviour
{
    string[] primeri, otveti;
    int pr = 0;

    // 1   -   только сложение   18
    int pr_1 = 30;
    string[] primeri_1 = { "1 + 1", "1 + 4", "2 + 2", "2 + 9", "3 + 4", "3 + 8", "4 + 7", "4 + 8", "5 + 9", "5 + 2", "6 + 7", "6 + 3", "7 + 1", "7 + 4", "8 + 2", "8 + 7", "9 + 5", "9 + 9" };
    string[] otveti_1 = {    "2",     "5",     "4",     "11",    "7",     "11",    "11",    "12",    "14",    "7",     "13",    "9",     "8",     "11",    "10",    "15",   "14",     "18" };

    // 2   -   сложение и вычитание (без отрицательных)   16 и 16
    int pr_2 = 50;
    string[] primeri_2 = { "9 + 12", "10 + 17", "6 + 18", "15 + 4", "9 + 20", "13 + 8", "17 + 4", "10 + 15", "13 + 12", "18 + 6", "17 + 8", "9 + 19", "8 + 16", "7 + 21", "4 + 11", "8 + 10",
    "8 - 7", "13 - 2", "20 - 10", "9 - 6", "10 - 8", "12 - 9", "14 - 6", "7 - 5", "19 - 6", "20 - 8", "13 - 9", "14 - 7", "2 - 1", "4 - 2", "9 - 4", "15 - 10"};
    string[] otveti_2 = {   "21",       "27",     "24",      "19",    "29",     "21",     "21",      "25",     "25",       "24",     "25",     "28",     "24",    "28",     "15",     "18",
      "1",      "11",     "10",     "3",      "2",      "3",      "8",      "2",     "13",      "12",     "4",     "7",     "1",     "2",      "5",     "5"};

    // 3   -   сложение и вычитание (с отрицательными)   15 и 15
    int pr_3 = 60;
    string[] primeri_3 = { "22 + 8", "38 + 44", "20 + 15", "33 + 67", "28 + 14", "22 + 63", "45 + 54", "55 + 28", "21 + 47", "95 + 3", "15 + 72", "28 + 35", "66 + 14", "73 + 26", "24 + 59",
    "99 - 9", "60 - 10", "44 - 12", "28 - 15", "98 - 78", "99 - 15", "77 - 25", "34 - 32", "58 - 60", "4 - 20", "20 - 50", "48 - 53", "20 - 90", "77 - 85", "23 - 84"};
    string[] otveti_3 = {    "30",     "82",       "35",    "100",      "42",       "85",     "99",      "83",       "68",      "98",    "87",       "63",      "80",      "99",     "83",
      "90",    "50",      "32",       "13",      "20",       "84",      "52",     "2",       "-2",       "-16",    "-30",     "-5",      "-70",     "-8",      "-61" };

    int r;
    public int ball, money, level;
    float level_filled, xp;

    public Text txt_ball, txt_money, txt_level;

    public GameObject _menu, _menu_getBall, _menu_naPole, _menu_naPole_nullBall;

    public Game game;

    // Панель пример
    public GameObject pnl_primer;
    public Text txt_primer;
    public InputField infld_otvet;
    public Button btn_sendOtvet, btn_nextPrimer, btn_finishPrimer;

    public GameObject[] cam_list_;

    bool isPrimer;

    // Уведомление
    public Text[] yved_list_;

    public Text txt_naPole;

    bool isBackInMenu, is_btnPrimer;

    public Image levelKrug;

    // Таймер
    public Text txt_time;
    float timeStart = 5f;
    bool isTime = false;
    public Image img_timer;

    GameObject objEsc;

    void Start()
    {
        game.GetComponent<Game>();

        for (int i = 0; i < cam_list_.Length; i++) cam_list_[i].SetActive(false);
        cam_list_[0].SetActive(true);

        btn_nextPrimer.interactable = false;
        btn_finishPrimer.interactable = false;

        isPrimer = false;
        isBackInMenu = false;
        is_btnPrimer = false;
        isTime = false;

        level = 1;
        level_filled = 0;
        timeStart = 5f;
        xp = 0;

        txt_time.text = timeStart.ToString("F2");

        gameObject.GetComponent<Image>().enabled = false;

        StartCoroutine(Cor_Menu());
        Load();
    }
    void Load()
    {
        if (PlayerPrefs.HasKey("ball")) ball = PlayerPrefs.GetInt("ball");
        if (PlayerPrefs.HasKey("money")) money = PlayerPrefs.GetInt("money");
        if (PlayerPrefs.HasKey("level")) level = PlayerPrefs.GetInt("level");
        if (PlayerPrefs.HasKey("level_filled")) level_filled = PlayerPrefs.GetFloat("level_filled");
        if (PlayerPrefs.HasKey("xp")) xp = PlayerPrefs.GetFloat("xp");
    }
    IEnumerator Cor_Menu() 
    {
        yield return new WaitForSeconds(1.5f); 
        _menu.SetActive(true);
        gameObject.GetComponent<Image>().enabled = true; 
    }
    void Update()
    {
        txt_ball.text = ball.ToString();
        txt_money.text = money.ToString();
        txt_level.text = level.ToString() + " <i><color=grey>[ " + xp.ToString("F0") + " / 100 ]</color></i>";

        if (isTime) CheckTime();

        // levelKrug.fillAmount = level_filled;

        CheckLevel();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            if (isPrimer)
                Send_Otvet();

        
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            // Esc();
        }
    }
    /*
    public void Esc()
    {
        if (gameObject.GetComponent<Image>().enabled)
        {
            gameObject.GetComponent<Image>().enabled = false;

            if (_menu.activeInHierarchy) objEsc = _menu;
            else if (_menu_getBall.activeInHierarchy) objEsc = _menu_getBall;
            else if (_menu_naPole.activeInHierarchy) objEsc = _menu_naPole.transform.GetChild(0).GetComponent<Image>().gameObject;

            objEsc.SetActive(false);
        }
        else
        {
            objEsc.SetActive(true);
            gameObject.GetComponent<Image>().enabled = true;
        }
    }*/

    void CheckTime()
    {
        txt_time.text = timeStart.ToString("F2");
        timeStart -= Time.deltaTime;

        img_timer.fillAmount = timeStart * 0.2f;

        if (timeStart <= 0)
        {
            if (isPrimer) // время на решение примера вышло
            {
                txt_primer.text = "Время вышло...";
                level_filled -= 1.0f / 2 / pr;

                btn_nextPrimer.interactable = true;
                btn_finishPrimer.interactable = true;
                btn_sendOtvet.interactable = false;
                infld_otvet.interactable = false;

                isPrimer = isTime = false;

                infld_otvet.text = "";
            }
            timeStart = 0;
            isTime = false;
        }
    }
    void CheckLevel()
    {

        xp = level_filled * 100f;

        if (level_filled >= 1)
        {
            xp = 0f;
            level_filled = 0;
            level++;
        }
        if (level_filled < 0)
        {
            level_filled = 0;
            if (level > 1) level--;
        }
    }

    IEnumerator Cor_Yved(int s) { yield return new WaitForSeconds(3); yved_list_[s].text = ""; }

    void Create_Yved(string t)
    {
        int s = 0;

        for (int i = 0; i < yved_list_.Length; i++)
        {
            if (yved_list_[i].text == "")
            {
                yved_list_[i].text = t;
                s = i;
                i = yved_list_.Length;
            }
        }
        StartCoroutine(Cor_Yved(s));
    }

    void enabledCam(int k)
    {
        for (int i = 0; i < cam_list_.Length; i++)
        {
            if (i == k) cam_list_[i].SetActive(true);
            else cam_list_[i].SetActive(false);
        }
    }

    IEnumerator Cor_BackInMenu()
    {
        yield return new WaitForSeconds(2);
        isBackInMenu = true;
        // Esc();
        BackInMenu();
    }
    public void BackInMenu()
    {
        if (!isPrimer)
        {
            is_btnPrimer = false;

            if (!isBackInMenu)
            {
                _menu_naPole.SetActive(false);
                _menu_naPole_nullBall.SetActive(false);
                _menu_getBall.SetActive(false);
                pnl_primer.SetActive(false);

                enabledCam(0);

                StartCoroutine(Cor_BackInMenu());
            }
            else
            {
                _menu.SetActive(true);

                isBackInMenu = false;
            }
        }
        else
        {
            Create_Yved("Заверши решение текущего примера");
        }
    }

    IEnumerator Cor_NaPole()
    {
        yield return new WaitForSeconds(2);
        Create_Yved("Выберите зону для удара по воротам");
        _menu_naPole.SetActive(true);
    }
    public void Na_Pole()
    {
        if (ball > 0)
        {
            game.ResetBall();

            _menu.SetActive(false);
            enabledCam(3);

            StartCoroutine(Cor_NaPole());
        }
        else
        {
            Create_Yved("У тебя нету мячей");
        }
    }

    public void OK_NaPole()
    {
        _menu_naPole_nullBall.SetActive(false);
        _menu_naPole.SetActive(false);
        objEsc = _menu;

        BackInMenu();

    }

    IEnumerator Cor_GetBall()
    {
        yield return new WaitForSeconds(2);

        _menu_getBall.SetActive(true);
    }
    public void Get_Ball()
    {
        enabledCam(1);
        _menu.SetActive(false);
        StartCoroutine(Cor_GetBall());
    }

    public void Next_Primer()
    {
        Generate_Primer();

        btn_sendOtvet.interactable = true;

    }
    public void Finish_Primer()
    {
        btn_nextPrimer.interactable = false;
        btn_finishPrimer.interactable = false;
        btn_sendOtvet.interactable = false;

        txt_primer.text = "Ждем твоих действий";

        _menu_getBall.SetActive(true);
    }

    public void Send_Otvet()
    {
        if (infld_otvet.text == otveti[r])
        {
            txt_primer.text = "Верно!\n+ <color=purple>1</color> мяч";
            ball++;
            level_filled += 1.0f / pr;

        }
        else
        {
            txt_primer.text = "Не верно!\nПопробуйте еще раз";
            level_filled -= 1.0f / 2 / pr;
        }

        btn_nextPrimer.interactable = true;
        btn_finishPrimer.interactable = true;
        btn_sendOtvet.interactable = false;
        infld_otvet.interactable = false;

        isPrimer = isTime = false;

        infld_otvet.text = "";
    }
    void Generate_Primer()
    {
        timeStart = 5f;
        isPrimer = true;
        isTime = true;

        switch (level)
        {
            case 1:
                primeri = primeri_1;
                otveti = otveti_1;
                pr = pr_1;
                break;

            case 2:
                primeri = primeri_2;
                otveti = otveti_2;
                pr = pr_2;
                break;

            case 3:
                primeri = primeri_3;
                otveti = otveti_3;
                pr = pr_3;
                break;

            default:
                break;
        }
        r = Random.Range(0, primeri.Length);
        txt_primer.text = primeri[r];
        btn_nextPrimer.interactable = false;
        btn_finishPrimer.interactable = false;
        btn_sendOtvet.interactable = true;
        infld_otvet.interactable = true;
    }

    IEnumerator Cor_Primer()
    {
        yield return new WaitForSeconds(2);

        pnl_primer.SetActive(true);

        Generate_Primer();
    }
    public void Primeri()
    {
        if (!is_btnPrimer)
        {
            is_btnPrimer = true;
            _menu_getBall.SetActive(false);
            enabledCam(2);
            StartCoroutine(Cor_Primer());
        }
    }

    public void Yravnenia()
    {
        if (isPrimer) Create_Yved("Заверши решение текущего примера");
        else
        {

        }
    }
    public void Sravnenia()
    {
        if (isPrimer) Create_Yved("Заверши решение текущего примера");
        else
        {

        }
    }

    public void Stats()
    {
        Debug.Log("Stats");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Magazin()
    {

    }

    private void OnApplicationQuit()
    {
        // ball, money, level, level_filled
        PlayerPrefs.SetInt("ball", ball);
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetFloat("level_filled", level_filled);
        PlayerPrefs.SetFloat("xp", xp);

        PlayerPrefs.Save();
    }
}

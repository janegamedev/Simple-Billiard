using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject balls; //prefab
    public GameObject startPositionBalls; //start posiition for balls
    public Rigidbody rbBall;
    public ScoringSystem scoringSystem;

    //Props 
    public Ball ball;
    public GameObject houseColor;
    public Camera houseTableCam;
    public Camera playerCam;

    //Players and game properties
    public List<PlayerClass> players;
    public int timeLeft = 30;
    public bool isPause;
    public bool inMenu;
    public bool isHit;
    public int turn; //player=0, player2 =1
    public bool isHitting;

    //MainLevelUI
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public GameObject gameLevelUI;

    //ChangeTurnMenu
    public GameObject ChangeTurnPanel;
    public TextMeshProUGUI TurnTextOnPanel;

    //MainMenu
    public GameObject exitMenu;
    public GameObject mainMenu;
    private bool mainMenuOpen = false;

    //WinMenu
    public GameObject winMenu;
    public TextMeshProUGUI winText;

    //StartMenu
    public GameObject startMenu;
    public InputField player1;
    public InputField player2;

    //Instractions
    public GameObject rotationIns;
    public GameObject hitIns;
    public GameObject houseIns;
    public bool hasReset;

    //which ball in pocket
    public TextMeshProUGUI updateBall;
    public GameObject updateBallObject;

    private void Awake()
    {
        scoringSystem = GetComponent<ScoringSystem>();

        players = new List<PlayerClass>();
        AddPlayers();
    }

    private void Start()
    {
        inMenu = true;
        isPause = true;
        ResetBalls();
        gameLevelUI.SetActive(false);
        CallStartMenu();
    }

    private void Update()
    {
        if (isHit || ball.isInHouse)
        {
            SetActiveHouseTableCam();
        }
        else
        {
            SetInactiveHouseTableCam();
        }

        //if (isHit && rbBall.velocity == Vector3.zero && ball.timePast >= 2)
        //{
        //    isHitFinished = true;
        //}

        if (Input.GetKeyDown("escape") && !mainMenuOpen)
            CallMainMenu();
        else if (Input.GetKeyDown("escape") && mainMenuOpen)
            CloseMainMenu();

        ShowCurrentPlayer(turnText);
        timerText.text = (" " + timeLeft);
    }

    //RESET BALLS
    void ResetBalls()
    {
        GameObject go = Instantiate(balls);
        go.transform.position = startPositionBalls.transform.position;
    }

    //Add player to the list
    void AddPlayers()
    {
        players.Add(new PlayerClass());
        players.Add(new PlayerClass());
    }

    //SHowing player score UI
    public void UpdateScore()
    {
        player1Score.text = players[0].playerName + " : " + players[0].playerScore;
        player2Score.text = players[1].playerName + " : " + players[1].playerScore;
    }

    //End turn
    public void EndOfTheTurn()
    {
        ball.isFirst = false;
        isHit = false;
        scoringSystem.CheckScore();
        CancelInvoke("CountDownTimer");
        timeLeft = 30;
        ShowChangeTurnPanel();
    }

    //change player turn
    public void ChangeTurn()
    {
        if (turn == 0)
            turn = 1;
        else
            turn = 0;
    }

    public void FinishGame(int player)
    {
        CancelInvoke("CountDownTimer");
        isPause = true;
        inMenu = true;
        gameLevelUI.SetActive(false);
        winMenu.SetActive(true);
        winText.text = "Player " + players[player].playerName + " won!";
    }

    //House Table, Color
    public void HouseTable()
    {
        inMenu = !inMenu;
        if (houseColor.activeSelf == true)
            houseColor.SetActive(false);
        else
            houseColor.SetActive(true);
    }

    //Change camera
    public void SetActiveHouseTableCam()
    {
        houseTableCam.enabled = true;

        if (!hasReset && ball.isInHouse)
            CallInstractionMenu(houseIns);
    }

    public void SetInactiveHouseTableCam()
    {
        houseTableCam.enabled = false;
    }


    //TIMER
    void CountDownTimer()
    {
        if (!inMenu || !isPause)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                if (timeLeft <= 10)
                    timerText.color = new Color32(255, 0, 0, 255);
            }

            if (timeLeft <= 0)
            {
                ball.StopBall();
                EndOfTheTurn();
                SetInactiveHouseTableCam();
                CancelInvoke("CountDownTimer");
            }
        }

    }

    //Showing current player
    void ShowCurrentPlayer(TextMeshProUGUI _text)
    {
        _text.text = players[turn].playerName + " turn";
    }

    // Turn panel
    void ShowChangeTurnPanel()
    {
        ChangeTurnPanel.SetActive(true);
        ShowCurrentPlayer(TurnTextOnPanel);
        Invoke("CloseChangeTurnPanel", 1);
    }

    void CloseChangeTurnPanel()
    {
        isHit = false;
        isHitting = false;
        isPause = false;
        ChangeTurnPanel.SetActive(false);
        InvokeRepeating("CountDownTimer", 1, 1);
    }

    //MENU
    public void CallStartMenu()
    {
        startMenu.SetActive(true);
        isPause = true;
        inMenu = true;
    }

    public void CloseStartMenu()
    {
        if (player1.text != null || player2.text != null)
        {
            players[0].playerName = player1.text;
            players[1].playerName = player2.text;
            UpdateScore();
            startMenu.SetActive(false);
            gameLevelUI.SetActive(true);
            CallInstractionMenu(rotationIns);
        }
    }

    void CallInstractionMenu(GameObject inst)
    {
        inst.gameObject.SetActive(true);
        inMenu = true;
        isPause = true;
    }

    public void CloseInstractionMenu(GameObject inst)
    {
        if (inst.gameObject.CompareTag("Rotation"))
        {
            hitIns.SetActive(true);
        }
        else if (inst.gameObject.CompareTag("Hit"))
        {
            isPause = false;
            inMenu = false;
            InvokeRepeating("CountDownTimer", 1, 1);
        }
        if (inst.gameObject.CompareTag("House"))
        {
            hasReset = true;
            isPause = false;
        }

        inst.SetActive(false);
    }

    public void ShowWhichBall (string tag)
    {
        updateBallObject.SetActive(true);
        updateBall.text = tag + " ball in pocket";
        Invoke("CloseShowWhichBall", 1);
    }

    void CloseShowWhichBall()
    {
        updateBallObject.SetActive(false);
    }

    public void CallMainMenu()
    {
        mainMenu.SetActive(true);
        mainMenuOpen = true;
        isPause = true;
        inMenu = true;
    }

    public void CloseMainMenu()
    {
        mainMenu.SetActive(false);
        mainMenuOpen = false;
        isPause = false;
        inMenu = false;
    }

    public void CallExitMenu()
    {
        exitMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseQuitMenu()
    {
        mainMenu.SetActive(true);
        exitMenu.SetActive(false);
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

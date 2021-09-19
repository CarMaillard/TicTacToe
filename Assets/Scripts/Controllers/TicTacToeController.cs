using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class TicTacToeController : MonoBehaviour
{

    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    public Dropdown difficulty;
    public Text PlayerVictories;
    public Text CpuVictories;
    private int playerVictories = 0;
    private int cpuVictories = 0;

    private string playerSide;
    private int moveCount;

    private TicTacToeModel minMax = new TicTacToeModel();

    void Awake()
    {
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartButton.SetActive(false);
        PopulateDifficulty();
    }

    void PopulateDifficulty()
    {
        List<string> difficultyList = new List<string>() { "Easy", "Normal" };
        difficulty.AddOptions(difficultyList);
    }

    public void SetDifficulty(int index)
    {
        Debug.Log(index);
        if (index == 0)
            minMax.isMaximizing = true;
        else if (index == 1)
            minMax.isMaximizing = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpaceController>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
            minMax.human = "X";
            minMax.ai = "O";
        }
        else
        {
            SetPlayerColors(playerO, playerX);
            minMax.human = "O";
            minMax.ai = "X";
        }

        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("draw");
        }
        else
        {
            ChangeSides();
        }
    }

    private string[,] CreateBoard(Text[] buttonList)
    {
        string[,] board = new string[3, 3] { { "", "", "" }, { "", "", "" }, { "", "", "" } };
        board[0, 0] = buttonList[0].text;
        board[0, 1] = buttonList[1].text;
        board[0, 2] = buttonList[2].text;
        board[1, 0] = buttonList[3].text;
        board[1, 1] = buttonList[4].text;
        board[1, 2] = buttonList[5].text;
        board[2, 0] = buttonList[6].text;
        board[2, 1] = buttonList[7].text;
        board[2, 2] = buttonList[8].text;
        return board;
    }

    void ChangeSides()
    {
        /////////////////// MinMax //////////////////////
        if (playerSide == minMax.human)
        {
            minMax.board = CreateBoard(buttonList);
            int[] move = minMax.bestMove();
            Debug.Log("Board: " + minMax.board);
            Debug.Log("IA move: " + move[0] + "," + move[1]);
            buttonList[move[0]*3 + move[1]].text = minMax.ai;
            buttonList[move[0] * 3 + move[1]].transform.parent.GetComponent<Button>().interactable = false;
            playerSide = (playerSide == "X") ? "O" : "X";
            EndTurn();
            return;
        }
        /////////////////////////////////////////////////

        playerSide = (playerSide == "X") ? "O" : "X";
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);
        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText(winningPlayer + " Wins!");
        }
        restartButton.SetActive(true);
        ScoreBoard(winningPlayer);
    }

    private void ScoreBoard(string winningPlayer) { 
        if (winningPlayer == minMax.human)
        {
            playerVictories++;
            PlayerVictories.text = "Player: " + playerVictories;
        }
        else if ((winningPlayer == minMax.ai))
        {
            cpuVictories++;
            CpuVictories.text = "Cpu: " + cpuVictories;
        }
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

}
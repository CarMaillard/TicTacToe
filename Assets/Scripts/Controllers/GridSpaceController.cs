using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpaceController : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private TicTacToeController gameController;

    public void SetGameControllerReference(TicTacToeController controller)
    {
        gameController = controller;
    }

    public void SetSpace()
    {
        buttonText.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController.EndTurn();
    }
}

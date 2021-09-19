using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeModel : MonoBehaviour
{
    public string[,] board;
    public string human;
    public string ai;
    public bool isMaximizing = true;

    int scores(string cas) {
        switch (cas) 
        {
            case "X": 
                return (ai == "X") ? 1 : -1;
            case "O": 
                return (ai == "X") ? -1 : 1;
            default: //case "tie":
                return 0; 
        } 
    }

    public bool equals3(string a, string b, string c)
    {
        return (a == b) && (b == c) && (a != "");
    }

    string checkWinner()
    {
        string winner = null;

        // horizontal
        for (int  i = 0; i < 3; i++)
        {
            if (equals3(board[i,0], board[i,1], board[i,2]))
            {
                winner = board[i,0];
            }
        }

        // Vertical
        for (int i = 0; i < 3; i++)
        {
            if (equals3(board[0,i], board[1,i], board[2,i]))
            {
                winner = board[0,i];
            }
        }

        // Diagonal
        if (equals3(board[0,0], board[1,1], board[2,2]))
        {
            winner = board[0,0];
        }
        if (equals3(board[2,0], board[1,1], board[0,2]))
        {
            winner = board[2,0];
        }

        int openSpots = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i,j] == "")
                {
                    openSpots++;
                }
            }
        }

        if (winner == null && openSpots == 0)
        {
            return "tie";
        }
        else
        {
            return winner;
        }
    }

    int minimax(int depth, bool isMaximizing_)
    {
        string result = checkWinner();
        if (result != null)
        {
            return scores(result);
        }

        if (isMaximizing_)
        {
            int bestScore = -1000000;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Is the spot available?
                    if (board[i,j] == "")
                    {
                        board[i,j] = ai;
                        int score = minimax(depth + 1, false);
                        board[i,j] = "";
                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = 1000000;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Is the spot available?
                    if (board[i,j] == "")
                    {
                        board[i,j] = human;
                        int score = minimax(depth + 1, true);
                        board[i,j] = "";
                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    public int[] bestMove()
    {
        // AI to make its turn
        int bestScore = -1000000;
        int[] move = new int[2];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Is the spot available?
                if (board[i,j] == "")
                {
                    board[i,j] = ai;
                    int score = minimax(0, isMaximizing);
                    board[i,j] = "";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        move = new int[2] { i, j };
                    }
                }
            }
        }
        return move;
    }
}

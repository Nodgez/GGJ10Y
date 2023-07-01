using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance { get { return instance; } }
    private static UserInterface instance;

    public CanvasGroup menu;
    public CanvasGroup gameOver;

    private void Awake()
    {
        instance = this;
    }

    public TMPro.TextMeshProUGUI remainingCowCount;

    public void UpdateCowCount(int amount)
    {
        if (amount <= 0)
            ShowGameOver();
        remainingCowCount.text = amount.ToString();
    }

    public void ShowGameOver()
    {
        gameOver.alpha = 1;
        gameOver.interactable = true;
        gameOver.blocksRaycasts = true;
    }

    public void ShowMenu()
    {
        menu.alpha = 1;
        menu.interactable = true;
        menu.blocksRaycasts = true;
    }

    public void HideMenu()
    {
        menu.alpha = 0;
        menu.interactable = false;
        menu.blocksRaycasts = false;
    }
}

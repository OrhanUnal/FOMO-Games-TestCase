using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveCountText;
    [SerializeField] private TextMeshProUGUI currentLevelNumberText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    public static event Action OnRetryClicked;
    public static event Action OnNextLevelClicked;

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.UpdateCounters += UpdateCounters;
        GameManager.OnLevelFinished += ShowResult;
    }

    private void OnDisable()
    {
        GameManager.UpdateCounters -= UpdateCounters;
        GameManager.OnLevelFinished -= ShowResult;
    }

    private void UpdateCounters(int moveCounter, int currentLevel)
    {
        moveCountText.text = $"Moves: {moveCounter}";
        currentLevelNumberText.text = $"Current Level: {currentLevel}";
    }

    private void ShowResult(bool won)
    {
        if (won) winPanel.SetActive(true);
        else losePanel.SetActive(true);
    }

    public void RetryButton()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        OnRetryClicked?.Invoke();
    }

    public void NextLevelButton()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        OnNextLevelClicked?.Invoke();
    }
}
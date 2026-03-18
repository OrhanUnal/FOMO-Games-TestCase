using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveCountText;
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
        GameManager.OnMoveCountChanged += UpdateMoveCount;
        GameManager.OnLevelFinished += ShowResult;
    }

    private void OnDisable()
    {
        GameManager.OnMoveCountChanged -= UpdateMoveCount;
        GameManager.OnLevelFinished -= ShowResult;
    }

    private void UpdateMoveCount(int remaining)
    {
        moveCountText.text = $"Moves: {remaining}";
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
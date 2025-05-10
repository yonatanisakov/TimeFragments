using EventBusScripts;
using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;

    private void Awake()
    {
        EventBus.Get<LevelFailEvent>().Subscribe(ShowGameOverText);
    }

    private void ShowGameOverText()
    {
        gameOverText.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        EventBus.Get<LevelFailEvent>().Unsubscribe(ShowGameOverText);
    }
}

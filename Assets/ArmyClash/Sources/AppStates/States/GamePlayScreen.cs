using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreen: StateScreen {
    [SerializeField] private TMP_Text _scoreField;
    [SerializeField] private Button _exitButton;

    public void OnExitButtonPressed(Action onClick) {
        _exitButton.onClick.AddListener(() => onClick?.Invoke());
    }
    public void PostScore(int red, int blue) {
        var redScore = $"<color=red>{red}</color>";
        var blueScore = $"<color=blue>{blue}</color>";
        
        _scoreField.text = $"{blueScore} : {redScore}";
    }

    public void Dispose() {
        _exitButton.onClick.RemoveAllListeners();
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : StateScreen {
    
    [SerializeField] private Button _randomizeButton;
    [SerializeField] private Button _playButton;
    
    public void OnRandomizeTeams(Action onClick) => _randomizeButton.onClick.AddListener(() => onClick?.Invoke());
    public void OnStartPlaying(Action onClick) => _playButton.onClick.AddListener(() => onClick?.Invoke());
    
}
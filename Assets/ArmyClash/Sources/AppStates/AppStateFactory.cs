using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppStateFactory : MonoBehaviour {
    [SerializeField] private StateScreen[] _screens;

    [Space, Header("Bootstrap dependencies")]
    [SerializeField] private SceneMediator _mediator;
    
    public AppStates Create() {
        foreach (var screen in _screens) {
            screen.Hide();
        }
        
        return new AppStates(new IAppState[] {
            Create<BootstrapState, BootstrapScreen>(), // First boot;
            Create<MenuState, MenuScreen>(_mediator), // Prepare scene;
            Create<GamePlayState, GamePlayScreen>(_mediator), // Start simulation;
            Create<GameResultsState, GameResultsScreen>(), // complete simulation;
        });
    }
    
    private void OnValidate() => _screens = FindObjectsOfType<StateScreen>(true);

    private bool TryGetScreen<TScreen>(out TScreen screen) where TScreen : StateScreen {
        screen = _screens.OfType<TScreen>().FirstOrDefault();
        
        return screen != null;
    }

    private TScreen GetScreen<TScreen>() where TScreen : StateScreen {
        if (TryGetScreen(out TScreen screen)) {
            return screen;
        }
        return null;
    }

    private TState Create<TState, TScreen>(params object[] args) where TState : IAppState where TScreen: StateScreen {
        var source = new List<object>(args);
        source.Add(GetScreen<TScreen>());
        
        return (TState)Activator.CreateInstance(typeof(TState), source.ToArray());
    }
}
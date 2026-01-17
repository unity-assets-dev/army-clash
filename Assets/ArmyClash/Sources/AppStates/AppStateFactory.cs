using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppStateFactory : MonoBehaviour {
    [SerializeField] private StateScreen[] _screens;

    [Space, Header("Bootstrap dependencies")]
    [SerializeField] private SceneMediator _mediator;
    
    public AppStates Create() {
        return new AppStates(new IAppState[] {
            GetState<BootstrapState, BootstrapScreen>(), // First boot;
            GetState<MenuState, MenuScreen>(_mediator), // Prepare scene;
            GetState<GamePlayState, GamePlayScreen>(_mediator), // Start simulation;
            GetState<GameResultsState, GameResultsScreen>(_mediator), // complete simulation;
        });
    }
    
    private void OnValidate() => _screens = FindObjectsOfType<StateScreen>(true);

    private void OnEnable() {
        foreach (var screen in _screens) {
            screen.Hide();
        }
    }

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

    private TState GetState<TState, TScreen>(params object[] args) where TState : IAppState where TScreen: StateScreen {
        var source = new List<object>(args);
        source.Add(GetScreen<TScreen>());
        
        return (TState)Activator.CreateInstance(typeof(TState), source.ToArray());
    }
}
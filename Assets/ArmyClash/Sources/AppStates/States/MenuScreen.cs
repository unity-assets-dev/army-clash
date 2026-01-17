using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MenuScreen: StateScreen {}

public class AppStateFactory : MonoBehaviour {
    [SerializeField] private StateScreen[] _screens;

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
        
        var type = typeof(TState);
        var constructor = type
            .GetConstructors(BindingFlags.Public | BindingFlags.CreateInstance)[0];
        
        return (TState)constructor.Invoke(source.ToArray());
    }
    
    public AppStates Create() {
        var states = new AppStates(new IAppState[] {
            GetState<BootstrapState, BootstrapScreen>(),
            GetState<MenuState, MenuScreen>(),
            GetState<GamePlayState, GamePlayScreen>(),
            GetState<GameResultsState, GameResultsScreen>(),
        });
        
        return states;
    }
    
}
using System.Collections.Generic;
using System.Linq;

public interface IFromState {
    public static void To<TState>() where TState : IAppState => AppStates.Instance.ChangeState<TState>();
    public static void ReturnBack() => AppStates.Instance.ReturnBack();
}

public class AppStates  {
    public static AppStates Instance { get; set; }
    
    private readonly HashSet<IAppState> _states = new();
    private readonly Stack<IAppState> _history = new();
    
    private IAppState _currentState;
    public IAppState CurrentState => _currentState;

    public AppStates(IAppState[] states) {
        Instance = this;
        
        foreach (var state in states) {
            AddState(state);
        }
    }

    public void ReturnBack() {
        if (_history.Count > 0) {
            ChangeToState(_history.Pop());
        }
    }
    
    public void ChangeState<T>() where T : IAppState {
        var state = _states.OfType<T>().FirstOrDefault();
        
        if(_currentState != null) _history.Push(state);
        
        ChangeToState(state);
    }
    
    public void AddState(IAppState state) => _states.Add(state);

    private void ChangeToState(IAppState state) {
        if (_currentState != state) {
            _currentState?.OnStateExit();
            _currentState = state;
            _currentState?.OnStateEnter();
        }
    }
    
    
}

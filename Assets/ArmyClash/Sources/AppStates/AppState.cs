public abstract class VisibleState<TScreen> : IAppState where TScreen : StateScreen {

    protected TScreen Screen { get; private set; }
    
    public VisibleState(TScreen screen) {
        Screen = screen;
    }
    public void OnStateEnter() {
        Screen.Show();
        OnEnterState();
    }
    
    protected virtual void OnEnterState() {}
    public void OnStateExit() {
        OnExitState();
        Screen.Hide();
    }
    
    protected virtual void OnExitState() {}
}
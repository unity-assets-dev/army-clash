public class BootstrapState : VisibleState<BootstrapScreen> {
    
    public BootstrapState(BootstrapScreen screen) : base(screen) { }

    protected override void OnEnterState() {
        Utils.Delay(2f, IFromState.To<MenuState>);
    }
}


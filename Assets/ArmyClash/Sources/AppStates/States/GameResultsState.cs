public class GameResultsState : VisibleState<GameResultsScreen> {

    public GameResultsState( GameResultsScreen screen) : base(screen) {}

    protected override void OnEnterState() {
        Utils.Delay(3, IFromState.To<MenuState>);
    }
}
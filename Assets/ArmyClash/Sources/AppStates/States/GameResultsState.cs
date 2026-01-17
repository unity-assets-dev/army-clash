public class GameResultsState : VisibleState<GameResultsScreen> {
    private readonly SceneMediator _scene;

    public GameResultsState(SceneMediator scene, GameResultsScreen screen) : base(screen) {
        _scene = scene;
    }

    protected override void OnEnterState() {
        
    }
}
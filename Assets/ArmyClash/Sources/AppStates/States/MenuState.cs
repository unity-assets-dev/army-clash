public class MenuState : VisibleState<MenuScreen> {
    
    private readonly SceneMediator _scene;

    public MenuState(SceneMediator scene, MenuScreen screen) : base(screen) {
        _scene = scene;
    }

    protected override void OnEnterState() {
        _scene.PlaceUnits();
        
        Screen.OnRandomizeTeams(_scene.RandomizeTeams);
        
        Screen.OnStartPlaying(IFromState.To<GamePlayState>);
    }
}


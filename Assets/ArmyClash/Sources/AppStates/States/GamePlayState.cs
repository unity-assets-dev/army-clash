using System;

public class GamePlayState : VisibleState<GamePlayScreen> {
    
    private readonly SceneMediator _scene;
    private readonly Action _showResultCommand;

    public GamePlayState(SceneMediator scene, GamePlayScreen screen) : base(screen) {
        _scene = scene;

        _showResultCommand = IFromState.To<GameResultsState>;
    }

    protected override void OnEnterState() {
        _scene.OnCountRemainUnits(OnCountRemainUnits);
        _scene.StartSimulation();
    }

    private void OnCountRemainUnits(int count) {
        if (count <= 0) {
            _showResultCommand?.Invoke();
        }
    }

    protected override void OnExitState() {
        
    }
}


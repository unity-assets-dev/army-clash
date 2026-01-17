using System;

public class GamePlayState : VisibleState<GamePlayScreen> {
    
    private readonly SceneMediator _scene;
    private readonly Action _showResultCommand;

    public GamePlayState(SceneMediator scene, GamePlayScreen screen) : base(screen) {
        _scene = scene;

        _showResultCommand = () => {
            IFromState.To<GameResultsState>();
        };
    }

    protected override void OnEnterState() {
        _scene.OnCountRemainUnits(OnCountRemainUnits);
        _scene.StartSimulation();
        Screen.OnExitButtonPressed(_showResultCommand);
    }

    private void OnCountRemainUnits(int red, int blue) {
        Screen.PostScore(red, blue);
        if (red <= 0 || blue <= 0) {
            _showResultCommand?.Invoke();
        }
    }

    protected override void OnExitState() {
        _scene.DisposeScene();
        Screen.Dispose();
    }
}


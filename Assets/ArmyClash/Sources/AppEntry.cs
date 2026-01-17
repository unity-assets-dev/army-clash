using UnityEngine;

public class AppEntry : MonoBehaviour {

    [SerializeField] private AppStateFactory _factory;
    
    private AppStates _states;

    private void OnEnable() {
        _states = _factory.Create();
        _states.ChangeState<BootstrapState>();
    }
}

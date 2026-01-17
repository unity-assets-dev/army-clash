using UnityEngine;

public abstract class StateScreen : MonoBehaviour {
    private void OnValidate() {
        name = $"[{GetType().Name}]";
        OnValidateNext();
    }

    protected virtual void OnValidateNext() {}

    public void Show() {
        gameObject.SetActive(true);
        OnScreenShow();
    }

    protected virtual void OnScreenShow() {}

    public void Hide() {
        OnScreenHide();
        gameObject.SetActive(false);
    }

    protected virtual void OnScreenHide() {}
}
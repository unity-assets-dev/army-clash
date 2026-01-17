public static class ActorExtensions {
    public static bool Alive(this Actor actor) => actor != null && actor.gameObject.activeSelf;
    public static bool Dead(this Actor actor) => actor == null || !actor.gameObject.activeSelf;
}

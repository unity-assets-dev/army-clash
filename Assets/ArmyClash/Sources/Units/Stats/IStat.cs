public interface IStat {
    int Health { get;  }
    int Attack { get;  }
    int AttackSpeed { get;  }
    int Speed { get;  }
    IStat Transform(IStat stats);
    void ApplyModifiers(ActorModel model);
}
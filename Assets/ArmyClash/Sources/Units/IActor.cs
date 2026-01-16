using System;

public interface IActor: IDisposable {
    
    void Initialize();
    void Tick(float dt);
}
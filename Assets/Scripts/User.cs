using System.Collections.Generic;
using System.Numerics;

public class User
{
    public string name;
    public float bestTime;
    public List<Vector3> deathPositions;
    
    public User(string name)
    {
        this.name = name;
        this.bestTime = float.MaxValue;
        this.deathPositions = new List<Vector3>();
    }
}

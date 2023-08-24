using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum PowerUpType { Invincibility, SlowFalling, Ghost };

public abstract class PowerUp : IEquatable<PowerUp>
{

    public abstract PowerUpType type { get; }

    // To be called in an Update function, to update state regularly
    public abstract void Effect(Rigidbody player);

    public bool Equals(PowerUp other)
    {   
        return type == other.type;
    }
}


public class SlowFallingPowerUp : PowerUp
{
    public override PowerUpType type => PowerUpType.SlowFalling;

    public override void Effect(Rigidbody player)
    {
        player.AddForce(
            Vector3.up * 10f, ForceMode.Acceleration 
            );
    }

}

public class Invincibility : PowerUp
{
    public override PowerUpType type => PowerUpType.Invincibility;

    public override void Effect(Rigidbody player) {    }
}

public class PowerupsManager : MonoBehaviour
{
    public List<PowerUp> activePowerups = new List<PowerUp>();

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

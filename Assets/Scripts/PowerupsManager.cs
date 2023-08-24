using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum PowerUpType
{
    Invincibility,
    SlowFalling,
    Ghost
};

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
    public SlowFallingPowerUp(float upwardsAccelerationConstant)
    {
        this.upwardsAccelerationConstant = upwardsAccelerationConstant;
    }

    public override PowerUpType type => PowerUpType.SlowFalling;

    private float upwardsAccelerationConstant;

    public override void Effect(Rigidbody rigidbody)
    {
        Debug.Log("Inside effect");
        rigidbody.AddForce(
            Vector3.up * Mathf.Abs(Physics.gravity.y * upwardsAccelerationConstant), ForceMode.Acceleration
        );
    }
}

public class Invincibility : PowerUp
{
    public override PowerUpType type => PowerUpType.Invincibility;

    public override void Effect(Rigidbody player)
    {
    }
}

public class Ghost : PowerUp
{
    public override PowerUpType type => PowerUpType.Ghost;

    public override void Effect(Rigidbody player)
    {
    }
}

public class PowerupsManager : MonoBehaviour
{
    public Dictionary<PowerUpType, PowerUp> activePowerups = new Dictionary<PowerUpType, PowerUp>();
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // print(rigidbody.velocity.y);
        foreach (KeyValuePair<PowerUpType, PowerUp> entry in activePowerups)
        {
            entry.Value.Effect(rigidbody);
        }
    }
}
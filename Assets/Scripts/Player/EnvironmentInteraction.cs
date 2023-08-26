using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteraction : MonoBehaviour
{
    const string obstacleTag = "Obstacle";
    const string checkPointTag = "CheckPoint";

    const string powerupInvincibilityTag = "Powerup/Invincibility";
    const string powerupGhostTag = "Powerup/Ghost";
    private const string powerupSlowFalling = "Powerup/SlowFalling";

    [SerializeField] float ghostDuration = 5f;
    [SerializeField] int slowFallingDuration = 10;
    [SerializeField] float slowFallingAirMultiplier = 1.8f;
    [SerializeField] private float slowFallingUpwardAcceleration = 1.8f;

    private Vector3 lastCheckPoint;
    private float initialAirMultiplier;

    public Dictionary<PowerUpType, PowerUp> activePowerups;

    List<Collider> invincibleColliders = new List<Collider>();
    private CapsuleCollider capsuleCollider;
    private Rigidbody rigidbody;
    private PlayerMovement playerMovement;


    private void Awake()
    {
        lastCheckPoint = transform.position;
        activePowerups = (GetComponent<PowerupsManager>()).activePowerups;
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        initialAirMultiplier = playerMovement.airMultiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case obstacleTag:


                if (activePowerups.ContainsKey(PowerUpType.Ghost)) return;
                if (invincibleColliders.Contains(other)) return;

                if (activePowerups.ContainsKey(PowerUpType.Invincibility))
                {
                    invincibleColliders.Add(other);
                    activePowerups.Remove(PowerUpType.Invincibility);
                    return;
                }

                ResetToLastCheckpoint();
                break;
            case checkPointTag:
                lastCheckPoint = other.transform.GetChild(0).position;
                break;
            case powerupInvincibilityTag:

                if (!activePowerups.ContainsKey(PowerUpType.Invincibility))
                {
                    activePowerups.Add(PowerUpType.Invincibility, new Invincibility());
                }


                break;
            case powerupGhostTag:
                if (!activePowerups.ContainsKey(PowerUpType.Ghost))
                {
                    activePowerups.Add(PowerUpType.Ghost, new Ghost());
                    capsuleCollider.isTrigger = true;
                    StartCoroutine(Wait(ghostDuration, DisableGhost));
                }

                break;
            case powerupSlowFalling:
                if (!activePowerups.ContainsKey(PowerUpType.Ghost))
                {
                    playerMovement.airMultiplier = slowFallingAirMultiplier;
                    activePowerups.Add(PowerUpType.SlowFalling, new SlowFallingPowerUp(slowFallingUpwardAcceleration));
                    StartCoroutine(Wait(slowFallingDuration, DisableSlowFalling));
                }


                break;
        }

        if (other.tag.Contains("Powerup"))
        {
            Destroy(other.gameObject);
        }
    }

    private void ResetToLastCheckpoint()
    {
        // Reset powerups
        DisableGhost();
        DisableSlowFalling();
        invincibleColliders.Clear();
        activePowerups.Clear();

        transform.position = lastCheckPoint;
    }

    private void DisableGhost()
    {
        capsuleCollider.isTrigger = false;
        activePowerups.Remove(PowerUpType.Ghost);
    }

    private void DisableSlowFalling()
    {
        activePowerups.Remove(PowerUpType.SlowFalling);
        playerMovement.airMultiplier = initialAirMultiplier;
    }

    private IEnumerator Wait(float seconds, Action predicate)
    {
        yield return new WaitForSeconds(seconds);

        predicate();
    }
}
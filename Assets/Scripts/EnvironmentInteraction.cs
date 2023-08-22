using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteraction : MonoBehaviour
{
    const string obstacleTag = "Obstacle";
    const string checkPointTag = "CheckPoint";

    const string powerupInvincibilityTag = "Powerup/Invincibility";

    private Vector3 lastCheckPoint;

    public List<PowerUp> activePowerups;

    List<Collider> invincibleColliders = new List<Collider>();

    private void Awake()
    {
        lastCheckPoint = transform.position;
        activePowerups = (GetComponent<PowerupsManager>()).activePowerups;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case obstacleTag:
                if (invincibleColliders.Contains(other)) return;

                if (activePowerups.Contains(new Invincibility()))
                {
                    invincibleColliders.Add(other);
                    activePowerups.Remove(new Invincibility());
                    return;
                }

                ResetToLastCheckpoint();
                break;
            case checkPointTag:
                lastCheckPoint = other.transform.GetChild(0).position;
                break;
            case powerupInvincibilityTag:
                print(activePowerups);
                if (!activePowerups.Contains(new Invincibility()))
                {
                    activePowerups.Add(new Invincibility());
                }
                break;
        }
    }

    private void ResetToLastCheckpoint()
    {
        transform.position = lastCheckPoint;
        invincibleColliders.Clear();
    }
}
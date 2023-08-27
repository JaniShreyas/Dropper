using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timerText;
    decimal timer = 0;
    bool levelComplete = false;

    void Update()
    {
        if (levelComplete) return;

        timer += (decimal)Time.deltaTime;
        decimal timeScore = Math.Round(timer, 2);
        timerText.text = $"Time Spent: {timeScore}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            levelComplete = true;
            print("Level Complete");
        }
    }
}

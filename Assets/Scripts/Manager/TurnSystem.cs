using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public event EventHandler onTurnChanged;
    [SerializeField] private TextMeshProUGUI turnText;
    private bool isPlayerTurn = true;
    private int turn = 1;

    private float timer = 2f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("There can only be one TurnSystem instance");
        }
        Instance = this;
    }
    void Update()
    {
        if (!isPlayerTurn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 2f;
                NextTurn();
            }
        }
    }
    public void NextTurn()
    {
        turn += 1;
        isPlayerTurn = !isPlayerTurn;
        turnText.text = "Turn: " + turn + (isPlayerTurn? " (Player)" : " (Enemy)");
        onTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

}

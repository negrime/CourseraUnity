using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerStats
{
    [SerializeField]
    private int _scores;
    [SerializeField]
    private int _jumps;

    public void UpdateScores(int value)
    {
        _scores += value;
    }

    public void UpdateJumps(int value)
    {
        _jumps += value;
    }

    public int GetScores()
    {
        return _scores;
    }

    public int GetJumps()
    {
        return _jumps;
    }
}
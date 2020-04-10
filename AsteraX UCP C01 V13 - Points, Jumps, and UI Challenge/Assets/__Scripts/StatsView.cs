using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour
{
    [SerializeField]
    private Text _scoresTxt;
    
    [SerializeField]
    private Text _jumpsTxt;

    private string _jumpStr = "Jumps: ";
    private string _scoresStr = "Scores: ";
    
    public void ShowScores(int value)
    {
        _scoresTxt.text = _scoresStr + value;
    }
    
    public void ShowJumps(int value)
    {
        _jumpsTxt.text = _jumpStr + value;
    }
}

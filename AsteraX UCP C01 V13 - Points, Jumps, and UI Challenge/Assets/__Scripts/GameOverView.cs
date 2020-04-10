using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{

    [SerializeField]
    private Text _finalScoreTxt;



    public void EnableGameOver(int value)
    {
        _finalScoreTxt.text += value;
        StartCoroutine(RestartScene(5));
    }


    private IEnumerator RestartScene(float value)
    {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene(0);
    }

}

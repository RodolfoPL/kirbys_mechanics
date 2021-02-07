/*
    Script that controls the Game Overs
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameControl : MonoBehaviour
{
    private int currentLevel = 0;
    [SerializeField] private string[] typesOfDeaths = new string[] { };

    private void Awake() {
        //typesOfDeaths = new string[1]{"Fallen on Water"};
    }
    public void GameOver(GameObject player, int deathIndex){
        Debug.Log("GAME OVER You Die " +  typesOfDeaths[deathIndex]);
        Destroy(player, 0.3f);
        Restart();
    }

    private void Restart(){
        SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem.EntityTree;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverMenu;
    private void Start()
    {
        gameOverMenu.SetActive(false);
        PlayerIdentity.OnPlayerDeath += EndGame;
    }

    private void EndGame()
    {
        this.gameObject.GetComponent<LevelUpMenu>().CloseLevelUpMenu();
        this.gameObject.GetComponent<LevelUpMenu>().levelUpMenu.SetActive(false);
        this.gameObject.GetComponent<LevelUpMenu>().enabled = false;
        gameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        // Carrega a cena novamente
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

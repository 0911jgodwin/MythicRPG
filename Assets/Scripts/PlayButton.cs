using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Animator anim;
    public bool gameStarting = false;
    public float restartDelay = 15f;
    float restartTimer = 0f;

    public void PlayGame()
    {
        anim.SetTrigger("GameStart");
        gameStarting = true;
        
    }

    private void Update()
    {
        if(gameStarting)
        {
            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        
    }
}

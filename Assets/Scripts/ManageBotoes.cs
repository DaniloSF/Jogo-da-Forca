using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    public int comecoTimer;
    public bool comecar;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /// <summary>
        /// Esperar um tempo antes de começar o jogo, e assim carrega a proxima cena
        /// </summary>
        if (comecar)
        {
            comecoTimer--;
            if (comecoTimer <= 0)
            {
                SceneManager.LoadScene("Game");
            }
        }

    }

    public void StartGame()
    {
        comecar = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    public int changeDelay = 60;
    // 1 = StartGame
    // 2 = MostrarCreditos
    // 3 = FecharJogo
    public int botao = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /// <summary>
        /// Dependendo do botão apertado carregar a proxima cena, 1 começa a forca com um delay, 2 vai para os creditos e 3 fecha o jogo.
        /// </summary>
        if (botao > 0)
        {
            switch (botao)
            {
                case 1:
                    changeDelay--;
                    if (changeDelay <= 0)
                    {
                        SceneManager.LoadScene("Game");
                    }
                    break;
                case 2:
                    changeDelay--;
                    if (changeDelay <= 0)
                    {
                        SceneManager.LoadScene("Creditos");
                    }
                    break;
                case 3:
                    Application.Quit();
                    botao = 0;
                    Debug.Log("Game is exiting");
                    break;
            }
            
        }

    }

    public void StartGame()
    {
        botao = 1;
    }

    public void MostrarCreditos()
    {
        botao = 2;
        changeDelay = 15;
    }
    
    public void FecharJogo()
    {
        botao = 3;
        changeDelay = 15;
    }
}

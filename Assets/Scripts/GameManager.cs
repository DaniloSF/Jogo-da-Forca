using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numTentativas;
    private int maxNumTentativas;
    int score = 0;

    public GameObject letra;
    public GameObject centro;
    public AudioManager audioManager;

    private string palavraOculta = "";
    private string[] palavrasOcultas = new string[] {"carro","elefante","futebol"};
    private int tamanhoPalavraOculta;
    char[] letrasOcultas;
    bool[] letrasDescobertas;

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");
        GameObject audioObject = GameObject.Find("AudioManager");
        audioManager = audioObject.GetComponent<AudioManager>();
        InitGame();
        InitLetras();

        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTeclado();
    }

    //Inicia palavra e coloca letras na tela
    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for(int i=0; i<numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f)*80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i+1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    void InitGame()
    {
        //palavraOculta = "elefante";
        //int numeroAleatorio = Random.Range(0, palavrasOcultas.Length);
        //palavraOculta = palavrasOcultas[numeroAleatorio];

        palavraOculta = PegaPalavraDoArquivo();
        tamanhoPalavraOculta = palavraOculta.Length;
        palavraOculta = palavraOculta.ToUpper();
        letrasOcultas = new char[tamanhoPalavraOculta];
        letrasDescobertas = new bool[tamanhoPalavraOculta];
        letrasOcultas = palavraOculta.ToCharArray();
    }

    void CheckTeclado()
    {
        if(Input.anyKeyDown)
        {
            char[] inputPalavra = Input.inputString.ToCharArray();
            if (inputPalavra == null || inputPalavra.Length == 0) return;
            
            char letraTeclada = inputPalavra[0];
            int letraTecladoComoInt = System.Convert.ToInt32(letraTeclada);

            if(numTentativas >= maxNumTentativas)
            {
                SceneManager.LoadScene("GameOver");
            }

            if(letraTecladoComoInt >= 97 && letraTecladoComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                bool acertou = false;
                for(int i=0; i<tamanhoPalavraOculta; i++)
                {
                    if(!letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            acertou = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            VerificaSePalavraDescoberta();
                        }
                    }
                }
                if (acertou)//som
                {
                    audioManager.PlaySound("beep");
                }
                else
                {
                    audioManager.PlaySound("error-01");
                }
            }
        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
    }

    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;
        for(int i=0; i<tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        if(condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta",palavraOculta);
            SceneManager.LoadScene("Victory");
        }
        
        
    }

    string PegaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavrasOcultas",typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length);
        return (palavras[palavraAleatoria]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //variaveis relacionadas ao score do jogo
    private int numTentativas;
    private int maxNumTentativas;
    int score = 0;

    //variaveis da engine
    public GameObject letra;
    public GameObject centro;
    public AudioManager audioManager;

    //variaveis relacionadas a palavra oculta
    private string palavraOculta = "";
    private string[] palavrasOcultas = new string[] {"carro","elefante","futebol"};
    private int tamanhoPalavraOculta;
    char[] letrasOcultas;
    bool[] letrasDescobertas;

    //array para armazenar as letras testadas
    bool[] letrasTestadas;

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
        UpdateTentativas("");
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

    //inicia os elementos do jogo, incluindo as variaveis relacionadas a palavra oculta e o array de letras testadas
    void InitGame()
    {
        palavraOculta = PegaPalavraDoArquivo(); 
        tamanhoPalavraOculta = palavraOculta.Length; 
        palavraOculta = palavraOculta.ToUpper(); 
        letrasOcultas = new char[tamanhoPalavraOculta]; 
        letrasOcultas = palavraOculta.ToCharArray(); 
        letrasDescobertas = new bool[tamanhoPalavraOculta]; 

        letrasTestadas = new bool[26];
        for(int i=0; i<26; i++)
        {
            letrasTestadas[i] = false;
        }
    }

    //realiza o tratamento de input, verificando se uma letra foi teclada e se ele esta na palavra ou nao
    void CheckTeclado()
    {
        if(Input.anyKeyDown) //checa se alguma tecla foi pressionada
        {
            char[] inputPalavra = Input.inputString.ToCharArray();
            if (inputPalavra == null || inputPalavra.Length == 0) return;
            
            char letraTeclada = inputPalavra[0];
            int letraTecladoComoInt = System.Convert.ToInt32(letraTeclada);

            if(letraTecladoComoInt >= 97 && letraTecladoComoInt <= 122)
            {
                if(!letrasTestadas[letraTecladoComoInt-97]) //checa se a letra ja foi testada
                {
                    letrasTestadas[letraTecladoComoInt-97] = true;
                    bool acertou = false;
                    for(int i=0; i<tamanhoPalavraOculta; i++)
                    {
                        if(!letrasDescobertas[i]) //checa se a letra ja foi descoberta
                        {
                            letraTeclada = System.Char.ToUpper(letraTeclada);
                            if (letrasOcultas[i] == letraTeclada) //checa se a letra esta na palavra
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
                    if (acertou)//se a letra foi acertada, toca um som positivo
                    {
                        audioManager.PlaySound("beep");
                    }
                    else //senao, toca um som negativo e aumenta o numero de tentativas
                    {
                        audioManager.PlaySound("error-01");
                        numTentativas++;
                        UpdateTentativas(letraTeclada.ToString());
                    }
                }
            }
            
            if(numTentativas > maxNumTentativas) //checa se o numero de tentativas ultrapassou o maximo, resultando em game over
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    //atualiza o numero de tentativas e as letras testadas na tela
    void UpdateTentativas(string letraTeclada)
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
        GameObject.Find("letrasTestadas").GetComponent<Text>().text += letraTeclada + " ";
    }

    //atualiza o score na tela
    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
    }

    //checa se todas as letras foram descobertas
    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;
        for(int i=0; i<tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        if(condicao) //se todas as letras foram descobertas, vai para a tela de vitoria
        {
            PlayerPrefs.SetString("ultimaPalavraOculta",palavraOculta);
            SceneManager.LoadScene("Victory");
        }
        
        
    }

    //escolhe uma palavra aleatoria do arquivo de palavras
    string PegaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavrasOcultas",typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length);
        return (palavras[palavraAleatoria]);
    }
}

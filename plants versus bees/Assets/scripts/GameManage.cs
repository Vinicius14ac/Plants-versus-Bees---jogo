using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Telas da UI")]
    public GameObject telaDeTitulo;
    public GameObject telaDeMorte;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Começa o jogo mostrando o título
        MostrarTelaTitulo();
    }

    public void MostrarTelaTitulo()
    {
        telaDeTitulo.SetActive(true);
        telaDeMorte.SetActive(false);
        Time.timeScale = 0f; // Pausa tudo que se move no jogo
    }

    public void IniciarJogo()
    {
        telaDeTitulo.SetActive(false);
        telaDeMorte.SetActive(false);
        Time.timeScale = 1f; // O jogo volta a rodar na velocidade normal
    }

    public void GameOver()
    {
        telaDeMorte.SetActive(true);
        Time.timeScale = 0f; // Pausa o jogo quando o jogador morre
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f; // Garante que o tempo volte ao normal antes de recarregar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarrega o nível atual
    }
}
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
        Time.timeScale = 1f;

        if (telaDeMorte != null)
        {
            telaDeMorte.SetActive(false);
        }

        // Só abre a Tela de Título se ela existir nesta fase (na Fase 2 fica vazio sem dar erro)
        if (telaDeTitulo != null)
        {
            telaDeTitulo.SetActive(true);
        }
    }

    public void IniciarJogo()
    {
        if (telaDeTitulo != null)
        {
            telaDeTitulo.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (telaDeMorte != null)
        {
            telaDeMorte.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
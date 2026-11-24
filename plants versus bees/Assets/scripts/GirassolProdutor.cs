using UnityEngine;

public class GirassolProdutor : MonoBehaviour
{
    public int dinheiroGerado = 25;
    public float intervaloGeracao = 5f;

    private float cronometro;

    void Update()
    {
        // Só gera dinheiro depois que o jogador clicar em "Pronto"
        if (GerenciadorOndas.instance != null && !GerenciadorOndas.instance.batalhaIniciada)
        {
            return;
        }

        cronometro += Time.deltaTime;

        if (cronometro >= intervaloGeracao)
        {
            if (GerenciadorDinheiro.instance != null)
            {
                GerenciadorDinheiro.instance.AdicionarDinheiro(dinheiroGerado);
            }
            cronometro = 0f;
        }
    }
}
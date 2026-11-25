using UnityEngine;

public class InimigoVida : MonoBehaviour
{
    public int vida = 3;
    public int recompensaDinheiro = 15;

    private bool morreu = false;

    public void ReceberDano(int quantidade)
    {
        if (morreu) return;

        vida -= quantidade;

        if (vida <= 0)
        {
            morreu = true;

            if (GerenciadorDinheiro.instance != null)
            {
                GerenciadorDinheiro.instance.AdicionarDinheiro(recompensaDinheiro);
            }

            Destroy(gameObject);
        }
    }
}
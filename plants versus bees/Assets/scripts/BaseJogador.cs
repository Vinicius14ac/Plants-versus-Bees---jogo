using UnityEngine;
using UnityEngine.UI;

public class BaseJogador : MonoBehaviour
{
    public int vida = 5;
    public Slider barraDeVida;

    void Start()
    {
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vida;
            barraDeVida.value = vida;
        }
    }

    public void ReceberDano(int quantidade)
    {
        vida -= quantidade;
        vida = Mathf.Max(vida, 0);

        if (barraDeVida != null)
        {
            barraDeVida.value = vida;
        }

        if (vida <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Inimigo"))
        {
            ReceberDano(1);
            Destroy(outro.gameObject);
        }
    }
}
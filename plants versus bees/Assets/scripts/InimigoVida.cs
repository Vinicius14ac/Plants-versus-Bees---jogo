using UnityEngine;

public class InimigoVida : MonoBehaviour
{
    public int vida = 3;

    public void ReceberDano(int quantidade)
    {
        vida -= quantidade;

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
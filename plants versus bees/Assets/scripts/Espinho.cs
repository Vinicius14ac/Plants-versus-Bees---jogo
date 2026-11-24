using System.Collections.Generic;
using UnityEngine;

public class Espinho : MonoBehaviour
{
    public float velocidade = 8f;
    public int dano = 1;
    public float tempoDeVida = 4f;

    // Lista para garantir que cada inimigo leve dano apenas 1 vez por espinho
    private List<GameObject> inimigosAtingidos = new List<GameObject>();

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        transform.Translate(Vector3.right * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Inimigo") && !inimigosAtingidos.Contains(outro.gameObject))
        {
            inimigosAtingidos.Add(outro.gameObject);

            InimigoVida vidaInimigo = outro.GetComponent<InimigoVida>();
            if (vidaInimigo != null)
            {
                vidaInimigo.ReceberDano(dano);
            }
            // Como não destruímos o espinho aqui, ele continua atravessando os inimigos
        }
    }
}

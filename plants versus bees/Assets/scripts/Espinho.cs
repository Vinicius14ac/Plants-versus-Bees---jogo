using UnityEngine;

public class Espinho : MonoBehaviour
{
    public float velocidade = 8f;
    public int dano = 1;
    [HideInInspector] public float distanciaMaxima = 4.5f;

    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Move na direção para a qual a Rosa está virada (direita ou esquerda)
        transform.position += transform.right * velocidade * Time.deltaTime;

        // Destrói o espinho ao atingir o alcance máximo da Rosa
        if (Vector2.Distance(posicaoInicial, transform.position) >= distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Inimigo"))
        {
            InimigoVida vidaInimigo = outro.GetComponent<InimigoVida>();
            if (vidaInimigo != null)
            {
                vidaInimigo.ReceberDano(dano);
            }
            Destroy(gameObject);
        }
    }
}
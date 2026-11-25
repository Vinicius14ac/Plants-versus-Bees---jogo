using UnityEngine;

public class RosaAtaque : MonoBehaviour
{
    [Header("Configuração de Tiro")]
    public GameObject espinhoPrefab;
    public Transform pontoDisparo;
    public float alcance = 4.2f;
    public float alturaDeteccao = 0.9f;
    public float intervaloTiro = 1.4f;

    private float cronometro;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= intervaloTiro && TemInimigoNoAlcance())
        {
            Atirar();
            cronometro = 0f;
        }
    }

    bool TemInimigoNoAlcance()
    {
        Vector2 centroArea = (Vector2)transform.position + (Vector2)transform.right * (alcance / 2f);
        Vector2 tamanhoArea = new Vector2(alcance, alturaDeteccao);

        Collider2D[] encontrados = Physics2D.OverlapBoxAll(centroArea, tamanhoArea, 0f);
        foreach (Collider2D col in encontrados)
        {
            if (col.CompareTag("Inimigo"))
            {
                return true;
            }
        }
        return false;
    }

    void Atirar()
    {
        if (espinhoPrefab == null) return;

        Vector3 localSaida = (pontoDisparo != null) ? pontoDisparo.position : transform.position;
        GameObject novoEspinho = Instantiate(espinhoPrefab, localSaida, transform.rotation);

        Espinho scriptEspinho = novoEspinho.GetComponent<Espinho>();
        if (scriptEspinho != null)
        {
            scriptEspinho.distanciaMaxima = alcance;
        }
    }

    // Mostra no Editor o retângulo exato onde a Rosa enxerga e atira
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 centroArea = transform.position + transform.right * (alcance / 2f);
        Gizmos.DrawWireCube(centroArea, new Vector3(alcance, alturaDeteccao, 0f));
    }
}
using UnityEngine;

public class InimigoMovimento : MonoBehaviour
{
    public Transform[] pontos;
    public float velocidade = 2f;

    private int indiceAtual = 0;

    void Update()
    {
        if (pontos == null || pontos.Length == 0 || indiceAtual >= pontos.Length)
        {
            return;
        }

        Transform pontoAlvo = pontos[indiceAtual];
        transform.position = Vector2.MoveTowards(transform.position, pontoAlvo.position, velocidade * Time.deltaTime);

        if (Vector2.Distance(transform.position, pontoAlvo.position) < 0.05f)
        {
            indiceAtual++;
        }
    }
}
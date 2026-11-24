using UnityEngine;

public class RosaAtiradora : MonoBehaviour
{
    public GameObject espinhoPrefab;
    public float tempoEntreTiros = 1.5f;

    private float cronometro;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= tempoEntreTiros)
        {
            Atirar();
            cronometro = 0f;
        }
    }

    void Atirar()
    {
        if (espinhoPrefab != null)
        {
            Instantiate(espinhoPrefab, transform.position, transform.rotation);
        }
    }
}
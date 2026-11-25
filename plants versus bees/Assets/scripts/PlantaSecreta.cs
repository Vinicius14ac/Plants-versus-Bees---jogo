using UnityEngine;

public class PlantaSecreta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D outro)
    {
        EliminarAbelha(outro);
    }

    private void OnTriggerStay2D(Collider2D outro)
    {
        EliminarAbelha(outro);
    }

    void EliminarAbelha(Collider2D outro)
    {
        InimigoVida vidaInimigo = outro.GetComponentInParent<InimigoVida>();
        if (vidaInimigo != null && (outro.CompareTag("Inimigo") || vidaInimigo.CompareTag("Inimigo")))
        {
            vidaInimigo.ReceberDano(99999);
        }
    }
}
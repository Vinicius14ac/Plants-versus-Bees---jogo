using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirassolRaioSol : MonoBehaviour
{
    [Header("Configuração do Raio de Sol")]
    public float raioAtaque = 2.2f;
    public int dano = 1;
    public float intervaloAtaque = 2f;

    [Header("Efeito Visual (Opcional)")]
    public SpriteRenderer visualPulso;
    public Color corPulso = new Color(1f, 0.9f, 0.2f, 0.55f);

    private float cronometro;

    void Start()
    {
        if (visualPulso != null)
        {
            visualPulso.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= intervaloAtaque)
        {
            if (TentarLiberarRaioDeSol())
            {
                cronometro = 0f;
            }
        }
    }

    bool TentarLiberarRaioDeSol()
    {
        Collider2D[] alvos = Physics2D.OverlapCircleAll(transform.position, raioAtaque);
        HashSet<InimigoVida> inimigosAtingidos = new HashSet<InimigoVida>();

        foreach (Collider2D col in alvos)
        {
            InimigoVida vidaInimigo = col.GetComponentInParent<InimigoVida>();
            if (vidaInimigo != null && (col.CompareTag("Inimigo") || vidaInimigo.CompareTag("Inimigo")))
            {
                inimigosAtingidos.Add(vidaInimigo);
            }
        }

        if (inimigosAtingidos.Count > 0)
        {
            foreach (InimigoVida inimigo in inimigosAtingidos)
            {
                inimigo.ReceberDano(dano);
            }

            if (visualPulso != null)
            {
                StopAllCoroutines();
                StartCoroutine(AnimarPulsoSolar());
            }

            return true;
        }

        return false;
    }

    IEnumerator AnimarPulsoSolar()
    {
        visualPulso.gameObject.SetActive(true);
        float duracao = 0.35f;
        float tempo = 0f;
        float diametroFinal = raioAtaque * 2f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / duracao;

            float escalaAtual = Mathf.Lerp(0.2f, diametroFinal, progresso);
            visualPulso.transform.localScale = new Vector3(escalaAtual, escalaAtual, 1f);

            Color c = corPulso;
            c.a = Mathf.Lerp(corPulso.a, 0f, progresso);
            visualPulso.color = c;

            yield return null;
        }

        visualPulso.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioAtaque);
    }
}
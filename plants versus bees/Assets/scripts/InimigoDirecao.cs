using UnityEngine;

public class InimigoDirecao : MonoBehaviour
{
    [Header("Sprites do Inimigo")]
    public Sprite spriteDireita;
    public Sprite spriteBaixo;

    [Tooltip("Opcional: se deixar vazio, ele vira o sprite de baixo para cima automaticamente")]
    public Sprite spriteCima;

    private SpriteRenderer spriteRenderer;
    private Vector3 posicaoAnterior;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicaoAnterior = transform.position;
    }

    // LateUpdate roda logo após o InimigoMovimento mover o objeto
    void LateUpdate()
    {
        Vector3 movimento = transform.position - posicaoAnterior;

        // Só atualiza se o inimigo realmente saiu do lugar (evita bugs quando o jogo está pausado)
        if (movimento.sqrMagnitude > 0.00001f)
        {
            AtualizarSprite(movimento);
        }

        posicaoAnterior = transform.position;
    }

    void AtualizarSprite(Vector3 direcao)
    {
        // Verifica se o movimento principal no momento é horizontal (X) ou vertical (Y)
        if (Mathf.Abs(direcao.x) > Mathf.Abs(direcao.y))
        {
            spriteRenderer.sprite = spriteDireita;
            spriteRenderer.flipY = false;

            // Se estiver indo para a esquerda (X negativo), espelha o sprite da direita
            spriteRenderer.flipX = direcao.x < 0f;
        }
        else
        {
            spriteRenderer.flipX = false;

            if (direcao.y < 0f)
            {
                // Indo para baixo
                spriteRenderer.sprite = spriteBaixo;
                spriteRenderer.flipY = false;
            }
            else
            {
                // Indo para cima
                if (spriteCima != null)
                {
                    spriteRenderer.sprite = spriteCima;
                    spriteRenderer.flipY = false;
                }
                else
                {
                    spriteRenderer.sprite = spriteBaixo;
                    spriteRenderer.flipY = true;
                }
            }
        }
    }
}
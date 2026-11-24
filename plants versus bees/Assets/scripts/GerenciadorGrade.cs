using UnityEngine;

public class GerenciadorGrade : MonoBehaviour
{
    [Header("Prefabs das Plantas")]
    public GameObject girassolPrefab;
    public GameObject rosaPrefab;

    [Header("Custo das Plantas")]
    public int custoGirassol = 50;
    public int custoRosa = 100;

    [Header("Configuração da Grade")]
    public float larguraCelula = 1f;
    public float alturaCelula = 1f;
    public Vector2 offsetGrade = Vector2.zero;

    [Header("Pré-visualização")]
    [Range(0.1f, 1f)]
    public float transparenciaPreview = 0.55f;

    private GameObject plantaSelecionada;
    private GameObject previewAtual;
    private float rotacaoAtual = 0f; // 0 = Direita | 180 = Esquerda

    void Start()
    {
        SelecionarPlanta(girassolPrefab);
    }

    void Update()
    {
        // Tecla 1 seleciona o Girassol
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelecionarPlanta(girassolPrefab);
        }

        // Tecla 2 seleciona a Rosa
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelecionarPlanta(rosaPrefab);
        }

        // Tecla R alterna a Rosa apenas entre Direita (0°) e Esquerda (180°)
        if (Input.GetKeyDown(KeyCode.R) && plantaSelecionada == rosaPrefab)
        {
            rotacaoAtual = (rotacaoAtual == 0f) ? 180f : 0f;
        }

        AtualizarPreview();

        // Clique esquerdo coloca a planta selecionada
        if (Input.GetMouseButtonDown(0))
        {
            PosicionarPlanta();
        }
    }

    void SelecionarPlanta(GameObject novaPlanta)
    {
        if (novaPlanta == null) return;

        plantaSelecionada = novaPlanta;

        if (previewAtual != null)
        {
            Destroy(previewAtual);
        }

        previewAtual = Instantiate(plantaSelecionada);
        previewAtual.name = "PreviewPlanta";

        foreach (Collider2D col in previewAtual.GetComponentsInChildren<Collider2D>())
        {
            Destroy(col);
        }

        foreach (MonoBehaviour script in previewAtual.GetComponentsInChildren<MonoBehaviour>())
        {
            Destroy(script);
        }

        foreach (SpriteRenderer sr in previewAtual.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder += 10;
        }
    }

    void AtualizarPreview()
    {
        if (previewAtual == null) return;

        Vector3 posicaoAlinhada = ObterPosicaoAlinhadaMouse();
        previewAtual.transform.position = posicaoAlinhada;

        bool ehRosa = (plantaSelecionada == rosaPrefab);
        AplicarOrientacao(previewAtual, ehRosa);

        // Fica vermelho transparente se o quadrado estiver ocupado OU se faltar dinheiro
        bool podeColocar = PodePlantar(posicaoAlinhada) && PodeComprar();
        Color corPreview = podeColocar
            ? new Color(1f, 1f, 1f, transparenciaPreview)
            : new Color(1f, 0.35f, 0.35f, transparenciaPreview);

        foreach (SpriteRenderer sr in previewAtual.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = corPreview;
        }
    }

    void PosicionarPlanta()
    {
        if (plantaSelecionada == null) return;

        Vector3 posicaoAlinhada = ObterPosicaoAlinhadaMouse();

        if (PodePlantar(posicaoAlinhada))
        {
            if (GerenciadorDinheiro.instance == null || GerenciadorDinheiro.instance.TentarGastarDinheiro(ObterCustoAtual()))
            {
                GameObject novaPlanta = Instantiate(plantaSelecionada, posicaoAlinhada, Quaternion.identity);
                AplicarOrientacao(novaPlanta, plantaSelecionada == rosaPrefab);
            }
        }
    }

    int ObterCustoAtual()
    {
        if (plantaSelecionada == girassolPrefab) return custoGirassol;
        if (plantaSelecionada == rosaPrefab) return custoRosa;
        return 0;
    }

    bool PodeComprar()
    {
        if (GerenciadorDinheiro.instance == null) return true;
        return GerenciadorDinheiro.instance.TemDinheiroSuficiente(ObterCustoAtual());
    }

    void AplicarOrientacao(GameObject objeto, bool ehRosa)
    {
        float angulo = ehRosa ? rotacaoAtual : 0f;
        objeto.transform.rotation = Quaternion.Euler(0f, 0f, angulo);

        SpriteRenderer sr = objeto.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipY = ehRosa && Mathf.Approximately(rotacaoAtual, 180f);
        }
    }

    Vector3 ObterPosicaoAlinhadaMouse()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;

        float xGrid = Mathf.Floor((posicaoMouse.x - offsetGrade.x) / larguraCelula) * larguraCelula + (larguraCelula / 2f) + offsetGrade.x;
        float yGrid = Mathf.Floor((posicaoMouse.y - offsetGrade.y) / alturaCelula) * alturaCelula + (alturaCelula / 2f) + offsetGrade.y;

        return new Vector3(xGrid, yGrid, 0f);
    }

    bool PodePlantar(Vector3 posicao)
    {
        Collider2D objetoNoLocal = Physics2D.OverlapCircle(posicao, 0.3f);
        return objetoNoLocal == null || objetoNoLocal.isTrigger;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        for (int x = -10; x <= 10; x++)
        {
            for (int y = -6; y <= 6; y++)
            {
                Vector3 centro = new Vector3(
                    (x * larguraCelula) + (larguraCelula / 2f) + offsetGrade.x,
                    (y * alturaCelula) + (alturaCelula / 2f) + offsetGrade.y,
                    0
                );
                Gizmos.DrawWireCube(centro, new Vector3(larguraCelula, alturaCelula, 0));
            }
        }
    }
}
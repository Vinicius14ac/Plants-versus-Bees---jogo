using UnityEngine;
using UnityEngine.EventSystems;

public class GerenciadorGrade : MonoBehaviour
{
    [Header("Prefabs das Plantas")]
    public GameObject girassolPrefab;
    public GameObject rosaPrefab;
    public GameObject plantaSecretaPrefab;

    [Header("Custo das Plantas")]
    public int custoGirassol = 50;
    public int custoRosa = 100;

    [Header("Bloqueio e Pré-visualização")]
    public float raioOcupacao = 0.45f;
    [Range(0.1f, 1f)]
    public float transparenciaPreview = 0.55f;
    public GameObject telaTitulo;

    private GameObject plantaSelecionada;
    private GameObject previewAtual;
    private float rotacaoAtual = 0f;

    // Sequência: Cima, Cima, Baixo, Baixo, Esquerda, Direita, Esquerda, Direita
    private readonly KeyCode[] sequenciaSecreta = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow
    };
    private int indiceCodigo = 0;
    private bool secretoDesbloqueado = false;

    void Start()
    {
        SelecionarPlanta(girassolPrefab);
    }

    void Update()
    {
        if (telaTitulo != null && telaTitulo.activeInHierarchy)
        {
            if (previewAtual != null) previewAtual.SetActive(false);
            return;
        }

        if (previewAtual != null && !previewAtual.activeSelf)
        {
            previewAtual.SetActive(true);
        }

        VerificarCodigoSecreto();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelecionarPlanta(girassolPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelecionarPlanta(rosaPrefab);
        }

        // Depois de digitar o código uma vez, a tecla 3 também seleciona o quadrado secreto
        if (secretoDesbloqueado && Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelecionarPlanta(plantaSecretaPrefab);
        }

        if (Input.GetKeyDown(KeyCode.R) && plantaSelecionada == rosaPrefab)
        {
            rotacaoAtual = (rotacaoAtual == 0f) ? 180f : 0f;
        }

        AtualizarPreview();

        if (Input.GetMouseButtonDown(0) && !IsMouseSobreUI())
        {
            PosicionarPlanta();
        }
    }

    void VerificarCodigoSecreto()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(sequenciaSecreta[indiceCodigo]))
            {
                indiceCodigo++;

                if (indiceCodigo >= sequenciaSecreta.Length)
                {
                    secretoDesbloqueado = true;
                    indiceCodigo = 0;
                    SelecionarPlanta(plantaSecretaPrefab);
                    Debug.Log("Código secreto ativado!");
                }
            }
            else if (
                Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Se errar a seta, reinicia (ou começa do 1 se apertou Cima)
                indiceCodigo = Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0;
            }
        }
    }

    bool IsMouseSobreUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
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

        foreach (Rigidbody2D rb in previewAtual.GetComponentsInChildren<Rigidbody2D>())
        {
            Destroy(rb);
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

        Vector3 posicaoMouse = ObterPosicaoMouse();
        previewAtual.transform.position = posicaoMouse;

        bool ehRosa = (plantaSelecionada == rosaPrefab);
        AplicarOrientacao(previewAtual, ehRosa);

        bool podeColocar = PodePlantar(posicaoMouse) && PodeComprar();
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

        Vector3 posicaoMouse = ObterPosicaoMouse();

        if (PodePlantar(posicaoMouse))
        {
            if (GerenciadorDinheiro.instance == null || GerenciadorDinheiro.instance.TentarGastarDinheiro(ObterCustoAtual()))
            {
                GameObject novaPlanta = Instantiate(plantaSelecionada, posicaoMouse, Quaternion.identity);
                AplicarOrientacao(novaPlanta, plantaSelecionada == rosaPrefab);
            }
        }
    }

    int ObterCustoAtual()
    {
        if (plantaSelecionada == girassolPrefab) return custoGirassol;
        if (plantaSelecionada == rosaPrefab) return custoRosa;
        if (plantaSelecionada == plantaSecretaPrefab) return 0; // Custo zero
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

    Vector3 ObterPosicaoMouse()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;
        return posicaoMouse;
    }

    bool PodePlantar(Vector3 posicao)
    {
        // O quadrado secreto ignora bloqueios de terreno e pode ser colocado direto na estrada
        if (plantaSelecionada == plantaSecretaPrefab)
        {
            return true;
        }

        Collider2D[] colisores = Physics2D.OverlapCircleAll(posicao, raioOcupacao);

        foreach (Collider2D col in colisores)
        {
            if (col.CompareTag("Bloqueado"))
            {
                return false;
            }

            if (!col.isTrigger && !col.CompareTag("Inimigo"))
            {
                return false;
            }
        }

        return true;
    }
}
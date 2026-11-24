using System.Collections;
using UnityEngine;
using TMPro;

public class GerenciadorOndas : MonoBehaviour
{
    public static GerenciadorOndas instance;

    [Header("Prefabs das Abelhas")]
    public GameObject abelhaNormalPrefab;    // n
    public GameObject abelhaCorredoraPrefab; // r
    public GameObject abelhaTanquePrefab;    // t

    [Header("Caminho (Waypoints)")]
    public Transform[] pontos;

    [Header("Configuração das Ondas")]
    public string[] ondas = new string[]
    {
        "n n n n n",
        "r n n n n",
        "r n r n r",
        "n n t n n",
        "r t r t r"
    };

    public float tempoEntreAbelhas = 1.5f;
    public float tempoEntreOndas = 3f;

    [Header("Interface (UI)")]
    public GameObject botaoPronto;
    public TMP_Text textoOnda;

    [HideInInspector]
    public bool batalhaIniciada = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (textoOnda != null)
        {
            textoOnda.text = "Monte sua defesa!";
        }
    }

    // Função chamada ao clicar no botão PRONTO
    public void IniciarBatalha()
    {
        if (batalhaIniciada) return;

        batalhaIniciada = true;

        if (botaoPronto != null)
        {
            botaoPronto.SetActive(false);
        }

        StartCoroutine(ExecutarOndas());
    }

    IEnumerator ExecutarOndas()
    {
        for (int i = 0; i < ondas.Length; i++)
        {
            if (textoOnda != null)
            {
                textoOnda.text = "Onda: " + (i + 1) + "/" + ondas.Length;
            }

            // Separa as letras "n", "r", "t" da onda atual
            string[] sequencia = ondas[i].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < sequencia.Length; j++)
            {
                SpawnarAbelha(sequencia[j].ToLower());
                yield return new WaitForSeconds(tempoEntreAbelhas);
            }

            // Espera todas as abelhas da onda atual serem derrotadas antes de passar para a próxima
            while (GameObject.FindGameObjectsWithTag("Inimigo").Length > 0)
            {
                yield return null;
            }

            // Pausa rápida antes da próxima onda (exceto na última)
            if (i < ondas.Length - 1)
            {
                if (textoOnda != null)
                {
                    textoOnda.text = "Próxima onda em breve...";
                }
                yield return new WaitForSeconds(tempoEntreOndas);
            }
        }

        // Todas as 5 ondas foram concluídas!
        if (textoOnda != null)
        {
            textoOnda.text = "Vitória!";
        }
        Debug.Log("Vitória! Todas as 5 ondas concluídas.");
        // Na próxima etapa vamos chamar a Tela de Vitória aqui
    }

    void SpawnarAbelha(string tipo)
    {
        GameObject prefabEscolhido = abelhaNormalPrefab;

        if (tipo == "r")
        {
            prefabEscolhido = abelhaCorredoraPrefab;
        }
        else if (tipo == "t")
        {
            prefabEscolhido = abelhaTanquePrefab;
        }

        if (prefabEscolhido != null)
        {
            GameObject novaAbelha = Instantiate(prefabEscolhido, transform.position, Quaternion.identity);
            InimigoMovimento movimento = novaAbelha.GetComponent<InimigoMovimento>();
            if (movimento != null)
            {
                movimento.pontos = pontos;
            }
        }
    }
}
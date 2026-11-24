using UnityEngine;
using TMPro;

public class GerenciadorDinheiro : MonoBehaviour
{
    public static GerenciadorDinheiro instance;

    [Header("Configuração Inicial")]
    public int dinheiroInicial = 100;
    public TMP_Text textoDinheiro;

    private int dinheiroAtual;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dinheiroAtual = dinheiroInicial;
        AtualizarUI();
    }

    public void AdicionarDinheiro(int quantidade)
    {
        dinheiroAtual += quantidade;
        AtualizarUI();
    }

    public bool TemDinheiroSuficiente(int custo)
    {
        return dinheiroAtual >= custo;
    }

    public bool TentarGastarDinheiro(int custo)
    {
        if (dinheiroAtual >= custo)
        {
            dinheiroAtual -= custo;
            AtualizarUI();
            return true;
        }
        return false;
    }

    void AtualizarUI()
    {
        if (textoDinheiro != null)
        {
            textoDinheiro.text = "Dinheiro: " + dinheiroAtual;
        }
    }
}
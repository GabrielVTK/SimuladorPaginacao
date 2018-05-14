using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Contabilidade : MonoBehaviour {

    // Contabilidade Essencial
    public static int qtdProcessos; // Qtd de processos simulados
    public static int qtdProcessosSimultaneos; // Qtd de processos executando simultanêamente
    public static int qtdProcessosSemAguardar; // Qtd de processos que entraram sem ter que aguardar
    public static int qtdProcessosAguardar; // Qtd de processos que tiveram que aguardar ao menos 1 tempo

    // Contabilidade Extra
    public static int tempoGeralEspera; // Tempo geral de espera
    public static int tempoEsperaEsperaram; // Tempo geral dos que esperaram ao menos 1 tempo
    public static int qtdTempoFragmentacaoInterna; // Qtd de tempo que não alocou devido há fragmentação interna

    void Start() {

        GameObject.Find("Canvas/PanelConfiguracao/TextConfiguracao1").GetComponent<Text>().text += " " + Simulacao.tamMemoria;
        GameObject.Find("Canvas/PanelConfiguracao/TextConfiguracao2").GetComponent<Text>().text += " " + Simulacao.tamPagina;
        GameObject.Find("Canvas/PanelConfiguracao/TextConfiguracao3").GetComponent<Text>().text += " " + Simulacao.tamMemoria / Simulacao.tamPagina;

        GameObject.Find("Canvas/PanelEssencial/TextContabilidade1").GetComponent<Text>().text += " " + Contabilidade.qtdProcessos;
        GameObject.Find("Canvas/PanelEssencial/TextContabilidade2").GetComponent<Text>().text += " " + Contabilidade.qtdProcessosSimultaneos;
        GameObject.Find("Canvas/PanelEssencial/TextContabilidade3").GetComponent<Text>().text += " " + Contabilidade.qtdProcessosSemAguardar;
        GameObject.Find("Canvas/PanelEssencial/TextContabilidade4").GetComponent<Text>().text += " " + Contabilidade.qtdProcessosAguardar;
        
        GameObject.Find("Canvas/PanelExtra/TextContabilidade1").GetComponent<Text>().text += " " + ((float)Contabilidade.tempoGeralEspera / (float)Contabilidade.qtdProcessos).ToString("n2");
        GameObject.Find("Canvas/PanelExtra/TextContabilidade2").GetComponent<Text>().text += " " + ((float)Contabilidade.tempoEsperaEsperaram / (float)Contabilidade.qtdProcessosAguardar).ToString("n2");
        GameObject.Find("Canvas/PanelExtra/TextContabilidade3").GetComponent<Text>().text += " " + Contabilidade.qtdTempoFragmentacaoInterna;

    }

    void Update() {}

    public void Voltar() {
        SceneManager.LoadScene("inicio");
    }

}

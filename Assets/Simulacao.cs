using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulacao : MonoBehaviour {

    public static string path;
    public int totalPaginas;
    public static int tamPagina;
    public static int tamMemoria;

    private List<Processo> processos;
    private List<Processo> processosAlocados;
    private List<Processo> processosDesalocados;
    private List<Pagina> mapaMemoria;

    private int contador;
    private int unidadeTempo = 100;
    private int velocidade = 1;
    private int tempo;

    void Start () {

        this.processos = new List<Processo>();
        this.processosAlocados = new List<Processo>();
        this.processosDesalocados = new List<Processo>();
        this.mapaMemoria = new List<Pagina>();

        this.totalPaginas = Simulacao.tamMemoria / Simulacao.tamPagina;

        this.tempo = 0;

        GameObject.Find("Canvas/ImageMapa/SliderVelocidade").GetComponent<Slider>().maxValue = this.unidadeTempo;

        //string[] lines = System.IO.File.ReadAllLines(simulacao.path);
        string[] lines = System.IO.File.ReadAllLines("C:/Users/Gabriel Vinicius/Documents/SimuladorPaginacao/processos.txt");

        string[] dados;

        foreach (string line in lines) {
            dados = line.Split(',');

            if(dados[0] != "ID") {
                this.processos.Add(
                    new Processo(int.Parse(dados[0]), int.Parse(dados[1]), int.Parse(dados[2]), int.Parse(dados[3]))
                );
            }
            
        }
        
        this.ListarProcessos();
        this.DesenharMapa();
    }
	
	void Update () {

        if(this.contador >= this.unidadeTempo && (this.processos.Count > 0 || this.processosAlocados.Count > 0)) {
            this.tempo++;

            foreach (Processo processo in this.processosAlocados) {
                if (processo.Contador == 0) { 
                    DesalocarProcesso(processo);
                }
            }

            foreach (Processo processo in this.processos) {
                if (processo.TempoEntrada <= this.tempo) {
                    if (!this.AlocarProcesso(processo)) {
                        break;
                    }
                } else {
                    break;
                }
            }
            
            foreach(Processo processo in this.processosAlocados) {
                this.processos.Remove(processo);
                
                if(processo.Contador == 0) {
                    DesalocarProcesso(processo);
                } else {
                    processo.Contador--;
                }
            }
            
            foreach(Processo processo in this.processosDesalocados) {
                this.processosAlocados.Remove(processo);
            }

            this.processosDesalocados.Clear();

            this.contador -= unidadeTempo;
            GameObject.Find("Canvas/ImageMapa/TextTempo").GetComponent<Text>().text = "Tempo: " + this.tempo;
        } else {
            this.contador += this.velocidade;
        }
		
	}

    public bool AlocarProcesso(Processo processo) {

        Debug.Log("Tentando alocar P" + processo.Id);
        
        int numPaginas = (int)Mathf.Ceil((float)processo.Bytes / (float)Simulacao.tamPagina);
        
        List<Pagina> paginas = new List<Pagina>();
        
        foreach(Pagina pagina in this.mapaMemoria) {
            if(numPaginas > 0 && !pagina.Ocupado) {
                paginas.Add(pagina);
                numPaginas--;

                if(numPaginas == 0) { break; }
            }
        }

        if(numPaginas == 0) {

            Debug.Log("Processo P" + processo.Id + " alocado!");

            int bytes = processo.Bytes;

            foreach(Pagina pagina in paginas) {
                pagina.Ocupado = true;
                pagina.Processo = processo;
                processo.Paginas.Add(pagina);

                if(bytes >= pagina.Bytes) {
                    pagina.BytesUsados = pagina.Bytes;
                    bytes -= pagina.Bytes;
                } else {
                    pagina.BytesUsados = bytes;
                }

                pagina.Bloco.transform.Find("text").GetComponent<Text>().text = "P" + processo.Id + "( " + pagina.BytesUsados + " / " + pagina.Bytes + " )";
            }

            processo.Paginas = paginas;
            this.processosAlocados.Add(processo);
            Destroy(GameObject.Find("Canvas/SVProcessos/Viewport/Content/processo" + processo.Id).gameObject);

            GameObject text = new GameObject("processo" + processo.Id, typeof(Text));
            text.transform.SetParent(GameObject.Find("Canvas/SVSaida/Viewport/Content").transform);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);
            text.GetComponent<Text>().text = this.tempo + ", " + processo.Id + ", " + processo.Paginas[0].Id + ", ENTROU";
            text.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 10);
            text.GetComponent<Text>().fontSize = 10;
            text.GetComponent<Text>().color = Color.black;

            return true;
        }

        Debug.Log("Processo P" + processo.Id + " não foi alocado!");
        return false;
    }

    public void DesalocarProcesso(Processo processo) {

        foreach (Pagina pagina in processo.Paginas) {
            pagina.Ocupado = false;
            pagina.BytesUsados = 0;
            pagina.Processo = null;
            pagina.Bloco.transform.Find("text").GetComponent<Text>().text = "";
        }
        
        GameObject text = new GameObject("processo" + processo.Id, typeof(Text));
        text.transform.SetParent(GameObject.Find("Canvas/SVSaida/Viewport/Content").transform);
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);
        text.GetComponent<Text>().text = this.tempo + ", " + processo.Id + ", " + processo.Paginas[0].Id + ", SAIU";
        text.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 10);
        text.GetComponent<Text>().fontSize = 10;
        text.GetComponent<Text>().color = Color.black;

        this.processosDesalocados.Add(processo);
    }

    public void ListarProcessos() {
        
        GameObject content = GameObject.Find("Canvas/SVProcessos/Viewport/Content");
        
        foreach (Processo processo in this.processos) {
            
            GameObject text = new GameObject("processo" + processo.Id, typeof(Text));
            text.transform.SetParent(content.transform);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);
            text.GetComponent<Text>().text = "P" + processo.Id + ", " + processo.TempoEntrada + ", " + processo.TempoSaida + ", " + processo.Bytes;
            text.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 10);
            text.GetComponent<Text>().fontSize = 10;
            text.GetComponent<Text>().color = Color.black;
        }

    }

    public void DesenharMapa() {

        int indice = 0;

        float margem = 1;
        float blocoAltura = this.GetAlturaBloco();
        float blocoLargura = this.GetLarguraBloco();

        int linhas = (int)(500 / blocoAltura);
        int colunas = (int)(660 / blocoLargura);

        // Esta subtração da margem é feita após a divisão de linhas e colunas
        // para não interverir no tamanho da tela
        blocoAltura -= 2 * margem;
        blocoLargura -= 2 * margem;

        float posX, posY;

        for (int j = 0; j < colunas; j++) {

            if (j > 0 && j < colunas) {
                posX = margem + (blocoLargura + 2 * margem) * j;
            } else {
                posX = margem + (blocoLargura + margem) * j;
            }

            for (int i = 0; i < linhas; i++) {

                if (i > 0 && i < linhas) {
                    posY = -margem - (blocoAltura + 2 * margem) * i;
                } else {
                    posY = -margem - (blocoAltura + margem) * i;
                }

                GameObject bloco = new GameObject("bloco_" + i + "_" + j, typeof(Image));
                bloco.transform.SetParent(GameObject.Find("ImageMapa").transform);
                bloco.GetComponent<Image>().color = Color.cyan;
                bloco.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                bloco.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                bloco.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                bloco.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posX, posY, 0.0f);
                bloco.GetComponent<RectTransform>().sizeDelta = new Vector2(blocoLargura, blocoAltura);

                GameObject text = new GameObject("text", typeof(Text));
                text.transform.SetParent(bloco.transform);
                text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                text.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                text.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 10);
                text.GetComponent<Text>().fontSize = 10;
                text.GetComponent<Text>().color = Color.black;
                text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                
                this.mapaMemoria.Add(new Pagina(indice, Simulacao.tamPagina, bloco));
                indice++;

                if(indice == this.totalPaginas) {
                    break;
                }
            }

            if (indice == this.totalPaginas) {
                break;
            }
        }

    }

    public void MudarVelocidade() {
        this.velocidade = (int)GameObject.Find("SliderVelocidade").GetComponent<Slider>().value;
        GameObject.Find("TextVelocidade").GetComponent<Text>().text = "Velocidade: " + this.velocidade;
    }

    public float GetAlturaBloco() {
        float altura = 0;

        switch(this.totalPaginas) {
            case 256:
                altura = 20.83f;
                break;
            case 128:
                altura = 41.66f;
                break;
            case 64:
                altura = 62.5f;
                break;
            case 32:
                altura = 62.5f;
                break;
            case 16:
                altura = 125;
                break;
            case 8:
                altura = 125;
                break;
            case 4:
                altura = 250;
                break;
        }
        
        return altura;
    }

    public float GetLarguraBloco() {
        float largura = 0;

        switch (this.totalPaginas) {
            case 256:
                largura = 60;
                break;
            case 128:
                largura = 60;
                break;
            case 64:
                largura  = 82.5f;
                break;
            case 32:
                largura = 165;
                break;
            case 16:
                largura = 165;
                break;
            case 8:
                largura = 330;
                break;
            case 4:
                largura = 330;
                break;
        }

        return largura;
    }

}

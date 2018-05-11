using SFB;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inicio : MonoBehaviour {
    
	void Start () {}
	
	void Update () {}

    public void OpenFile() {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Selecione uma lista de processos", "", "txt", false);
        Simulacao.path = paths[0];
    }

    public void Simulate() {
        
        if (Simulacao.path != "" && Simulacao.path != null) {

            switch(GameObject.Find("Canvas/dropMemoria").GetComponent<Dropdown>().value) {
                case 0:
                    Simulacao.tamMemoria = 512;
                    break;
                case 1:
                    Simulacao.tamMemoria = 1024;
                    break;
                case 2:
                    Simulacao.tamMemoria = 2048;
                    break;
            }

            switch(GameObject.Find("Canvas/dropPagina").GetComponent<Dropdown>().value) {
                case 0:
                    Simulacao.tamPagina = 8;
                    break;
                case 1:
                    Simulacao.tamPagina = 16;
                    break;
                case 2:
                    Simulacao.tamPagina = 32;
                    break;
                case 3:
                    Simulacao.tamPagina = 64;
                    break;
                case 4:
                    Simulacao.tamPagina = 128;
                    break;
            }
            
            SceneManager.LoadScene("simulacao");
        }

    }

}

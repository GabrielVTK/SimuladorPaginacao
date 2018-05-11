using UnityEngine;

public class Pagina {

    private int id;
    private int bytes;
    private int bytesUsados;
    private GameObject bloco;
    private Processo processo;
    private bool ocupado;


    public Pagina(int id, int bytes, GameObject bloco) {
        this.id = id;
        this.bytes = bytes;
        this.bytesUsados = 0;
        this.bloco = bloco;
        this.ocupado = false;
    }

    public int Id {
        get {
            return id;
        }
    }

    public int Bytes {
        get {
            return bytes;
        }

        set {
            bytes = value;
        }
    }

    public int BytesUsados {
        get {
            return bytesUsados;
        }

        set {
            bytesUsados = value;
        }
    }

    public GameObject Bloco {
        get {
            return bloco;
        }

        set {
            bloco = value;
        }
    }

    public bool Ocupado {
        get {
            return ocupado;
        }

        set {
            ocupado = value;
        }
    }

    public Processo Processo {
        get {
            return processo;
        }

        set {
            processo = value;
        }
    }
}

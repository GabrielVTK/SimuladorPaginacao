using System.Collections.Generic;

public class Processo {

    private int id;
    private int tempoEntrada;
    private int tempoSaida;
    private int bytes;
    private int contador;

    private List<Pagina> paginas;

    public Processo(int id, int tempoEntrada, int tempoSaida, int bytes) {
        this.id = id;
        this.tempoEntrada = tempoEntrada;
        this.tempoSaida = tempoSaida;
        this.bytes = bytes;
        this.contador = this.TempoSaida - this.TempoEntrada;
        this.paginas = new List<Pagina>();
    }

    public int Id {
        get {
            return id;
        }
    }

    public int TempoEntrada {
        get {
            return tempoEntrada;
        }

        set {
            tempoEntrada = value;
        }
    }

    public int TempoSaida {
        get {
            return tempoSaida;
        }

        set {
            tempoSaida = value;
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

    public List<Pagina> Paginas {
        get {
            return paginas;
        }

        set {
            paginas = value;
        }
    }

    public int Contador {
        get {
            return contador;
        }

        set {
            contador = value;
        }
    }
}

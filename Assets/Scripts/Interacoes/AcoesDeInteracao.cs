public abstract class AcoesDeInteracao
{
    public abstract float Duracao { get; }

    public abstract bool PodeInteragir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador);
    public abstract void Interagir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador);
}
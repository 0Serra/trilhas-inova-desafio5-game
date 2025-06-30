using UnityEngine;

public class AcaoColetarAgua : AcoesDeInteracao
{
    public override float Duracao => 1.5f;

    public override bool PodeInteragir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        return celulaAlvo.tipo == TipoDeCelula.Agua && !estadosDoJogador.TemAgua;
    }

    public override void Interagir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        estadosDoJogador.ColetarAgua();

        if (celulaAlvo.objeto != null)
        {
            AudioSource audio = celulaAlvo.objeto.GetComponent<AudioSource>();

            if (audio != null && audio.clip != null)
            {
                audio.loop = false;
                audio.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource ou AudioClip não encontrado no prefab de água.");
            }
        }
    }
}

using UnityEngine;

public class SpritesArvores : MonoBehaviour
{
    public Sprite spriteCinzas;
    public RuntimeAnimatorController animacaoFogoController;

    public AudioClip somDeQueimando;
    public AudioClip somDeApagarFogo;

    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TocarSom(AudioClip clip, bool loop = false)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop(); 
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    public void PararSom()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}

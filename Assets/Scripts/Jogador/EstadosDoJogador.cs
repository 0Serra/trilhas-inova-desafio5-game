using UnityEngine;

public class EstadosDoJogador : MonoBehaviour
{
    private bool temAgua = false;
    public bool TemAgua => temAgua;

    public void ColetarAgua()
    {
        temAgua = true;
        Debug.Log("COLETOU ÁGUA!");
    }

    public void UsarAgua()
    {
        temAgua = false;
        Debug.Log("USOU ÁGUA!");
    }
}

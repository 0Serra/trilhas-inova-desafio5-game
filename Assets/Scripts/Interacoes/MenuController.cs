using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    
    public void NovoJogo()
    {
        SceneManager.LoadScene("GameScene");
    }

    
    /*public void Tutorial()
    {
        
        Debug.Log("");
    }

    public void Recordes()
    {}
        Debug.Log("");
    }

    public void Configuracoes()
    {
        Debug.Log("");
    }

    
    public void Sair()
    {
        Debug.Log("Sair do jogo.");
        Application.Quit();
    }
    */
}

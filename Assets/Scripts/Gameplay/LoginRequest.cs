using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class LoginRequest : MonoBehaviour
{

    //Informações do usuário
    [Header("Credenciais de Teste")]
    public string username = "usuario_teste";
    public string password = "senha123";

    //Endereço da API do back end
    private string apiUrl = "<https://suaapi.com/auth/login>";

    public System.Action<string> OnLoginMensagem;
    public System.Action OnLoginSucesso;

    //Função que deve ser chamada na hora de fazer login
    public void FazerLogin()
    {
        StartCoroutine(EnviarLogin(username, password));
    }

    //Função que checa se o login do usuario está devidamente cadastrado no banco de dados
    private IEnumerator EnviarLogin(string usuario, string senha)
    {
        LoginData loginData = new LoginData { username = usuario, password = senha };
        string jsonData = JsonUtility.ToJson(loginData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Erro na requisição: " + request.error);
            OnLoginMensagem?.Invoke("Erro de conexão: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

            if (response.success)
            {
                Debug.Log("Login realizado com sucesso: " + response.message);
                OnLoginMensagem?.Invoke("✅ Login realizado com sucesso!");
                OnLoginSucesso?.Invoke();
            }
            else
            {
                Debug.Log("Falha no login: " + response.message);
                OnLoginMensagem?.Invoke("❌ Falha no login: " + response.message);
            }
        }

    }
}

//Informações enviadas para o backend
[System.Serializable]
public class LoginData
{
    public string username;
    public string password;
}

//Informações recebidas do backend
[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
}

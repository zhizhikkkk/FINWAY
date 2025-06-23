using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AuthService
{
    private const string LoginUrl = "https://yourserver.com/api/login";
    private const string RegisterUrl = "https://yourserver.com/api/register";

    public async Task<bool> Login(string username, string password)
    {
        return await SendLoginRequest(username, password);
    }

    public async Task<bool> Register(string username, string password, string email)
    {
        return await SendRegisterRequest(username, password,email);
    }

    private async Task<bool> SendLoginRequest(string username, string password)
    {
        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(LoginUrl, form))
        {
            try
            {
                await www.SendWebRequestAsync();

                Debug.Log("Login successful: " + www.downloadHandler.text);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Login error: " + ex.Message);
                return false;
            }
        }
    }

    private async Task<bool> SendRegisterRequest(string username, string password,string email)
    {
        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email);

        using (UnityWebRequest www = UnityWebRequest.Post(RegisterUrl, form))
        {
            try
            {
                await www.SendWebRequestAsync();

                Debug.Log("Registration successful: " + www.downloadHandler.text);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Registration error: " + ex.Message);
                return false;
            }
        }
    }
}

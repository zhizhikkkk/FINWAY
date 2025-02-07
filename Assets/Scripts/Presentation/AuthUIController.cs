using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UniRx;
using Zenject;
using R3;
using System;
using UnityEngine.SceneManagement;

public class AuthUIController : MonoBehaviour
{
    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public Button loginButton;
    public Button switchToRegisterButton;

    [Header("Registration Panel")]
    public GameObject registrationPanel;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField emailInput;
    public Button registerButton;
    public Button switchToLoginButton;

    [Inject] private AuthService authService;

    private void Start()
    {
        // Начальная настройка
        loginPanel.SetActive(true);
        registrationPanel.SetActive(false);

        // Подписка на события
        loginButton.onClick.AddListener(HandleLogin);
        switchToRegisterButton.onClick.AddListener(ShowRegistrationPanel);
        registerButton.onClick.AddListener(HandleRegister);
        switchToLoginButton.onClick.AddListener(ShowLoginPanel);
    }

    private void ShowRegistrationPanel()
    {
        //TODO: Make dowteen animation
        registrationPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    private void ShowLoginPanel()
    {
        //TODO: Make dowteen animation
        registrationPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    private async void HandleLogin()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;

        try
        {
            bool success = await authService.Login(username, password);
            if (success)
            {
                Debug.Log("Login successful");
                SceneManager.LoadScene("Home");
            }
            else
            {
                Debug.Log("Login failed");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during login: " + ex.Message);
        }
    }



    private async void HandleRegister()
    {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        string email = emailInput.text;

        if (password != confirmPassword)
        {
            Debug.Log("Passwords do not match");
            return;
        }

        try
        {
            bool success = await authService.Register(username, password,email);
            if (success)
            {
                Debug.Log("Registration successful");
                ShowLoginPanel();
            }
            else
            {
                Debug.Log("Registration failed");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during registration: " + ex.Message);
        }
    }

}

using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private RectTransform _rectTransform;
    private InputService _inputService;

    public float Speed = 300f; 

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();  
    }

    private void Update()
    {
        Vector2 movement = _inputService.GetMovementInput();
        _rectTransform.anchoredPosition += movement * Speed * Time.deltaTime;
    }
}

using UnityEngine;

public class TextureOffsetScroll : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeedX, _scrollSpeedY;

    private float _originalXSpeed;
    private float _offsetX;

    private Renderer _renderer;
    private readonly string MAINTEXT = "_MainTex";

    private CameraFollowPlayer _camFollow;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _camFollow = FindObjectOfType<CameraFollowPlayer>();
        _originalXSpeed = _scrollSpeedX;
    }

    void Update()
    {
        ScrollTextureAccordingToCamSpeed();
    }

    private void ScrollTextureAccordingToCamSpeed()
    {
        _scrollSpeedX = _originalXSpeed * _camFollow.Speed;

        _offsetX += Time.deltaTime * _scrollSpeedX;

        _renderer.material.SetTextureOffset(MAINTEXT, new Vector2(_offsetX, _scrollSpeedY * Time.time));
    }
}

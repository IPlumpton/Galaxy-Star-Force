using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shieldStateRenderer;

    private void Start()
    {
        _shieldStateRenderer = GetComponent<SpriteRenderer>();
        SetShieldColor(3);
    }

    public void SetShieldColor(int shieldState)
    {
        switch (shieldState)
        {
            case 3:
                _shieldStateRenderer.color = new Color(1, 1, 1, 1);
                break;
            case 2:
                _shieldStateRenderer.color = new Color(1, 1, 1, 0.5f);
                break;
            case 1:
                _shieldStateRenderer.color = new Color(1, 1, 1, 0.25f);
                break;
            case 0:
                _shieldStateRenderer.color = new Color(1, 1, 1, 0);
                break;
            default:
                break;
        }
    }
}

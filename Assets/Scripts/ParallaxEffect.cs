using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private float textureUnitSizeX;
    private Vector3 lastCameraPosition;
    private void Start()
    {
        lastCameraPosition = cam.position;   
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }
    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cam.position;

        if (Mathf.Abs(cam.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cam.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cam.position.x + offsetPositionX, transform.position.y);

        }
    }
}

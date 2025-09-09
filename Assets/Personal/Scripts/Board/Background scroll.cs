using UnityEngine;

public class Backgroundscroll : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Vector2 speed = new Vector2(0.5f, 0f);

    void Update()
    {
        Vector2 offset = material.mainTextureOffset;
        offset += speed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}

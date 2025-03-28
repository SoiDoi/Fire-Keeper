using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    
    public void DrawTexture(Texture2D texture)
    {
        

        textureRenderer.sharedMaterial.mainTexture = texture; // Help Noise initialized in Unity's editor
        textureRenderer.transform.localScale = new Vector3(texture.width,1,texture.height);
    }
}

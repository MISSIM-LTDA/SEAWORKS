using UnityEngine;

public class DrawOnTexture : MonoBehaviour
{
    public Material materialToDrawOn;
    public Color brushColor = Color.white;
    public float brushSize = 1f;

    private Texture2D textureToDrawOn;
    private Vector2 lastMousePosition;

    void Start()
    {
        // Get the texture from the material
        textureToDrawOn = (Texture2D)materialToDrawOn.GetTexture("_MainTex");
    }

    void Update()
    {
        // Check if the left mouse button is down
        if (Input.GetMouseButton(0))
        {
            // Get the current mouse position
            Vector2 mousePosition = Input.mousePosition;

            // Check if the mouse position has changed
            if (mousePosition != lastMousePosition)
            {
                // Convert the mouse position to texture coordinates
                Vector2 textureCoords = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);

                // Calculate the pixel coordinates based on the texture size
                int pixelX = (int)(textureCoords.x * textureToDrawOn.width);
                int pixelY = (int)(textureCoords.y * textureToDrawOn.height);

                // Draw a circle at the pixel coordinates
                DrawCircle(pixelX, pixelY);

                // Apply the changes to the material
                materialToDrawOn.SetTexture("_MainTex", textureToDrawOn);
            }

            // Update the last mouse position
            lastMousePosition = mousePosition;
        }
    }

    void DrawCircle(int x, int y)
    {
        // Calculate the radius of the brush in pixels
        int radius = Mathf.RoundToInt(brushSize * textureToDrawOn.width / 2f);

        // Loop through all the pixels in the circle
        for (int i = x - radius; i <= x + radius; i++)
        {
            for (int j = y - radius; j <= y + radius; j++)
            {
                // Check if the pixel is within the bounds of the texture
                if (i >= 0 && i < textureToDrawOn.width && j >= 0 && j < textureToDrawOn.height)
                {
                    // Calculate the distance from the center of the circle to the current pixel
                    float distance = Vector2.Distance(new Vector2(i, j), new Vector2(x, y));

                    // Check if the pixel is within the brush radius
                    if (distance <= radius)
                    {
                        // Set the pixel color to the brush color
                        textureToDrawOn.SetPixel(i, j, brushColor);
                    }
                }
            }
        }

        // Apply the changes to the texture
        textureToDrawOn.Apply();
    }
}

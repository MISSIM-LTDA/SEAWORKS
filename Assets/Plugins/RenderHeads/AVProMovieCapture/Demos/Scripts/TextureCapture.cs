using UnityEngine;

namespace RenderHeads.Media.AVProMovieCapture.Demos
{
    public class TextureCapture : MonoBehaviour
    {
        private CaptureFromTexture movieCapture;
        [SerializeField] private RenderTexture texture;
        private void Start()
        {
            movieCapture = GetComponent<CaptureFromTexture>();

            if (movieCapture != null){
                movieCapture.SetSourceTexture(texture);
            }
        }
    }
}
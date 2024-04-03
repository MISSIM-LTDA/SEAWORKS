using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class DisplayCapture : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("user32.dll")]
    private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("user32.dll")]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("user32.dll")]
    private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern int DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern int DeleteObject(IntPtr hObject);

    // Adjust the width and height according to your display resolution
    private const int DisplayWidth = 1920;
    private const int DisplayHeight = 1080;

    public Texture2D capturedTexture;

    void Update()
    {
        IntPtr hwnd = GetDesktopWindow();
        IntPtr hdcSrc = GetDC(hwnd);
        IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
        IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, DisplayWidth, DisplayHeight);
        IntPtr hOld = SelectObject(hdcDest, hBitmap);

        // Capture the display to the texture
        BitBlt(hdcDest, 0, 0, DisplayWidth, DisplayHeight, hdcSrc, 0, 0, 0x00CC0020);

        // Create a new texture and load the captured data into it
        capturedTexture = new Texture2D(DisplayWidth, DisplayHeight, TextureFormat.RGB24, false);
        capturedTexture.Apply();

        // Read the captured pixels and apply them to the texture
        capturedTexture.ReadPixels(new Rect(0, 0, DisplayWidth, DisplayHeight), 0, 0);
        capturedTexture.Apply();

        // Cleanup
        SelectObject(hdcDest, hOld);
        DeleteObject(hBitmap);
        DeleteDC(hdcDest);
        ReleaseDC(hwnd, hdcSrc);
    }
}

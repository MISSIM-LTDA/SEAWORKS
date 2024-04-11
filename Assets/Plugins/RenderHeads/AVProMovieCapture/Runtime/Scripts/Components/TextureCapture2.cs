using UnityEngine;

//-----------------------------------------------------------------------------
// Copyright 2012-2022 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProMovieCapture.Demos
{
	/// <summary>
	/// Animates a procedural texture effect driven by a shader
	/// </summary>
	public class TextureCapture2 : MonoBehaviour
	{
		
		[SerializeField] CaptureFromTexture _movieCapture = null;
		public RenderTexture _texture;

		private void Start()
		{
			
			if (_movieCapture)
			{
				_movieCapture.SetSourceTexture(_texture);
			}
		}

		
	}
}
Shader "Hole/Hole" {
 
	SubShader {
		// Render the mask after regular geometry, but before masked geometry and transparent things.
 
		Tags {"Queue" = "Geometry-2" }
 

		Lighting Off
		//ZTest LEqual
		ZTest Always
		// Don't draw in the RGBA channels; just the depth buffer

		ZWrite On
		ColorMask 0
 
		// Do nothing specific in the pass:
 
		Pass {}
	}
}

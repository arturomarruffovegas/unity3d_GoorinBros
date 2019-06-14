Shader "GG"
{
	SubShader
	{
		Tags {"Queue" = "Geometry-1" }
		Lighting Off
		Pass
		{
			ZWrite On
			ZTest LEqual
			ColorMask 0
		}
	}
}
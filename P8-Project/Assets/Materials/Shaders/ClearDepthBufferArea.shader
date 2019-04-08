/*
This code is a simple way to clear the depth buffer, taken from: https://en.wikibooks.org/wiki/Cg_Programming/Unity/Portals
*/

Shader "Custom/ClearDepthBufferArea"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+200" }

		Pass
		{
			ZTest Always // always pass depth test (nothing occludes this material)   <- Main part of this shader     
			Cull Off // turn off backface culling

			Stencil {
			Ref 3
			Comp Equal // only pass stencil test if stencil value equals 1
			Fail Keep // do not change stencil value if stencil test fails
			ZFail Keep // do not change stencil value if stencil test passes but depth test fails
			Pass Keep // keep stencil value if stencil test passes
			}

		 CGPROGRAM
		 #pragma vertex vert
		 #pragma fragment frag

		 float4 vert(float4 vertex: POSITION) : SV_POSITION
		 {
			return UnityObjectToClipPos(vertex);
		 }

		 fixed4 frag() : SV_Target
		 {
			return float4(0.0, 0.0, 0.0, 0.0);
		 }
		 ENDCG
	  }
	}
}
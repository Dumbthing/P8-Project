/*
This code is a simple way to clear the depth buffer, taken from: https://en.wikibooks.org/wiki/Cg_Programming/Unity/Portals
It is also combined with Unity's Skybox shader
*/

Shader "Stencils/ClearNextStencilDepthBuffer"
{
	Properties{
		_Cube("Environment Map", Cube) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+200" }

		Pass
		{
			ZTest Always // always pass depth test (nothing occludes this material) 
			Cull Off // turn off backface culling

			Stencil {
			Ref 2
			Comp Equal // only pass stencil test if stencil value equals 1
			Fail Keep // do not change stencil value if stencil test fails
			ZFail Keep // do not change stencil value if stencil test passes but depth test fails
			Pass Keep // keep stencil value if stencil test passes
			}
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			// User-specified uniforms
			samplerCUBE _Cube;

			struct vertexInput {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
			};

			struct vertexOutput {
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
			};

		//vertexOutput vert(vertexInput input)
		//{
		//   vertexOutput output;
		//   output.vertex = UnityObjectToClipPos(input.vertex);
		//   output.texcoord = input.texcoord;
		//   return output;
		//}

		float4 vert(float4 vertex: POSITION) : SV_POSITION
		{
		   return UnityObjectToClipPos(vertex);
		}

		//fixed4 frag(vertexOutput input) : COLOR
		//{
		//   return texCUBE(_Cube, input.texcoord);
		//}

		 fixed4 frag() : SV_Target
		 {
			return float4(0.0, 0.0, 0.0, 0.0);
		 }
		 ENDCG
	  }
	}
}
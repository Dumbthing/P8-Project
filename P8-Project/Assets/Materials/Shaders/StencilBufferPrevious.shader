Shader "Stencils/Materials/StencilBufferPrevious" {
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+300" }
			Stencil
			{
				Ref 1
				Comp equal
				Pass keep
				Fail keep
			}

		CGPROGRAM
		#pragma surface surf Lambert noshadow

		sampler2D _MainTex;
		fixed4 _Color;
		float4x4 _WorldToPortal;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			// Discard geometry based on z axis proximity, but not when camera is close enough to the portal
			if (mul(_WorldToPortal, float4(_WorldSpaceCameraPos, 1.0)).z > 0.2) {
				if (mul(_WorldToPortal, float4(IN.worldPos, 1.0)).z > 0.21)
					discard;
			}
			else if (mul(_WorldToPortal, float4(_WorldSpaceCameraPos, 1.0)).z < -0.2) {
				if (mul(_WorldToPortal, float4(IN.worldPos, 1.0)).z < -0.21)
					discard;
			}
		}
		ENDCG
		}
	Fallback Off
}


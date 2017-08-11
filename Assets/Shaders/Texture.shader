Shader "Test/Texture" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Basic

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		
		}


		half4 LightingBasic(SurfaceOutput s, half3 lightDir, half attenuation)
		{
			half4 c;
			c.rgb = s.Albedo  * attenuation; // color of surface * color of light * amount of light hitting surface * attenuation
			c.a = s.Alpha;
			return c;
		}

		ENDCG
	}
	FallBack "Diffuse"
}

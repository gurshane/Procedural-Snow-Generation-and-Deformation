Shader "Project/Displacement" 
{
	//Takes in a texture for a surface and a displacement map that is used to displace each vertex on the relevant surface
	Properties
	{
	  _MainTex("Main Tex", 2D) = "white" {}
	  _DispTex("Displacement Map", 2D) = "black" {}
	  _DispAmount("Disp Amount", Range(0, 1.0)) = 0.3
	}
	
	SubShader
	{
		
		Tags
	    { 
		  "RenderType" = "Opaque" 
	    }
		LOD 300

		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:disp
		#pragma target 4.6

		struct appdata 
	    {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		sampler2D _DispTex;
		float _DispAmount;

		//Displaces the inputted vertex
		void disp(inout appdata v)
		{
			//Move the vertex along the result of its normal mulitplied with either a 0 or a 1 (depending on whether or not the corresponding displacement map pixel is black or white, respectively)
			//and some base displacement amount
			v.vertex.xyz += v.normal * ((tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r) * _DispAmount);
		}


		sampler2D _MainTex;
		struct Input 
		{
			float2 uv_MainTex;
		};

		//Surface shader
		void surf(Input IN, inout SurfaceOutput o) 
		{
			//Get colour of the current vertex from the inputted texture and colour the vertex as such
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}

		ENDCG
	}
		FallBack "Diffuse"
}
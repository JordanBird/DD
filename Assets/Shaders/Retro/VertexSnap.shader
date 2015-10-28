Shader "Custom/Retro/VertexSnap" {
	Properties {
		// I've left the standard Unity stuff in here in case we don't want to go with a crappy flat shaded mess. If we want that, ask and I'll do it!
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_VertexPrecision ("Vertex Precision", Float) = 100.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma vertex vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _VertexPrecision;

		// Reduces precision of a value. Swap out (int) for round for a less efficient but maybe better effect.
		half reducePrecision(half value, half precision) {
			return (int)(value * precision) / precision;
		}

		void vert(inout appdata_full v) {
			// Snap position of vertices by reducing their precision.
			half3 pos = v.vertex.xyz - _WorldSpaceCameraPos.xyz;
			pos = mul(pos, UNITY_MATRIX_V);
			pos.x = reducePrecision(pos.x, _VertexPrecision);
			pos.y = reducePrecision(pos.y, _VertexPrecision);
			pos.z = reducePrecision(pos.z, _VertexPrecision);
			pos = mul(UNITY_MATRIX_V, pos);
			v.vertex.xyz = pos + _WorldSpaceCameraPos.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

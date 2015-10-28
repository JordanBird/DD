Shader "Custom/Retro/ColorCrush"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorPrecision ("Color Precision", Float) = 40.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			half _ColorPrecision;

			half reducePrecision(half value, half precision) {
				return round(value * precision) / precision;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.r = reducePrecision(col.r, _ColorPrecision);
				col.g = reducePrecision(col.g, _ColorPrecision);
				col.b = reducePrecision(col.b, _ColorPrecision);
				return col;
			}
			ENDCG
		}
	}
}

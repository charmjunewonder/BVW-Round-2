Shader "Custom/CutOutLights" {
		Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_CutOutTex ("Cutout Base (RGB) Trans (A)", 2D) = "white" {}
		_CutOutThres ("CutOutThreshold", Range (0.0, 1.0)) = 0.1
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
				};

				sampler2D _MainTex;
				sampler2D _CutOutTex;
				float4 _MainTex_ST;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				
				float _CutOutThres;
				
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					fixed4 cutoutcol = tex2D(_CutOutTex,i.texcoord);
					if(cutoutcol.r > _CutOutThres && cutoutcol.g > _CutOutThres && cutoutcol.b > _CutOutThres)
					{
						//float c = (cutoutcol.r + cutoutcol.g + cutoutcol.b)/3.0;
						discard;
					}
					return col;
					
				}
			ENDCG
		}
	}

    FallBack "Diffuse"
}

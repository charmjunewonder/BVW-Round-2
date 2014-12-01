// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/lifebar" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100

	Blend SrcAlpha OneMinusSrcAlpha
	//ZTest Off
	Lighting Off

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
			float4 _MainTex_ST;
			fixed _Cutoff;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				float cutoff = col.a - _Cutoff;
				float texstep = 0.01;
				if(cutoff < 0.005)
					discard;
				else
				{
					float alphaup = max(0,tex2D(_MainTex, i.texcoord + float2(0,texstep)).a - _Cutoff) > 0 ? 1 : 0;
					float alphadown = max(0,tex2D(_MainTex, i.texcoord + float2(0,-texstep)).a - _Cutoff) > 0 ? 1 : 0;
					float alphaleft = max(0,tex2D(_MainTex, i.texcoord + float2(-texstep,0)).a - _Cutoff) > 0 ? 1 : 0;
					float alpharight = max(0,tex2D(_MainTex, i.texcoord + float2(texstep,0)).a - _Cutoff) > 0 ? 1 : 0;
					
					float alphaleftup = max(0,tex2D(_MainTex, i.texcoord + float2(-texstep,texstep)).a - _Cutoff) > 0 ? 1 : 0;
					float alphaleftdown = max(0,tex2D(_MainTex, i.texcoord + float2(-texstep,-texstep)).a - _Cutoff) > 0 ? 1 : 0;
					float alpharightup = max(0,tex2D(_MainTex, i.texcoord + float2(texstep,texstep)).a - _Cutoff) > 0 ? 1 : 0;
					float alpharightdown = max(0,tex2D(_MainTex, i.texcoord + float2(texstep,-texstep)).a - _Cutoff) > 0 ? 1 : 0;
					
					col.a = 1.0/9 + alphaup/9 + alphadown/9 + alphaleft/9 + alpharight/9 + alphaleftup/9 + alphaleftdown/9 + alpharightup/9 + alpharightdown/9;
				}
	
				
				return col;
			}
		ENDCG
	}
}

}
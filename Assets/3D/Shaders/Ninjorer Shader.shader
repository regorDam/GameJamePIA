

Shader "Toon Alpha Bumped Outline Ninjorer OP" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0, 1)) = 0
		_Ramp ("Shading Ramp", 2D) = "gray" {}
		_MainTex ("Diffuse (RGB) - Alpha (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Cutoff ("Alpha", Range(0, 1)) = 0.5
	
    }
 
    SubShader {
		Tags { "RenderType"="Geometry" }
		CGPROGRAM
		#pragma surface surf Ramp alphatest:_Cutoff fullforwardshadows
		#include "UnityCG.cginc"


 
		sampler2D _Ramp;
 
		half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half3 ramp = tex2D (_Ramp, float2(diff,diff)).rgb;
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = s.Alpha;
			return c;
		}
 
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;

 
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
			o.Alpha =  tex2D (_MainTex, IN.uv_MainTex ).a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG


  		CGINCLUDE
		#include "UnityCG.cginc"
 
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};
 
		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
		};
		uniform float _Outline;
		uniform float4 _OutlineColor;

		v2f vert(appdata v) {
			v2f o;
			o.pos.xy +=  o.pos.z * _Outline;
			o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*_Outline,1) );
			o.color = _OutlineColor;
			return o;
		}
		ENDCG
 
		Pass {
			Name "OUTLINE"
			Cull Front
			ZWrite On
			ColorMask RGB


 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
	}
	Fallback "VertexLit"
}
 


//Base concept shader by:
//Jack Trades @ www.xiphias.org
//All modifies are done for us


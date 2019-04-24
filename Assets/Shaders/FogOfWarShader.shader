
Shader "Custom/FogOfWar" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,0.5)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_FogRadius("FogRadius", Float) = 1.0
		_FogClearRadius("FogClearRadius", Float) = 1.0
		//_FogMaxRadius("FogMaxRadius", Float) = 0.5
		//_Player1_Pos("_Player1_Pos", Vector) = (0,0,0,1)

		
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "Render" = "Transparent" "IgnoreProjector" = "True" }
		LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		CGPROGRAM

#pragma target 3.0 
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"


		fixed4     _Color;
		float     _FogRadius;
		float     _FogMaxRadius;
		//float4     _Player1_Pos;
		float4     _Player2_Pos;
		float4     _Player3_Pos;
		float _FogClearRadius;
		sampler2D _MainTex;
		
		float4 _lampePosition[10];
		float _lampeNumber; 

		struct appdata {
		float4 vertex : POSITION;
		float2 texcoord: TEXCOORD0;

		};

	struct v2f {
		float4 vertex : SV_POSITION;
		float3 worldPos : TEXCOORD0;
		float2 texcoord: TEXCOORD1;
	};

	float powerForPos(float4 pos, float2 nearVertex) {
		//float atten = clamp(_FogRadius - length(pos.xz - nearVertex.xy), 0.0, _FogRadius);
		/*if (length(pos.xz - nearVertex.xy) > _FogRadius)
			return 1.0; 
		else return 0.0;*/
		if (length(pos.xy - nearVertex.xy) < _FogClearRadius)
		{
			return 0.0;
		}else if (length(pos.xy - nearVertex.xy) < _FogRadius)
		{
			//return 0.0;
			return 1.0 -(_FogRadius - length(pos.xy - nearVertex.xy)) / _FogRadius;
		}
		else
			return 1.0;
		
		//return 0.0;
	}

	v2f vert(appdata v) {
		v2f o;

		o.worldPos = mul(unity_ObjectToWorld, v.vertex);
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = v.texcoord;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord)* _Color;
		
	float alpha = 1;
		int k = 0;
		for (k = 0; k < _lampeNumber; k++)
		{
			alpha *= powerForPos(_lampePosition[k], i.worldPos);
		}
		alpha = clamp(alpha, 0.0, 1.0);
		fixed4 output; 
		output.rgb = col.rgb; 
		//_debug_float = alpha;
		output.a = alpha;
		return output;
	}

		ENDCG
	}


	}
		FallBack "Diffuse"
}
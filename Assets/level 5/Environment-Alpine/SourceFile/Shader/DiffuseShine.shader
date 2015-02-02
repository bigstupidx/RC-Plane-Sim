Shader "TQWorld/Object/Diffuse Shine " {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_Shine ("Shine", Color) = (1,1,1,1)
	_MainTex ("Base (RGB),Shine in alpha", 2D) = "white" {}
	//_Lightmap ("Light Map",2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;
fixed4 _Shine;

struct Input {
	float2 uv_MainTex;


};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	//half3 l = tex2D(_LightMap, IN.uv2_LightMap).rgb;
	
	
	o.Albedo = tex.rgb ;
	
    fixed4 S =  _Shine * 10;
	o.Emission =  tex.a * tex * S ;
	
}
ENDCG
} 
FallBack "Self-Illumin/VertexLit"
}

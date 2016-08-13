Shader "Custom/Grid" {
	Properties{
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_ColorA("Color A", Color) = (0.64,0.64,0.64,1)
		_ColorB("Color B", Color) = (0.21,0.21,0.21,1)
		_GridOffset("Grid Offset", Range(1, 2)) = 1
		_GridScale("Grid Scale", Range(0, 10)) = 0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

		struct Input {
		float3 worldPos;
	};

	half _Glossiness;
	half _Metallic;
	float4 _ColorA;
	float4 _ColorB;
	float _GridOffset;
	float _GridScale;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		float3 c = (IN.worldPos.rgb*_GridScale).rgb;
		float r = ((frac(c.r)*_GridOffset) - 1.0);
		float g = ((frac(c.g)*_GridOffset) - 1.0);
		float b = ((frac(c.b)*_GridOffset) - 1.0);
		float if_leA = step((r*g*b), 0.0);
		float if_leB = step(0.0, (r*g*b));
		c = lerp((if_leA*_ColorB.rgb) + (if_leB*_ColorA.rgb), _ColorA.rgb, if_leA*if_leB);
		o.Albedo = c.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
	}
	ENDCG
	}
		FallBack "Diffuse"
}

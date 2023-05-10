// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ccc/Sky"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (1, 1, 1, 1)
        _NebulaTex ("Nebula Tex", 2D) = "white" {}
        _NebulaIntensity ("Color Intensity", Float) = 1
        _NoiseMask ("Noise Mask", 2D) = "white" {}
        _NoiseSpeed("Noise Speed", vector) = (0.0, 0.0, 0.0, 0.0)
        _StarIntensity("Star Intensity", Float) = 50
        _SkyFogOffset("Sky Fog Offset", vector) = (0.0, 0.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float4 pos_world : TEXCOORD2;
            };

            float4 _FogColor;
            sampler2D _NebulaTex;
            float4 _NebulaTex_ST;
            float _NebulaIntensity;
            sampler2D _NoiseMask;
            float4 _NoiseMask_ST;
            float2 _NoiseSpeed;
            float _StarIntensity;
            float3 _SkyFogOffset;

            v2f vert (a2v v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord0, _NebulaTex);
                o.uv.zw = TRANSFORM_TEX(v.texcoord1, _NoiseMask)+_Time.y*float2(_NoiseSpeed.x, _NoiseSpeed.y);
                o.normal_world = UnityObjectToWorldNormal(v.normal);
                o.pos_world = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                //ÐÇ¿Õ
                half3 baseColor  = tex2D(_NebulaTex, i.uv.xy).rgb;
                half  noiseMask  = tex2D(_NoiseMask, i.uv.zw).r;
                half3 starBlink = max(0.0, pow(baseColor, 5)) * noiseMask * _StarIntensity ;
                half3 diffColor = baseColor + starBlink;
                //Îí
                half3 viewDir = UnityWorldSpaceViewDir(i.pos_world);
                half3 normal_world = normalize(i.normal_world);

                half yDir = clamp(0.0, 1.0, normalize(normal_world + _SkyFogOffset).y);
                half dirFinal = clamp(0.0, 1.0, yDir);

                half Factor = 1 - abs(normalize(i.pos_world).y);
                half fogFactor = Factor * Factor * (1 - smoothstep(0.0, 0.9, dirFinal));

                half3 finalColor = lerp(dirFinal, diffColor, fogFactor) * _FogColor * _NebulaIntensity;
                return half4(finalColor, 1.0);
                //return yDir.xxxx;
            }
            ENDCG
        }
    }
}

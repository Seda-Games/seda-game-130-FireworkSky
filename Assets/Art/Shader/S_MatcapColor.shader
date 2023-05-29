// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ccc/MatcapColor"
{
    Properties
    {
        _Matcap     ("Matcap", 2D) = "white" {}
        _MatcapAdd  ("MatcapAdd", 2D) = "white" {}
        _MainTex("MainTex", 2D) = "white" {}
        _MatcapIntensity ("MatcapIntensity",Float) = 1.29
        _MatcapAddIntensity("MatcapAddIntensity",Float) = 0.18
        _FresnelPow("FresnelPow", Float) = 1
        _FresnelIntesity ("FresnelIntesity", Float) = 1
        _TopColor("TopColor", Color) = (1, 1, 1, 1)
        _TopColorConcrol ("TopColorConcrol", Range(0.0, 1.0)) = 0.0
        _TopMatcapAddIntensity("TopMatcapIntensity",Float) = 1

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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float3 normal : NORMAL;
                float4 color : color0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float2 uv_matcap : TEXCOORD1;
                float4 color : color0;
                float3 normal_world : TEXCOORD2;
                float4 pos_world : TEXCOORD3;

            };

            sampler2D _Matcap;
            sampler2D _MatcapAdd;
            sampler2D _MainTex;
            float _MatcapIntensity;
            float _MatcapAddIntensity;
            float _TopColorConcrol;
            float4 _TopColor;
            float _TopMatcapAddIntensity;
            float _FresnelIntesity;
            float _FresnelPow;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord0;
                o.normal_world = UnityObjectToWorldNormal(v.normal);
                o.pos_world = mul(unity_ObjectToWorld, v.vertex);
                o.uv_matcap = (mul(UNITY_MATRIX_V, o.normal_world).xy + 1 )*0.5;
                o.color = v.color;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half3 normal_world = normalize(i.normal_world);
                half3 vDir = normalize(UnityWorldSpaceViewDir(i.pos_world));
                half fresnel = saturate(pow(1 - dot(normal_world, vDir), _FresnelPow) * _FresnelIntesity);


                // sample the texture
                half4 MatcapColor = tex2D(_Matcap, i.uv_matcap)* _MatcapIntensity;
                half4 MatcapAddColor = tex2D(_MatcapAdd, i.uv_matcap) * _MatcapAddIntensity;
                half4 MainTexColor = tex2D(_MainTex, i.uv) ;
                half4 dColor = (MatcapColor * MainTexColor)+ fresnel;
                half4 topColor = (MatcapAddColor * _TopMatcapAddIntensity * _TopColor ) + fresnel;
                half4 topColor0 = (MatcapAddColor * MainTexColor) + fresnel;
                half4 topColor1 = lerp(topColor0, topColor, _TopColorConcrol+0.001);
                half4 finalColor = lerp(topColor1, dColor, i.color.r+0.001);
                finalColor =  sqrt(max(exp2(log2(max(finalColor, 0.0)) * 2.2), 0.0));
                return finalColor;
               
            }
            ENDCG
        }
    }
            FallBack"Diffuse"
}

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ccc/HalfLambert Ground Common"
{
    Properties
    {
        _DiffuseTex ("Diffuse Tex", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (1, 1, 1, 1)
        _BaseShadowPower ("Power", Float) = 1
        _BaseShadowMul ("Mul", Float) = 1

    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
                float4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float3 pos_world : TEXCOORD2;

            };
            sampler2D _DiffuseTex;
            float4 _BaseColor;
            float4 _ShadowColor;
            float _BaseShadowPower;
            float _BaseShadowMul;
            float4 _LightColor0;


            v2f vert (a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.pos_world = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal_world = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
                return o;
            }




            half4 frag(v2f i) : SV_Target
            {
                //dir
                half3 normal_world = normalize(i.normal_world);
                half3 view_world = normalize(_WorldSpaceCameraPos.xyz - i.pos_world);
                half3 light_world = normalize(_WorldSpaceLightPos0.xyz);

                //BaseColor
                half3 diffuseCol = tex2D(_DiffuseTex, i.uv).rgb;
                half3 baseColor = diffuseCol * _BaseColor.rgb;

                //ShadowColor
                half3 shadowColor = diffuseCol * _ShadowColor.rgb;

                //LightAtten
                half3 lightColor =  _LightColor0.rgb;
                half lightAtten = max(max(lightColor.x, lightColor.y), lightColor.z);

                //Direct Diffuse直接光漫反射
                half diff_term = max(0.0, dot(normal_world, light_world));
                half halfLambert = diff_term * 0.5 + 0.5;
                half diffVal = saturate(pow(halfLambert * lightAtten, _BaseShadowPower) * _BaseShadowMul);


                half3 finalColor = max(0, lerp(shadowColor, baseColor, diffVal) * lightColor );
                finalColor = sqrt(max(exp2(log2(max(finalColor, 0.0)) * 2.2), 0.0));

                return half4(finalColor, 1.0);
            }
            ENDCG
        }
    }
        FallBack"Diffuse"
}

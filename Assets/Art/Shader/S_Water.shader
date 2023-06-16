// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ccc/S_Water"
{
    Properties
    {
        _ReflectionTex("ReflectionTex", 2D) = "white" {}
        _RefIntensity ("RefIntensity", float) =1.3
        _WaterNormal ("WaterNormal", 2D) = "blump"{}
        _NormalIntensity ("NormalIntensity", float) = 6.84
        _NormalTilling("NormalTilling", float) = 11.9
        _UnderWaterTilling ("UnderWaterTilling", float) = 225
        _WaterSpeed ("WaterSpeed", float) = 1
        _SpecSmoothess("SpecSmoothess", Range(0.1,1.0)) = 0.1
        _SpecTint ("SpecTint", COLOR) = (1, 1, 1, 1)
        _SpecIntensity ("SpecIntensity", float) = 0.95
        _SpecEnd ("SpecEnd", float) = 300
        _SpecStart ("SpecStart", float) = 0
        _UnderfIntensity ("UnderfIntensity", float) = 1
        _RT("RT", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"  }
        LOD 100

        Pass
        {
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "AutoLight.cginc"
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

    sampler2D _ReflectionTex;
    float _RefIntensity;
    sampler2D _WaterNormal;
    float _NormalIntensity;
    float _NormalTilling;
    float _UnderWaterTilling;
    float _WaterSpeed;
    float _SpecSmoothess;
    float4 _SpecTint;
    float _SpecIntensity;
    float _SpecEnd;
    float _SpecStart;
    float _UnderfIntensity;
    sampler2D _RT;
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float3 binormalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float4 worldPos : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord0;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.normalWS = UnityObjectToWorldDir(v.normal);
                o.tangentWS = UnityObjectToWorldDir(v.tangent).xyz;
                o.binormalWS = normalize(cross(o.normalWS, o.tangentWS.xyz)) * v.tangent.w;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                half3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                float4 screenPos = float4(i.screenPos.xyz , i.screenPos.w+ 0.00000000001);
                float4 screenPosNorm = screenPos / screenPos.w;
                //water normal
                //half3 normalTS = UnpackNormal(tex2D(_WaterNormal,i.uv));
                half3x3 TBN = half3x3(i.tangentWS, i.binormalWS, i.normalWS);
                half normalValue = _Time.y * 0.1 * _WaterSpeed;
                half2 normalUV1 = i.worldPos.xz / _NormalTilling  + normalValue;
                half2 normalUV2 = i.worldPos.xz / _NormalTilling * 1.5 + normalValue*-1;
                half3 waterNormal1 = tex2D(_WaterNormal, normalUV1).xyz;
                half3 waterNormal2 = UnpackNormal(tex2D(_WaterNormal, normalUV2));
                half2 blendNormal = (waterNormal1 + waterNormal2).xy*0.5;
                half3 waterNormalFinal = half3(blendNormal, sqrt(1 - dot(blendNormal, blendNormal)));
                //half3 normalTS = UnpackNormal(half4(waterNormalFinal, 1.0));
                half3 normalWS = mul(waterNormalFinal, TBN);
                //ReflectColor
                half2 reflectionUV = normalWS.xz / (i.pos.w + 1) * _NormalIntensity +screenPosNorm.xy;
                half3 reflectionCol = tex2D(_ReflectionTex, reflectionUV).rgb;
                half3 rtCol = tex2D(_RT, half2(i.uv.x, 1- i.uv.y)).rgb*0.3;
                half3 reflectionColFilnal = reflectionCol + rtCol;
                //SpecColor
                half SpecValue = pow(max(0.0,dot(normalWS, normalize(viewDir + lightDir))), 256* _SpecSmoothess);
                half3 SpecColor = _SpecTint * SpecValue * _SpecIntensity;
                //UnderWaterColor
                half2 underWaterUV = i.worldPos.xz / _UnderWaterTilling + normalWS.xy * 0.1;
                half3 UnderWaterColor = tex2D(_ReflectionTex, underWaterUV);
                //combine
                half fresnel = saturate(1 - dot(i.normalWS, viewDir));
                half3 col = lerp(UnderWaterColor * _UnderfIntensity, reflectionColFilnal * _RefIntensity, fresnel);
                half3 finalCol = col + SpecColor;
                return half4(finalCol, 1.0);
            }
            ENDCG
        }
    }
}

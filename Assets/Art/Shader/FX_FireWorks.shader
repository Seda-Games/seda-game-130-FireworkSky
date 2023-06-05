Shader "ccc/FX_FireWorks"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Power("_Power",float) = 1
        _Transparent("_Transparent",range(0,1)) = 1
        _Round_Anim("Round&Anim",vector) = (0,0,0,0)
    }
        SubShader
    {
        Tags { "RenderType" = "Transprent" "Queue" = "Transparent" "PreviewType" = "Plane" "IgnoreProjector" = "True"}
        LOD 100
    ZWrite Off
    Cull back
    Blend SrcAlpha One
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_particles           
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float2 customData	: TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 roundClamp : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Power;
            float _Transparent;
        //XY Clamp,Z - all shake; W - shake Speed
            float4 _Round_Anim;
            half4 _Color;
             v2f vert(appdata v)
                    {
                     v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv.xy = v.texcoord0;
                        o.uv.zw = v.texcoord1.xy;
                        o.customData = v.texcoord1.zw;
                        o.roundClamp.xy = v.color.rg * 1.5;	//use for round scale
                        o.roundClamp.z = abs(cos(_Time.y * _Round_Anim.w)) * (1 - _Round_Anim.z) + _Round_Anim.z;
                        o.roundClamp.w = abs(sin(v.color.r * _Time.y * _Round_Anim.w + v.color.g * 20))+0.001;

                        return o;
                    }

                 float2 hash22(float2 xy)
                     {
                    float2 hash = frac(sin(float2(1.0 + dot(xy, float2(37.0, 17.0)), 2.0 + dot(xy, float2(11.0, 47.0)))) * 103.0);
                    return hash;
                 }


                fixed4 frag(v2f i) : SV_Target
                    {
                    fixed4 col = fixed4(0,0,0,1);
                    float2 dropPos = i.uv.xy - 0.5;
                    float maxRound = clamp(i.roundClamp.z * (_Round_Anim.x + i.roundClamp.x), 0.2, 0.7);
                    float minRound = clamp(i.roundClamp.z * (_Round_Anim.y - i.roundClamp.y), 0, 0.199);
                    float drop = smoothstep(maxRound, minRound, length(dropPos));
                    col += drop;
                    fixed4 tex = tex2D(_MainTex, i.uv.zw);
                    float mask = step(length(i.uv.zw - 0.5),0.5);
                    col.rgb *= tex.rgb * _Power * _Color.rgb;
                    col.a *= mask * tex.a * _Transparent * i.roundClamp.w * _Color.a * i.customData.x;
                    return col;
                    }
                    ENDCG
                }
    }
}


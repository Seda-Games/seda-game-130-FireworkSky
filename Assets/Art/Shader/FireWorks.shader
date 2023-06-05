Shader "ccc/FireWorks"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Power("_Power",float) = 1
        _Transparent("_Transparent",range(0,1)) = 1
        _Round_Anim("Round&Anim",vector) = (0,0,0,0)
        _Offset ("Offset", float) = 0.0
            _Dissolve("Dissolve",Range(0.0,1.0))= 1
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
                float4 vertex : SV_POSITION;
                float4 roundClamp : TEXCOORD2;
                float2 custom : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Power;
            float _Transparent;
            float4 _Round_Anim;
            float _Offset;
            float _Dissolve;

             v2f vert(appdata v)
                    {
                     v2f o;                   
                        o.uv.xy = v.texcoord0;
                        o.uv.zw = v.texcoord1.xy;
                        o.custom = v.texcoord1.zw;
                        float offset = 1-o.uv.w;
                        o.vertex = UnityObjectToClipPos(v.vertex+float4(0, offset * -o.custom.y,0, 0));
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

                 float heart(float2 st)
                 {
                     st = (st - float2(0.5, 0.38)) * float2(2.1, 2.8);
                     return pow(st.x, 2) + pow(st.y - sqrt(abs(st.x)), 2);
                 }
  

                fixed4 frag(v2f i) : SV_Target
                    {
                    fixed4 col = fixed4(0,0,0,1);
                    float2 dropPos = i.uv.xy - 0.5;
                    float2 heartUv = (i.uv.xy * 2 - 1)+0.5;
                    float pattern = 1 - step(_Transparent, heart(heartUv));
                    float maxRound = clamp(i.roundClamp.z * (_Round_Anim.x + i.roundClamp.x), 0.2, 0.7);
                    float minRound = clamp(i.roundClamp.z * (_Round_Anim.y - i.roundClamp.y), 0, 0.199);
                    float drop = saturate(smoothstep(maxRound, minRound, 1-pattern));

                    col += drop;
                    fixed4 tex = tex2D(_MainTex, i.uv.zw);
                    float mask = step(length(i.uv.zw - 0.5),0.5);
                    col.rgb *= tex.rgb * _Power;
                    col.a *= mask * tex.a * _Transparent * i.roundClamp.w * (1- i.custom.x -(1-i.uv.wwww));
                    col.a = saturate(col.a);
                    //return fixed4(i.roundClamp.zz,0,1);
                    return col;
                    }
                    ENDCG
                }
    }
}


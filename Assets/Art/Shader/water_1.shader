Shader "ccc/water_unitl"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterColor01("water Color 01",Color)=(1,1,1,1)
        _WaterColor02("water Color 02",Color)=(1,1,1,1)
        _TranAmount("TransAmount",Range(0,100))=5
        _DepthRange("Depth Range",Range(1,100))=1
        // bumpMap
        _NormalTex ("bump map",2D) = "bump" {}
        _WaterSpeed("Water speed",float) = 5
        _BumpSacle("bump scale",float) = 1
        _Specular ("Specualr",Color) = (1,1,1,1)
        _Gloss("Gloss",Range(0,256))=8
             
        //波浪
        /*_WaveTex("WaveTex",2D) = "white" {}
       _NoiseTex("Noise",2D) = "white" {}
        _WaveSpeed("Wave speed",float) = 1
        _WaveRange("wave range",float) = 0.5
        _WaveRangeA("wave rangeA",float) = 1
        _WaveDetal("wave offset  Detal",float) = 0.5*/
        // grabpass
        _Distortion("Distortion",float) = 1
        // cubemap
        _CubeMap("CubeMap",Cube) = "SkyBox"{}
        _FresnelSacle("Fresnel Sacle",float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite off
        //GrabPass {"_GrabPassTex"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D_float _CameraDepthTexture;
            fixed4 _WaterColor01;
            fixed4 _WaterColor02;
            float _TranAmount;
            float _DepthRange;
            sampler2D _NormalTex;
            float4 _NormalTex_ST;
            float _BumpSacle;
            float _WaterSpeed;
            float4 _Specular;
            float _Gloss;
            //sampler2D _NoiseTex; float4 _NoiseTex_ST;
            /*sampler2D _WaveTex; float4 _WaveTex_ST;
            
            float _WaveSpeed;
            float _WaveRange;
            float _WaveRangeA;
            float _WaveDetal;*/
            //sampler2D _GrabPassTex;
            //float4 _GrabPassTex_TexelSize;
            float _Distortion;
            samplerCUBE _CubeMap;
            float _FresnelSacle;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;                
                float4 pos : SV_POSITION;
                float4 proj : TEXCOORD1;
                float2 uv_NormalTex : TEXCOORD2;
                float4 TtoW0 : TEXCOORD3;
                float4 TtoW1 : TEXCOORD4;
                float4 TtoW2 : TEXCOORD5;
                LIGHTING_COORDS(6,7) //阴影+衰减
                //float2 uv_WaveTex : TEXCOORD8;
                //float2 uv_NoiseTex : TEXCOORD9;
            };



            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_NormalTex = TRANSFORM_TEX(v.uv, _NormalTex);
                //o.uv_WaveTex = TRANSFORM_TEX(v.uv, _WaveTex);
                //o.uv_NoiseTex = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.proj = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(o.proj.z);

                float3 worldPos = mul(unity_ObjectToWorld,v.vertex);
                float3 tangent = UnityObjectToWorldDir(v.tangent.xyz);
                float3 normal = UnityObjectToWorldNormal(v.normal);
                float3 biTangent = cross(tangent,normal) * v.tangent.w;
                o.TtoW0 = fixed4(tangent.x,biTangent.x,normal.x,worldPos.x);
                o.TtoW1 = fixed4(tangent.y,biTangent.y,normal.y,worldPos.y);
                o.TtoW2 = fixed4(tangent.z,biTangent.z,normal.z,worldPos.z);
                //包含光照衰减和阴影				
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.proj)).r);
                half deltaDepth = depth - i.proj.z;

                // sample the textureUNITY_PROJ_COORD()i.proj
                fixed4 col = lerp(_WaterColor01,_WaterColor02,min(_DepthRange,deltaDepth)/_DepthRange); //tex2D(_MainTex, i.uv);
                col.a = min(_TranAmount,deltaDepth)/_TranAmount;// col.a * _TranAmount;

                // normal
                float4 bumpOffset1 = tex2D(_NormalTex,i.uv_NormalTex + float2(_WaterSpeed * _Time.y,0));
                float4 bumpOffset2 = tex2D(_NormalTex,float2( 1 - i.uv_NormalTex.y,i.uv_NormalTex.x) + float2(_WaterSpeed * _Time.y/10,0));
                float4 offsetColor = (bumpOffset1 + bumpOffset2)/2;
                float3 normal = UnpackNormal(offsetColor);
                normal.xy *= _BumpSacle;                
                float4 bumpColor1 = tex2D(_NormalTex,i.uv_NormalTex + normal.xy + float2(_WaterSpeed * _Time.y,0));
                float4 bumpColor2 = tex2D(_NormalTex,float2( 1 - i.uv_NormalTex.y,i.uv_NormalTex.x) + normal.xy + float2(_WaterSpeed * _Time.y/10,0));
                normal = UnpackNormal((bumpColor1 + bumpColor2)/2).xyz;
                fixed2 offset = normal.xy;
                normal = normalize(half3(dot(i.TtoW0.xyz,normal),dot(i.TtoW1.xyz,normal),dot(i.TtoW2.xyz,normal)));
                
                //光照参数
                float3 worldPos = float3(i.TtoW0.w,i.TtoW1.w,i.TtoW2.w);
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                float3 halfDir = normalize(worldLightDir + worldViewDir);
                //包含光照衰减以及阴影，但是base光源一般没有衰减
				UNITY_LIGHT_ATTENUATION(atten, i, worldPos);
                float3 diffuseFactor = max(0, dot(normal,worldLightDir));
                float3 specualrFactor = pow(max(0,dot(normal,halfDir)),_Gloss);
                col.rgb = atten * (col.rgb * _LightColor0.rgb * diffuseFactor + _Specular.rgb * specualrFactor * _LightColor0.rgb);
                col.a = col.a + _Specular.a * specualrFactor;

                // 波浪
                //float waveA = 1 - min(_WaveRangeA,deltaDepth) / _WaveRangeA; 

                //fixed4 noiseColor = tex2D(_NoiseTex,i.uv_NoiseTex);
                //fixed4 waveColor1 = tex2D(_WaveTex,float2(waveA + _WaveRange * sin(_Time.x * _WaveSpeed + noiseColor.r),1 )+ offset);
                //waveColor1.rgb *= (1-(sin(_Time.x * _WaveSpeed + noiseColor.r)+1)/2) * noiseColor.r;
                //fixed4 waveColor2 = tex2D(_WaveTex,float2(waveA + _WaveRange * sin(_Time.x * _WaveSpeed + _WaveDetal + noiseColor.r),1)+offset);
                //waveColor2.rgb *= (1-sin(_Time.x * _WaveSpeed + _WaveDetal + noiseColor.r)+1)/2 * noiseColor.r; 
                //col.rgb = col.rgb + (waveColor1.rgb + waveColor2.rgb) * waveA;
                //grabpass计算折射
                //对屏幕图像的采样坐标进行偏移
				//选择使用切线空间下的法线方向来进行偏移是因为该空间下的法线可以反映顶点局部空间下的法线方向
                offset = offset * _Distortion;// *_GrabPassTex_TexelSize.xy;
                //对scrPos偏移后再透视除法得到真正的屏幕坐标
                i.proj.xy = offset * i.proj.z + i.proj.xy;
                //fixed3 refraCol = tex2D(_GrabPassTex,i.proj.xy/i.proj.w).rgb;
                
                //反射
                fixed3 reflectDir = reflect(-worldViewDir,normal);
                fixed3 reflection = texCUBE(_CubeMap,reflectDir);
                fixed3 fresnel = _FresnelSacle + (1-_FresnelSacle) * pow(1 - dot(worldViewDir,normal),5);
                //fixed3 refraAndRefle = lerp(refraCol,reflection,fresnel);
                fixed3 refraAndRefle= reflection * fresnel;
                col.rgb *= refraAndRefle;
                return col;
            }
            ENDCG
        }
    }
}

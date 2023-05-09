// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/HalfLambertWallWorldUV"
{
	Properties
	{
		_CoverColor("CoverColor", Color) = (0,0,0,0)
		_CoverColorIntensity("CoverColorIntensity", Range( 0 , 1)) = 0
		[Toggle(_ACES_ON)] _ACES("ACES", Float) = 0
		[Toggle]_WorldUV("WorldUV", Float) = 0
		_WroldUVTilingOffset("WroldUVTilingOffset", Vector) = (1,1,0,0)
		_DiffuseTex("DiffuseTex", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (0,0,0,0)
		_ShaowColor("ShaowColor", Color) = (0,0,0,0)
		_Power("Power", Float) = 1
		_Mul("Mul", Float) = 1
		[Toggle(_USELMP_ON)] _UseLmp("UseLmp", Float) = 0
		_LmpColor("LmpColor", Color) = (1,1,1,1)
		_LmpIntensity("LmpIntensity", Float) = 1
		_LightMul("LightMul", Float) = 0
		[Toggle]_HighlightWorldUV("HighlightWorldUV", Float) = 1
		_WroldUVTilingOffset2("WroldUVTilingOffset2", Vector) = (1,1,0,0)
		_FloorHighlightTex("FloorHighlightTex", 2D) = "black" {}
		_FloorHighlightColorTex("FloorHighlightColorTex", 2D) = "white" {}
		_FloorHighlightIntesity("FloorHighlightIntesity", Float) = 1
		_HighlightShaowIntensity("HighlightShaowIntensity", Range( 0 , 1)) = 0.3
		[Toggle]_MulTexWorldUV("MulTexWorldUV", Float) = 1
		_MulTex("MulTex", 2D) = "white" {}
		_WroldUVTilingOffset3("WroldUVTilingOffset3", Vector) = (1,1,0,0)
		_MulColor("MulColor", Color) = (1,1,1,0)
		_MulColorIntensity("MulColorIntensity", Float) = 0
		_EmissionColor("EmissionColor", Color) = (1,1,1,0)
		_EmissionIntensity("EmissionIntensity", Range( 0 , 1)) = 0
		_MetalTex("MetalTex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "DisableBatching" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature_local _ACES_ON
		#pragma shader_feature_local _USELMP_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _MulColor;
		uniform sampler2D _MulTex;
		uniform float _MulTexWorldUV;
		uniform sampler2D _FloorHighlightTex;
		uniform float4 _FloorHighlightTex_ST;
		uniform float4 _WroldUVTilingOffset3;
		uniform float _HighlightWorldUV;
		uniform float4 _WroldUVTilingOffset2;
		uniform sampler2D _FloorHighlightColorTex;
		uniform float4 _BaseColor;
		uniform sampler2D _DiffuseTex;
		uniform float _WorldUV;
		uniform float4 _DiffuseTex_ST;
		uniform float4 _WroldUVTilingOffset;
		uniform sampler2D _MetalTex;
		uniform float4 _MetalTex_ST;
		uniform float _HighlightShaowIntensity;
		uniform float4 _ShaowColor;
		uniform float _FloorHighlightIntesity;
		uniform float _MulColorIntensity;
		uniform float _Power;
		uniform float _Mul;
		uniform float _LmpIntensity;
		uniform float4 _LmpColor;
		uniform float _LightMul;
		uniform float4 _EmissionColor;
		uniform float _EmissionIntensity;
		uniform float4 _CoverColor;
		uniform float _CoverColorIntensity;


		float3 ACESTonemap107( float3 LinearColor )
		{
			float a = 2.51f;
			float b = 0.03f;
			float c = 2.43f;
			float d = 0.59f;
			float e = 0.14f;
			return
			saturate((LinearColor*(a*LinearColor+b))/(LinearColor*(c*LinearColor+d)+e));
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 temp_output_71_0 = ( ase_lightAtten * ase_lightColor.rgb );
			float3 temp_cast_0 = (1.0).xxx;
			#ifdef UNITY_PASS_FORWARDBASE
				float3 staticSwitch84 = temp_cast_0;
			#else
				float3 staticSwitch84 = temp_output_71_0;
			#endif
			float3 LightColor78 = staticSwitch84;
			float2 uv_FloorHighlightTex = i.uv_texcoord * _FloorHighlightTex_ST.xy + _FloorHighlightTex_ST.zw;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_worldPos = i.worldPos;
			float2 appendResult164 = (float2(( 1.0 - ase_worldPos.z ) , ase_worldPos.y));
			float2 WorldUVyz199 = appendResult164;
			float2 appendResult233 = (float2(ase_worldPos.x , ase_worldPos.y));
			float2 WorldUVxy238 = appendResult233;
			float2 temp_output_250_0 = ( ( ase_vertex3Pos.x == 1.0 ? float2( 0,0 ) : WorldUVyz199 ) + ( ase_vertex3Pos.z == 1.0 ? float2( 0,0 ) : WorldUVxy238 ) );
			float2 appendResult285 = (float2(_WroldUVTilingOffset3.x , _WroldUVTilingOffset3.y));
			float2 appendResult286 = (float2(_WroldUVTilingOffset3.z , _WroldUVTilingOffset3.w));
			float2 WorldUV3290 = frac( ( ( temp_output_250_0 * appendResult285 ) + appendResult286 ) );
			float4 lerpResult198 = lerp( _MulColor , float4( 1,1,1,1 ) , tex2D( _MulTex, (( _MulTexWorldUV )?( WorldUV3290 ):( uv_FloorHighlightTex )) ).r);
			float4 MulColor182 = lerpResult198;
			float2 appendResult267 = (float2(_WroldUVTilingOffset2.x , _WroldUVTilingOffset2.y));
			float2 appendResult268 = (float2(_WroldUVTilingOffset2.z , _WroldUVTilingOffset2.w));
			float2 WorldUV2270 = frac( ( ( temp_output_250_0 * appendResult267 ) + appendResult268 ) );
			float2 uv_DiffuseTex = i.uv_texcoord * _DiffuseTex_ST.xy + _DiffuseTex_ST.zw;
			float2 appendResult259 = (float2(_WroldUVTilingOffset.x , _WroldUVTilingOffset.y));
			float2 appendResult261 = (float2(_WroldUVTilingOffset.z , _WroldUVTilingOffset.w));
			float2 WorldUV1239 = frac( ( ( temp_output_250_0 * appendResult259 ) + appendResult261 ) );
			float4 BaseColor96 = ( _BaseColor * tex2D( _DiffuseTex, (( _WorldUV )?( WorldUV1239 ):( uv_DiffuseTex )) ) );
			float2 uv_MetalTex = i.uv_texcoord * _MetalTex_ST.xy + _MetalTex_ST.zw;
			float4 lerpResult296 = lerp( float4( 0,0,0,0 ) , ( tex2D( _FloorHighlightTex, (( _HighlightWorldUV )?( WorldUV2270 ):( uv_FloorHighlightTex )) ) * tex2D( _FloorHighlightColorTex, (( _HighlightWorldUV )?( WorldUV2270 ):( uv_FloorHighlightTex )) ) * BaseColor96 ) , tex2D( _MetalTex, uv_MetalTex ).r);
			float4 FloorHighlight170 = lerpResult296;
			float4 ShaowColor93 = ( _ShaowColor * tex2D( _DiffuseTex, (( _WorldUV )?( WorldUV1239 ):( uv_DiffuseTex )) ) );
			float4 blendOpSrc281 = ( FloorHighlight170 * _HighlightShaowIntensity );
			float4 blendOpDest281 = ShaowColor93;
			float4 lerpBlendMode281 = lerp(blendOpDest281,( 1.0 - ( 1.0 - blendOpSrc281 ) * ( 1.0 - blendOpDest281 ) ),_FloorHighlightIntesity);
			float4 blendOpSrc295 = MulColor182;
			float4 blendOpDest295 = ( saturate( lerpBlendMode281 ));
			float4 lerpBlendMode295 = lerp(blendOpDest295,( blendOpSrc295 * blendOpDest295 ),_MulColorIntensity);
			float4 blendOpSrc177 = FloorHighlight170;
			float4 blendOpDest177 = BaseColor96;
			float4 lerpBlendMode177 = lerp(blendOpDest177,( 1.0 - ( 1.0 - blendOpSrc177 ) * ( 1.0 - blendOpDest177 ) ),_FloorHighlightIntesity);
			float4 blendOpSrc179 = MulColor182;
			float4 blendOpDest179 = ( saturate( lerpBlendMode177 ));
			float4 lerpBlendMode179 = lerp(blendOpDest179,( blendOpSrc179 * blendOpDest179 ),_MulColorIntensity);
			float3 break72 = temp_output_71_0;
			float LightAtten56 = max( max( break72.x , break72.y ) , break72.z );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 WorldNormal22 = ase_normWorldNormal;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult5_g1 = dot( WorldNormal22 , ase_worldlightDir );
			float temp_output_114_0 = ( pow( ( LightAtten56 * (dotResult5_g1*0.5 + 0.5) ) , _Power ) * _Mul );
			float4 temp_cast_2 = (saturate( temp_output_114_0 )).xxxx;
			float4 temp_cast_3 = (temp_output_114_0).xxxx;
			UnityGI gi119 = gi;
			float3 diffNorm119 = WorldNormalVector( i , WorldNormal22 );
			gi119 = UnityGI_Base( data, 1, diffNorm119 );
			float3 indirectDiffuse119 = gi119.indirect.diffuse + diffNorm119 * 0.0001;
			float3 LightMap151 = indirectDiffuse119;
			#ifdef _USELMP_ON
				float4 staticSwitch132 = ( saturate( min( temp_cast_3 , ( _LmpIntensity * float4( LightMap151 , 0.0 ) * _LmpColor ) ) ) + float4( ( max( (LightMap151*0.3 + -0.3) , float3( 0,0,0 ) ) * _LightMul ) , 0.0 ) );
			#else
				float4 staticSwitch132 = temp_cast_2;
			#endif
			float4 lerpResult5 = lerp( ( saturate( lerpBlendMode295 )) , ( saturate( lerpBlendMode179 )) , staticSwitch132);
			float4 temp_output_105_0 = max( ( float4( LightColor78 , 0.0 ) * ( lerpResult5 + ( _EmissionColor * _EmissionIntensity * (BaseColor96).r ) ) ) , float4( 0,0,0,0 ) );
			float3 LinearColor107 = ( temp_output_105_0 * temp_output_105_0 ).rgb;
			float3 localACESTonemap107 = ACESTonemap107( LinearColor107 );
			#ifdef _ACES_ON
				float4 staticSwitch110 = float4( sqrt( localACESTonemap107 ) , 0.0 );
			#else
				float4 staticSwitch110 = temp_output_105_0;
			#endif
			float4 lerpResult155 = lerp( staticSwitch110 , _CoverColor , _CoverColorIntensity);
			c.rgb = lerpResult155.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
119;458;1248;747;2075.174;1586.207;1.393031;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;163;-5559.766,1207.073;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;262;-5383.754,1311.824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;232;-5459.256,1757.217;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;164;-5189.64,1250.166;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;233;-5225.311,1790.584;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;199;-5029.842,1250.932;Inherit;False;WorldUVyz;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;238;-5038.494,1785.358;Inherit;False;WorldUVxy;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;243;-4570.131,1232.171;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;252;-4571.271,1527.134;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;231;-4332.368,1330.848;Inherit;False;0;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;260;-4591.408,2368.167;Inherit;False;Property;_WroldUVTilingOffset;WroldUVTilingOffset;4;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;251;-4325.239,1595.053;Inherit;False;0;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;250;-4086.813,1434.195;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;259;-4278.852,2323.331;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;261;-4266.518,2449.727;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;236;-3842.78,1440.726;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;269;-4541.358,2692.827;Inherit;False;Property;_WroldUVTilingOffset2;WroldUVTilingOffset2;15;0;Create;True;0;0;0;False;0;False;1,1,0,0;0.05,0.05,0.2,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;263;-3668.278,1455.425;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;267;-4228.803,2647.991;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FractNode;257;-3512.478,1439.826;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;89;-4621.485,-495.2582;Inherit;False;1513.141;690.5745;LightAtten;10;56;78;85;84;73;74;72;71;69;70;LightAtten;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;268;-4216.469,2774.387;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;239;-3323.741,1413.453;Inherit;False;WorldUV1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LightAttenuation;70;-4584.445,-209.5635;Inherit;True;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;-3848.292,2306.56;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LightColorNode;69;-4555.197,0.3959541;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;185;-5108.846,-1211.095;Inherit;False;0;62;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;200;-4950.371,-999.064;Inherit;False;239;WorldUV1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-4301.742,-133.9185;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;284;-4556.766,2991.821;Inherit;False;Property;_WroldUVTilingOffset3;WroldUVTilingOffset3;22;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;265;-3655.593,2299.16;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;188;-4766.679,-1162.119;Inherit;False;Property;_WorldUV;WorldUV;3;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FractNode;266;-3458.191,2293.96;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;103;-4618.97,-1901.393;Inherit;False;504.303;432.5138;WorldNormal&ViewDir;4;37;36;22;21;WorldNormal&ViewDir;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;72;-3988.142,-114.9577;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;95;-3519.517,-1368.414;Inherit;False;919.8992;508.5779;BaseColor;4;3;63;62;96;BaseColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;285;-4229.603,2943.419;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;270;-3258.704,2250.873;Inherit;False;WorldUV2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;195;-2523.84,-1494.321;Inherit;False;1813.777;925.9236;FloorHighlight;19;170;176;173;196;180;293;291;292;182;198;178;168;194;169;202;296;297;298;299;FloorHighlight;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;286;-4217.269,3069.815;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;3;-3443.39,-1318.414;Inherit;False;Property;_BaseColor;BaseColor;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;74;-3748.239,-136.0654;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;62;-3477.179,-1102.064;Inherit;True;Property;_DiffuseTex;DiffuseTex;5;0;Create;True;0;0;0;False;0;False;-1;None;1bd3ee965f7b54247b847c8981483e2f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;21;-4580.688,-1839.904;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;287;-3886.247,2721.892;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;169;-2495.54,-1357.038;Inherit;False;0;168;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;202;-2464.044,-1207.957;Inherit;False;270;WorldUV2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-4358.93,-1843.281;Inherit;False;WorldNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;73;-3618.226,-116.7487;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;288;-3684.855,2716.231;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-3082.129,-1176.357;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-3335.325,-111.0509;Inherit;False;LightAtten;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2521.508,-26.84355;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;-2889.51,-1170.921;Inherit;False;BaseColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-2326.067,434.7483;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FractNode;289;-3496.281,2718.145;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;194;-2210.657,-1309.411;Inherit;False;Property;_HighlightWorldUV;HighlightWorldUV;14;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;178;-1662.024,-1018.933;Inherit;False;96;BaseColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;290;-3288.604,2709.714;Inherit;False;WorldUV3;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;1;-2313.352,-23.64051;Inherit;False;Half Lambert Term;-1;;1;86299dc21373a954aa5772333626c9c1;0;1;3;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;176;-1775.168,-1240.491;Inherit;True;Property;_FloorHighlightColorTex;FloorHighlightColorTex;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;57;-2271.129,-126.6467;Inherit;False;56;LightAtten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;119;-2113.223,439.3763;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;168;-1779.455,-1444.321;Inherit;True;Property;_FloorHighlightTex;FloorHighlightTex;16;0;Create;True;0;0;0;False;0;False;-1;None;48da81f073562cb4697e7db3b582ea9e;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;-1420.307,-1264.402;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;-2426.082,-766.1576;Inherit;False;290;WorldUV3;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;292;-2442.07,-939.1524;Inherit;False;0;168;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;92;-4618.395,-1365.605;Inherit;False;1009.513;513.5418;ShaowColor;4;61;4;60;93;ShaowColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-1899.42,186.6232;Inherit;False;Property;_Power;Power;8;0;Create;True;0;0;0;False;0;False;1;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-1954.754,-42.57423;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-1848.682,434.9766;Inherit;False;LightMap;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;297;-1440.926,-1105.501;Inherit;True;Property;_MetalTex;MetalTex;28;0;Create;True;0;0;0;False;0;False;-1;None;b93ae725550813142b98d26c8a6d3f74;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;112;-1696.724,-16.07341;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;293;-2184.89,-866.8618;Inherit;False;Property;_MulTexWorldUV;MulTexWorldUV;20;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;296;-1097.347,-1279.736;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;122;-1880.518,331.5341;Inherit;False;Property;_LmpIntensity;LmpIntensity;12;0;Create;True;0;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-1956.17,951.6128;Inherit;False;Constant;_Offset;Offset;11;0;Create;True;0;0;0;False;0;False;-0.3;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;-2034.589,784.9277;Inherit;False;151;LightMap;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;4;-4506.395,-1317.605;Inherit;False;Property;_ShaowColor;ShaowColor;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.9150943,0.5817209,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;144;-1956.17,872.6122;Inherit;False;Constant;_Scale;Scale;10;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;127;-1833.637,555.5023;Inherit;False;Property;_LmpColor;LmpColor;11;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0.6735501,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-1620.284,198.5722;Inherit;False;Property;_Mul;Mul;9;0;Create;True;0;0;0;False;0;False;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;60;-4559.485,-1083.524;Inherit;True;Property;_MainTex;MainTex;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;62;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;196;-1873.996,-996.6017;Inherit;False;Property;_MulColor;MulColor;23;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;180;-1944.363,-821.1026;Inherit;True;Property;_MulTex;MulTex;21;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-1448.42,-8.950319;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-1569.127,405.6723;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-4181.719,-1136.929;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;143;-1707.39,759.4601;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;170;-916.4795,-1287.616;Inherit;True;FloorHighlight;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1489.59,930.0141;Inherit;False;Property;_LightMul;LightMul;13;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;-3978.395,-1141.605;Inherit;False;ShaowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;131;-1197.751,223.0507;Inherit;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;283;-1103.037,-540.0648;Inherit;False;Property;_HighlightShaowIntensity;HighlightShaowIntensity;19;0;Create;True;0;0;0;False;0;False;0.3;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;150;-1436.325,777.5707;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;198;-1474.606,-878.8385;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;171;-1114.84,-440.4279;Inherit;False;170;FloorHighlight;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-1234.066,777.3354;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;282;-753.037,-484.765;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;182;-1287.532,-890.9621;Inherit;False;MulColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;116;-1025.85,227.2808;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-1101.792,-330.5651;Inherit;False;96;BaseColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-1120.403,-216.4476;Inherit;False;Property;_FloorHighlightIntesity;FloorHighlightIntesity;18;0;Create;True;0;0;0;False;0;False;1;9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-851.4619,-377.4208;Inherit;False;93;ShaowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;136;-838.4562,224.2216;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;281;-606.1368,-452.2648;Inherit;False;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;177;-835.7455,-272.5255;Inherit;False;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;183;-942.2606,-132.3633;Inherit;False;182;MulColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;154;-984.2825,65.56297;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-949.41,-43.93542;Inherit;False;Property;_MulColorIntensity;MulColorIntensity;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-4141.136,-370.0164;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;279;-637.3599,595.5148;Inherit;False;96;BaseColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;132;-632.2313,11.59759;Inherit;False;Property;_UseLmp;UseLmp;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;179;-634.2871,-136.6982;Inherit;False;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;295;-470.2725,-294.6503;Inherit;False;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;275;-664.5157,319.6331;Inherit;False;Property;_EmissionColor;EmissionColor;26;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;84;-3899.557,-388.571;Inherit;True;Property;_Keyword0;Keyword 0;1;0;Create;True;0;0;0;False;0;False;0;0;0;False;UNITY_PASS_FORWARDBASE;Toggle;2;Key0;Key1;Fetch;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;280;-429.1128,595.4011;Inherit;False;FLOAT;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;278;-634.2749,509.4202;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;27;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;-267.9897,332.5148;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;5;-387.5648,-135.387;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-3544.54,-391.6899;Inherit;False;LightColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;-235.0488,-357.7228;Inherit;True;78;LightColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;276;-121.1956,-95.35925;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;23.90306,-193.2225;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;105;330.4021,-125.9091;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;518.0461,-60.46124;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;107;783.2668,-5.645473;Inherit;False;float a = 2.51f@$float b = 0.03f@$float c = 2.43f@$float d = 0.59f@$float e = 0.14f@$return$saturate((LinearColor*(a*LinearColor+b))/(LinearColor*(c*LinearColor+d)+e))@;3;Create;1;True;LinearColor;FLOAT3;0,0,0;In;;Inherit;False;ACESTonemap;True;False;0;;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SqrtOpNode;108;1004.584,-0.395175;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;156;1156.664,28.83972;Inherit;False;Property;_CoverColor;CoverColor;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4245283,0.4245283,0.4245283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;157;1165.359,216.6189;Inherit;False;Property;_CoverColorIntensity;CoverColorIntensity;1;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;110;1179.547,-83.76404;Inherit;False;Property;_ACES;ACES;2;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;222;-8113.778,-1577.883;Inherit;False;1364.25;606.295;Comment;11;207;212;204;208;205;219;206;209;221;220;203;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-4354.889,-1641.443;Inherit;False;ViewDir;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;155;1489.427,-28.35395;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;203;-8063.778,-1172.33;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;209;-7303.692,-1309.622;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;221;-7082.414,-1297.088;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;206;-7534.429,-1503.883;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATanOpNode;205;-7692.429,-1339.882;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;36;-4567.021,-1644.783;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PiNode;207;-7779.429,-1527.883;Inherit;False;1;0;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-6911.528,-1297.866;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;219;-7438.315,-1183.588;Inherit;False;Property;_TilingOffset;TilingOffset;25;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;220;-7234.315,-1115.088;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;208;-7781.429,-1435.883;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;298;-1285.453,-1366.012;Inherit;False;Constant;_Float0;Float 0;29;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;299;-1260.082,-1184.084;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;204;-7833.932,-1342.395;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1799.105,-260.6817;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;ASE/HalfLambertWallWorldUV;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.06;0,0,0,1;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;262;0;163;3
WireConnection;164;0;262;0
WireConnection;164;1;163;2
WireConnection;233;0;232;1
WireConnection;233;1;232;2
WireConnection;199;0;164;0
WireConnection;238;0;233;0
WireConnection;231;0;243;1
WireConnection;231;3;199;0
WireConnection;251;0;252;3
WireConnection;251;3;238;0
WireConnection;250;0;231;0
WireConnection;250;1;251;0
WireConnection;259;0;260;1
WireConnection;259;1;260;2
WireConnection;261;0;260;3
WireConnection;261;1;260;4
WireConnection;236;0;250;0
WireConnection;236;1;259;0
WireConnection;263;0;236;0
WireConnection;263;1;261;0
WireConnection;267;0;269;1
WireConnection;267;1;269;2
WireConnection;257;0;263;0
WireConnection;268;0;269;3
WireConnection;268;1;269;4
WireConnection;239;0;257;0
WireConnection;264;0;250;0
WireConnection;264;1;267;0
WireConnection;71;0;70;0
WireConnection;71;1;69;1
WireConnection;265;0;264;0
WireConnection;265;1;268;0
WireConnection;188;0;185;0
WireConnection;188;1;200;0
WireConnection;266;0;265;0
WireConnection;72;0;71;0
WireConnection;285;0;284;1
WireConnection;285;1;284;2
WireConnection;270;0;266;0
WireConnection;286;0;284;3
WireConnection;286;1;284;4
WireConnection;74;0;72;0
WireConnection;74;1;72;1
WireConnection;62;1;188;0
WireConnection;287;0;250;0
WireConnection;287;1;285;0
WireConnection;22;0;21;0
WireConnection;73;0;74;0
WireConnection;73;1;72;2
WireConnection;288;0;287;0
WireConnection;288;1;286;0
WireConnection;63;0;3;0
WireConnection;63;1;62;0
WireConnection;56;0;73;0
WireConnection;96;0;63;0
WireConnection;289;0;288;0
WireConnection;194;0;169;0
WireConnection;194;1;202;0
WireConnection;290;0;289;0
WireConnection;1;3;24;0
WireConnection;176;1;194;0
WireConnection;119;0;120;0
WireConnection;168;1;194;0
WireConnection;173;0;168;0
WireConnection;173;1;176;0
WireConnection;173;2;178;0
WireConnection;77;0;57;0
WireConnection;77;1;1;0
WireConnection;151;0;119;0
WireConnection;112;0;77;0
WireConnection;112;1;113;0
WireConnection;293;0;292;0
WireConnection;293;1;291;0
WireConnection;296;1;173;0
WireConnection;296;2;297;1
WireConnection;60;1;188;0
WireConnection;180;1;293;0
WireConnection;114;0;112;0
WireConnection;114;1;115;0
WireConnection;121;0;122;0
WireConnection;121;1;151;0
WireConnection;121;2;127;0
WireConnection;61;0;4;0
WireConnection;61;1;60;0
WireConnection;143;0;152;0
WireConnection;143;1;144;0
WireConnection;143;2;145;0
WireConnection;170;0;296;0
WireConnection;93;0;61;0
WireConnection;131;0;114;0
WireConnection;131;1;121;0
WireConnection;150;0;143;0
WireConnection;198;0;196;0
WireConnection;198;2;180;1
WireConnection;137;0;150;0
WireConnection;137;1;138;0
WireConnection;282;0;171;0
WireConnection;282;1;283;0
WireConnection;182;0;198;0
WireConnection;116;0;131;0
WireConnection;136;0;116;0
WireConnection;136;1;137;0
WireConnection;281;0;282;0
WireConnection;281;1;94;0
WireConnection;281;2;174;0
WireConnection;177;0;171;0
WireConnection;177;1;97;0
WireConnection;177;2;174;0
WireConnection;154;0;114;0
WireConnection;132;1;154;0
WireConnection;132;0;136;0
WireConnection;179;0;183;0
WireConnection;179;1;177;0
WireConnection;179;2;184;0
WireConnection;295;0;183;0
WireConnection;295;1;281;0
WireConnection;295;2;184;0
WireConnection;84;1;71;0
WireConnection;84;0;85;0
WireConnection;280;0;279;0
WireConnection;274;0;275;0
WireConnection;274;1;278;0
WireConnection;274;2;280;0
WireConnection;5;0;295;0
WireConnection;5;1;179;0
WireConnection;5;2;132;0
WireConnection;78;0;84;0
WireConnection;276;0;5;0
WireConnection;276;1;274;0
WireConnection;86;0;79;0
WireConnection;86;1;276;0
WireConnection;105;0;86;0
WireConnection;106;0;105;0
WireConnection;106;1;105;0
WireConnection;107;0;106;0
WireConnection;108;0;107;0
WireConnection;110;1;105;0
WireConnection;110;0;108;0
WireConnection;37;0;36;0
WireConnection;155;0;110;0
WireConnection;155;1;156;0
WireConnection;155;2;157;0
WireConnection;209;0;206;0
WireConnection;209;1;203;2
WireConnection;221;0;209;0
WireConnection;221;1;220;0
WireConnection;206;0;205;0
WireConnection;206;1;207;0
WireConnection;206;2;208;0
WireConnection;205;0;204;0
WireConnection;212;0;221;0
WireConnection;212;1;219;0
WireConnection;220;0;219;3
WireConnection;220;1;219;4
WireConnection;204;0;203;1
WireConnection;204;1;203;3
WireConnection;0;13;155;0
ASEEND*/
//CHKSM=DEA5D49ABADF9A752B5CA5BCA5F471751A3A08D3
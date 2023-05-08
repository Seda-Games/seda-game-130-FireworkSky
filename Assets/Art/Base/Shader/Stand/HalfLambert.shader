// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/HalfLambert"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		[Toggle(_ACES_ON)] _ACES("ACES", Float) = 0
		_DiffuseTex("DiffuseTex", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_AO("AO", 2D) = "white" {}
		_AOIntensity("AOIntensity", Range( 0 , 1.5)) = 0
		_BaseColor("BaseColor", Color) = (0,0,0,0)
		_ShaowColor("ShaowColor", Color) = (0,0,0,0)
		_Mul("Mul", Float) = 1
		_Power("Power", Float) = 0
		[Toggle(_KKHIGHTLINGHT_ON)] _KKHightLinght("KKHightLinght", Float) = 0
		_HighlightColor("HighlightColor", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 10
		_FresnelColor("FresnelColor", Color) = (0,0,0,0)
		_FresnelScale("FresnelScale", Float) = 1
		_FresnelPower("FresnelPower", Float) = 0.5
		_EmissinColorIntensity("EmissinColorIntensity", Float) = 0
		_EmissionColor("EmissionColor", Color) = (0.5,0.5,0.5,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ACES_ON
		#pragma shader_feature_local _KKHIGHTLINGHT_ON
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
			float3 viewDir;
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

		uniform float4 _EmissionColor;
		uniform float _EmissinColorIntensity;
		uniform float4 _ShaowColor;
		uniform sampler2D _DiffuseTex;
		uniform float4 _DiffuseTex_ST;
		uniform float4 _BaseColor;
		uniform float4 _HighlightColor;
		uniform sampler2D _NormalMap;
		uniform float _Shininess;
		uniform float _Power;
		uniform float _Mul;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;
		uniform float _AOIntensity;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float4 _FresnelColor;


		float3 ACESTonemap120( float3 LinearColor )
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
			float3 temp_cast_1 = (1.0).xxx;
			#ifdef UNITY_PASS_FORWARDBASE
				float3 staticSwitch84 = temp_cast_1;
			#else
				float3 staticSwitch84 = temp_output_71_0;
			#endif
			float3 LightColor78 = staticSwitch84;
			float2 uv_DiffuseTex = i.uv_texcoord * _DiffuseTex_ST.xy + _DiffuseTex_ST.zw;
			float4 ShaowColor93 = ( _ShaowColor * tex2D( _DiffuseTex, uv_DiffuseTex ) );
			float4 BaseColor96 = ( _BaseColor * tex2D( _DiffuseTex, uv_DiffuseTex ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 temp_output_128_0 = ( ase_worldViewDir + ase_worldlightDir );
			float3 WorldNormal22 = normalize( (WorldNormalVector( i , UnpackNormal( tex2D( _NormalMap, i.uv_texcoord ) ) )) );
			float dotResult129 = dot( temp_output_128_0 , WorldNormal22 );
			float NdotH144 = dotResult129;
			float Shininess192 = _Shininess;
			float3 half_dir147 = temp_output_128_0;
			float dotResult150 = dot( half_dir147 , i.viewDir );
			float TdotH151 = dotResult150;
			float KKHightLinght184 = pow( ( 1.0 - ( TdotH151 * TdotH151 ) ) , Shininess192 );
			#ifdef _KKHIGHTLINGHT_ON
				float staticSwitch196 = KKHightLinght184;
			#else
				float staticSwitch196 = pow( NdotH144 , Shininess192 );
			#endif
			float4 HightLightColor142 = max( ( _HighlightColor * staticSwitch196 ) , float4( 0,0,0,0 ) );
			float3 break72 = temp_output_71_0;
			float LightAtten56 = max( max( break72.x , break72.y ) , break72.z );
			float dotResult5_g1 = dot( WorldNormal22 , ase_worldlightDir );
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			float4 lerpResult5 = lerp( ShaowColor93 , ( BaseColor96 + HightLightColor142 ) , max( ( LightAtten56 * ( pow( (dotResult5_g1*0.5 + 0.5) , _Power ) * _Mul ) ) , ( tex2D( _AO, uv_AO ).r * _AOIntensity ) ));
			float3 ViewDir37 = ase_worldViewDir;
			float fresnelNdotV6 = dot( WorldNormal22, ViewDir37 );
			float fresnelNode6 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV6, _FresnelPower ) );
			float4 FresnelColor99 = max( ( fresnelNode6 * _FresnelColor ) , float4( 0,0,0,0 ) );
			float4 temp_output_105_0 = max( ( float4( LightColor78 , 0.0 ) * ( lerpResult5 + FresnelColor99 ) ) , float4( 0,0,0,0 ) );
			float3 LinearColor120 = ( temp_output_105_0 * temp_output_105_0 ).rgb;
			float3 localACESTonemap120 = ACESTonemap120( LinearColor120 );
			#ifdef _ACES_ON
				float4 staticSwitch122 = float4( sqrt( localACESTonemap120 ) , 0.0 );
			#else
				float4 staticSwitch122 = temp_output_105_0;
			#endif
			c.rgb = staticSwitch122.rgb;
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
			o.Emission = ( _EmissionColor * _EmissinColorIntensity ).rgb;
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
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
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
Version=18100
1920;-122;1920;1019;1207.871;1109.567;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;143;-5664,704;Inherit;False;962.4656;525.8445;NdotH;7;129;125;144;147;128;126;124;NdotH;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;126;-5552,752;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;124;-5616,912;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;128;-5328,848;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;152;-5653.622,1384.725;Inherit;False;687.6201;324.3573;TdotH;4;151;150;148;149;TdotH;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;103;-4585.63,-1984.747;Inherit;False;1093.731;429.8099;WorldNormal&ViewDir;6;37;36;22;21;106;107;WorldNormal&ViewDir;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;147;-5168,816;Inherit;False;half_dir;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;149;-5604.683,1542.762;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;148;-5599.428,1434.725;Inherit;False;147;half_dir;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-4513.612,-1912.593;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;150;-5361.062,1461.363;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;106;-4290.708,-1937.33;Inherit;True;Property;_NormalMap;NormalMap;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-5179.92,1443.61;Inherit;True;TdotH;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;195;-5655.414,2671.994;Inherit;False;1124.438;362.2588;KKHightLinght;7;171;131;172;181;192;182;184;KKHightLinght;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;21;-3941.697,-1931.37;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;171;-5605.414,2721.994;Inherit;False;151;TdotH;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-3719.939,-1934.747;Inherit;False;WorldNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-5520,1120;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-5414.094,2724.902;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-5483.453,2896.804;Inherit;False;Property;_Shininess;Shininess;11;0;Create;True;0;0;False;0;False;10;10.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;89;-4625.276,-714.6766;Inherit;False;1513.141;690.5745;LightAtten;10;56;78;85;84;73;74;72;71;69;70;LightAtten;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;181;-5258.695,2779.687;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;192;-5305.018,2893.057;Inherit;False;Shininess;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;129;-5184,928;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-4928,928;Inherit;True;NdotH;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;123;-4584.706,105.3501;Inherit;False;1759.08;992.9066;HightLightColor;9;194;141;185;134;142;188;189;193;196;HightLightColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;182;-5029.452,2780.253;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;70;-4575.276,-423.8817;Inherit;True;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;69;-4558.988,-219.0229;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;193;-4448,736;Inherit;False;192;Shininess;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;-4502.556,505.6771;Inherit;True;144;NdotH;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-4305.532,-353.3372;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-4754.976,2774.233;Inherit;True;KKHightLinght;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;36;-3928.031,-1736.249;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;72;-3991.931,-334.3764;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PowerNode;189;-4176,576;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;185;-4147.64,824.9626;Inherit;False;184;KKHightLinght;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-3715.898,-1732.909;Inherit;False;ViewDir;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;98;-2507.264,-1365.481;Inherit;False;1160.431;445.9648;FresnelColor;8;8;44;26;99;6;39;40;101;FresnelColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2189.948,-59.39069;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;95;-3519.517,-1368.414;Inherit;False;919.8992;508.5779;BaseColor;4;3;63;62;96;BaseColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;74;-3752.028,-355.4842;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;134;-4064,320;Inherit;False;Property;_HighlightColor;HighlightColor;10;0;Create;True;0;0;False;0;False;1,1,1,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;196;-3899.406,570.5989;Inherit;False;Property;_KKHightLinght;KKHightLinght;9;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2457.264,-1130.71;Inherit;False;Property;_FresnelScale;FresnelScale;13;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-3443.39,-1318.414;Inherit;False;Property;_BaseColor;BaseColor;5;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-3664,464;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;62;-3477.179,-1102.064;Inherit;True;Property;_DiffuseTex;DiffuseTex;1;0;Create;True;0;0;False;0;False;-1;None;f2297f93d32996340a84adbc340bd584;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;1;-1981.792,-56.18768;Inherit;False;Half Lambert Term;-1;;1;86299dc21373a954aa5772333626c9c1;0;1;3;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;73;-3597.079,-323.1674;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;-2457.418,-1320.043;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2455.602,-1050.382;Inherit;False;Property;_FresnelPower;FresnelPower;14;0;Create;True;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;92;-4618.395,-1365.605;Inherit;False;1009.513;513.5418;ShaowColor;4;61;4;60;93;ShaowColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-1758.055,66.76774;Inherit;False;Property;_Power;Power;8;0;Create;True;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-2455.644,-1228.676;Inherit;False;37;ViewDir;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;8;-2147.497,-1106.283;Inherit;False;Property;_FresnelColor;FresnelColor;12;0;Create;True;0;0;False;0;False;0,0,0,0;0.01886791,0.01886791,0.01886791,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;6;-2197.005,-1322.963;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-3336.138,-321.4012;Inherit;False;LightAtten;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-3118.807,-1183.998;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;176;-1597.734,-46.47422;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-1549.383,62.94986;Inherit;False;Property;_Mul;Mul;7;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;194;-3408,480;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;60;-4570.395,-1125.605;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;62;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-4506.395,-1317.605;Inherit;False;Property;_ShaowColor;ShaowColor;6;0;Create;True;0;0;False;0;False;0,0,0,0;0.5849056,0.5849056,0.5849056,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;113;-1327.028,405.9467;Inherit;False;Property;_AOIntensity;AOIntensity;4;0;Create;True;0;0;False;0;False;0;0;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;-1418.717,-139.2886;Inherit;False;56;LightAtten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;-1417.055,-38.84019;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;-2889.51,-1170.921;Inherit;False;BaseColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;108;-1325.601,191.2921;Inherit;True;Property;_AO;AO;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-1873.27,-1326.32;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-4186.395,-1141.605;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;142;-3232,496;Inherit;True;HightLightColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-1102.341,-55.21618;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;-3978.395,-1141.605;Inherit;False;ShaowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;197;-1660.627,-1310.506;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-843.1385,-241.7497;Inherit;False;96;BaseColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-984.8791,206.4531;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-4144.927,-589.435;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-848.2965,-145.4405;Inherit;False;142;HightLightColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;-601.4885,-189.1315;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-646.9041,-285.6485;Inherit;False;93;ShaowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;84;-3903.346,-607.9892;Inherit;True;Property;_Keyword0;Keyword 0;1;0;Create;True;0;0;False;0;False;0;0;0;False;UNITY_PASS_FORWARDBASE;Toggle;2;Key0;Key1;Fetch;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-1532.179,-1324.755;Inherit;True;FresnelColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;115;-762.0526,31.42431;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-385.8315,19.32403;Inherit;True;99;FresnelColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-3548.329,-611.1083;Inherit;False;LightColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;5;-409.1598,-224.5454;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;104;-17.21011,-115.288;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;-130.2949,-335.5502;Inherit;True;78;LightColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;176.2521,-161.6673;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;105;409.5642,-165.098;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;572.507,-53.27158;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;120;721.9092,-41.35149;Inherit;False;float a = 2.51f@$float b = 0.03f@$float c = 2.43f@$float d = 0.59f@$float e = 0.14f@$return$saturate((LinearColor*(a*LinearColor+b))/(LinearColor*(c*LinearColor+d)+e))@;3;False;1;True;LinearColor;FLOAT3;0,0,0;In;;Inherit;False;ACESTonemap;True;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SqrtOpNode;121;943.2264,-36.1012;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;170;-5658.345,2289.064;Inherit;False;1292.018;276.6938;aniso;8;168;167;169;165;162;166;164;163;aniso;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;116;237.7594,-480.2416;Inherit;False;Property;_EmissionColor;EmissionColor;16;0;Create;True;0;0;False;0;False;0.5,0.5,0.5,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;118;228.7594,-297.2416;Inherit;False;Property;_EmissinColorIntensity;EmissinColorIntensity;15;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;160;-5654.601,1824.565;Inherit;False;859.9165;375.1273;NdotV;5;159;155;156;158;154;NdotV;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;158;-5200.65,1962.12;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;122;1108.934,-162.202;Inherit;False;Property;_ACES;ACES;0;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;167;-4989.073,2388.591;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;162;-4848.416,2394.235;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;165;-5412.244,2345.564;Inherit;False;Half Lambert Term;-1;;2;86299dc21373a954aa5772333626c9c1;0;1;3;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;154;-5585.17,1874.565;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;-5007.915,1960.234;Inherit;True;NdotV;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;486.7594,-403.2416;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;168;-4728.345,2397.064;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-5495.029,2454.709;Inherit;False;159;NdotV;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;-5608.345,2339.064;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;169;-4590.328,2392.021;Inherit;False;aniso;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;164;-5145.462,2383.064;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;156;-5359.538,1959.48;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;155;-5604.601,2086.371;Inherit;False;22;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1541.614,-422.5133;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;ASE/HalfLambert;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;128;0;126;0
WireConnection;128;1;124;0
WireConnection;147;0;128;0
WireConnection;150;0;148;0
WireConnection;150;1;149;0
WireConnection;106;1;107;0
WireConnection;151;0;150;0
WireConnection;21;0;106;0
WireConnection;22;0;21;0
WireConnection;172;0;171;0
WireConnection;172;1;171;0
WireConnection;181;0;172;0
WireConnection;192;0;131;0
WireConnection;129;0;128;0
WireConnection;129;1;125;0
WireConnection;144;0;129;0
WireConnection;182;0;181;0
WireConnection;182;1;192;0
WireConnection;71;0;70;0
WireConnection;71;1;69;1
WireConnection;184;0;182;0
WireConnection;72;0;71;0
WireConnection;189;0;188;0
WireConnection;189;1;193;0
WireConnection;37;0;36;0
WireConnection;74;0;72;0
WireConnection;74;1;72;1
WireConnection;196;1;189;0
WireConnection;196;0;185;0
WireConnection;141;0;134;0
WireConnection;141;1;196;0
WireConnection;1;3;24;0
WireConnection;73;0;74;0
WireConnection;73;1;72;2
WireConnection;6;0;44;0
WireConnection;6;4;26;0
WireConnection;6;2;39;0
WireConnection;6;3;40;0
WireConnection;56;0;73;0
WireConnection;63;0;3;0
WireConnection;63;1;62;0
WireConnection;176;0;1;0
WireConnection;176;1;175;0
WireConnection;194;0;141;0
WireConnection;178;0;176;0
WireConnection;178;1;177;0
WireConnection;96;0;63;0
WireConnection;101;0;6;0
WireConnection;101;1;8;0
WireConnection;61;0;4;0
WireConnection;61;1;60;0
WireConnection;142;0;194;0
WireConnection;77;0;57;0
WireConnection;77;1;178;0
WireConnection;93;0;61;0
WireConnection;197;0;101;0
WireConnection;109;0;108;1
WireConnection;109;1;113;0
WireConnection;91;0;97;0
WireConnection;91;1;90;0
WireConnection;84;1;71;0
WireConnection;84;0;85;0
WireConnection;99;0;197;0
WireConnection;115;0;77;0
WireConnection;115;1;109;0
WireConnection;78;0;84;0
WireConnection;5;0;94;0
WireConnection;5;1;91;0
WireConnection;5;2;115;0
WireConnection;104;0;5;0
WireConnection;104;1;100;0
WireConnection;86;0;79;0
WireConnection;86;1;104;0
WireConnection;105;0;86;0
WireConnection;119;0;105;0
WireConnection;119;1;105;0
WireConnection;120;0;119;0
WireConnection;121;0;120;0
WireConnection;158;0;156;0
WireConnection;122;1;105;0
WireConnection;122;0;121;0
WireConnection;167;0;164;0
WireConnection;162;0;167;0
WireConnection;165;3;166;0
WireConnection;159;0;158;0
WireConnection;117;0;116;0
WireConnection;117;1;118;0
WireConnection;168;0;162;0
WireConnection;169;0;168;0
WireConnection;164;0;165;0
WireConnection;164;1;163;0
WireConnection;156;0;154;0
WireConnection;156;1;155;0
WireConnection;0;2;117;0
WireConnection;0;13;122;0
ASEEND*/
//CHKSM=73437F63B8ADD279F753C3F0FC7C537A8E9ED6B4
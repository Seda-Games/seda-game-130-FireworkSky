// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ccc/water_3"
{
	Properties
	{
		_ReflectionTex("ReflectionTex", 2D) = "white" {}
		_WaterNormal("WaterNormal", 2D) = "white" {}
		_NormalIntensity("NormalIntensity", Float) = 1
		_NormalTilling("NormalTilling", Float) = 8
		_UnderWaterTilling("UnderWaterTilling", Float) = 8
		_WaterSpeed("WaterSpeed", Float) = 0
		_SpecSmoothess("SpecSmoothess", Range( 0.1 , 1)) = 0
		_SpecTint("SpecTint", Color) = (1,1,1,1)
		_SpecIntensity("SpecIntensity", Float) = 0
		_SpecEnd("SpecEnd", Float) = 0
		_SpecStart("SpecStart", Float) = 0
		_UnderWater("UnderWater", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform sampler2D _UnderWater;
		uniform float _UnderWaterTilling;
		uniform sampler2D _WaterNormal;
		uniform float _NormalTilling;
		uniform float _WaterSpeed;
		uniform sampler2D _ReflectionTex;
		uniform float _NormalIntensity;
		uniform float _SpecSmoothess;
		uniform float4 _SpecTint;
		uniform float _SpecIntensity;
		uniform float _SpecEnd;
		uniform float _SpecStart;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_7_0 = ( (ase_worldPos).xz / _NormalTilling );
			float temp_output_13_0 = ( _Time.y * 0.1 * _WaterSpeed );
			float2 temp_output_31_0 = ( (( tex2D( _WaterNormal, ( temp_output_7_0 + temp_output_13_0 ) ) + float4( UnpackNormal( tex2D( _WaterNormal, ( ( temp_output_7_0 * 1.5 ) + ( temp_output_13_0 * -1.0 ) ) ) ) , 0.0 ) )).rg * 0.5 );
			float dotResult34 = dot( temp_output_31_0 , temp_output_31_0 );
			float3 appendResult38 = (float3(temp_output_31_0 , sqrt( ( 1.0 - dotResult34 ) )));
			float3 WaterNormal40 = (WorldNormalVector( i , appendResult38 ));
			float2 temp_output_105_0 = ( ( (ase_worldPos).xz / _UnderWaterTilling ) + ( (WaterNormal40).xy * 0.1 ) );
			float4 UnderWaterColor95 = tex2D( _UnderWater, temp_output_105_0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 unityObjectToClipPos47 = UnityObjectToClipPos( ase_vertex3Pos );
			float4 ReflectColor54 = tex2D( _ReflectionTex, ( (ase_screenPosNorm).xy + ( ( (WaterNormal40).xz / ( 1.0 + unityObjectToClipPos47.w ) ) * _NormalIntensity ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult99 = dot( ase_worldNormal , ase_worldViewDir );
			float clampResult102 = clamp( dotResult99 , 0.0 , 1.0 );
			float4 lerpResult104 = lerp( UnderWaterColor95 , ReflectColor54 , ( 1.0 - clampResult102 ));
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult58 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float dotResult60 = dot( WaterNormal40 , normalizeResult58 );
			float clampResult86 = clamp( ( ( _SpecEnd - distance( ase_worldPos , _WorldSpaceCameraPos ) ) / ( _SpecEnd - _SpecStart ) ) , 0.0 , 1.0 );
			float4 SpecColor74 = ( ( ( pow( max( dotResult60 , 0.0 ) , ( _SpecSmoothess * 256.0 ) ) * _SpecTint ) * _SpecIntensity ) * clampResult86 );
			o.Emission = ( lerpResult104 + SpecColor74 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows 

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
				float4 screenPos : TEXCOORD1;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
2324;-69;1440;1013;5486.462;-2776.603;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;41;-5604.091,-1242.506;Inherit;False;3737.353;1059.832;WaterNormal;26;16;6;7;9;4;15;11;14;13;24;8;28;27;22;29;31;34;35;32;18;36;38;39;40;44;45;WaterNormal;0.2018868,0.5826798,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-5554.091,-1192.506;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;14;-5332.52,-442.3736;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;11;-5407.921,-561.9745;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-5290.921,-298.0746;Inherit;False;Property;_WaterSpeed;WaterSpeed;5;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-5326.161,-986.696;Inherit;False;Property;_NormalTilling;NormalTilling;3;0;Create;True;0;0;False;0;False;8;11.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;6;-5182.009,-1183.113;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-5133.618,-594.4749;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-5028.286,-428.6711;Inherit;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-5311.141,-871.1052;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;False;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-4989.248,-1128.315;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-4924.57,-954.1169;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-4941.286,-572.6711;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-4763.688,-700.0942;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-4633.812,-1079.922;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-4496.152,-1094.351;Inherit;True;Property;_WaterNormal;WaterNormal;1;0;Create;True;0;0;False;0;False;-1;None;b41b18a4565f29241bb50153ce7e988b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-4615.464,-754.637;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;None;ee3441bafac7f4147858d2cfb278e6a2;True;0;False;bump;Auto;True;Instance;4;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-4077.309,-910.103;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-3784.886,-713.8059;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;18;-3770.384,-855.0677;Inherit;False;FLOAT2;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-3501.331,-811.6642;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;34;-3278.475,-689.8936;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-3087.604,-678.0649;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;36;-2870.014,-732.6226;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-2587.014,-762.6227;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;75;-5742.209,1323.712;Inherit;False;3232.441;1430.372;SpecColor;26;82;74;87;86;72;69;85;73;65;84;81;70;83;79;63;67;78;66;80;60;68;61;58;56;57;55;SpecColor;0.5413136,0.8773585,0.812958,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;55;-5620.574,1492.659;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;57;-5692.209,1653;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;39;-2342.32,-743.6231;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;51;-5413.393,240.5395;Inherit;False;2515.021;812.5394;ReflectColor;14;17;2;3;21;1;20;46;49;43;50;47;42;48;54;ReflectColor;1,0.7867925,0.7867925,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-2091.536,-705.0146;Inherit;False;WaterNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-5291.747,1547.628;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;48;-5363.393,856.0681;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;61;-5093.506,1373.712;Inherit;False;40;WaterNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;58;-5041.031,1554.9;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-5253.752,496.7688;Inherit;False;40;WaterNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;47;-5139.293,835.8269;Inherit;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;114;-6221.376,2866.76;Inherit;False;2258.718;1183.544;UnderWaterColor;15;106;90;92;91;107;109;108;93;111;112;105;110;113;88;95;UnderWaterColor;1,0.9553353,0.4,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-4948.447,736.0671;Inherit;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;43;-5029.752,519.7688;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;106;-5997.449,3300.047;Inherit;False;40;WaterNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;90;-6029.783,2953.187;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;78;-5562.206,2130.981;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-4757.602,828.598;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;80;-5615.66,2357.744;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;68;-4616.406,1829.993;Inherit;False;Constant;_Float5;Float 5;6;0;Create;True;0;0;False;0;False;256;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-4792.857,1700.528;Inherit;False;Property;_SpecSmoothess;SpecSmoothess;6;0;Create;True;0;0;False;0;False;0;0.1;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;60;-4784.864,1481.175;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;2;-4597.161,347.7118;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;107;-5729.449,3291.047;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;92;-5757.869,2968.712;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;63;-4547.5,1524.081;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-4569.634,708.2587;Inherit;False;Property;_NormalIntensity;NormalIntensity;2;0;Create;True;0;0;False;0;False;1;6.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-5737.352,3412.293;Inherit;False;Constant;_Float6;Float 6;12;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-4431.604,1766.305;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;79;-5248.035,2220.989;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-5088.172,2167.021;Inherit;False;Property;_SpecEnd;SpecEnd;9;0;Create;True;0;0;False;0;False;0;300;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-5039.229,2459.207;Inherit;False;Property;_SpecStart;SpecStart;10;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-6025.826,3110.567;Inherit;False;Property;_UnderWaterTilling;UnderWaterTilling;4;0;Create;True;0;0;False;0;False;8;67.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;46;-4665.071,559.6802;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;65;-4301.099,1547.05;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-5515.868,3270.762;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;93;-5440.102,2978.744;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;81;-4865.712,2243.047;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;70;-4229.857,1697.718;Inherit;False;Property;_SpecTint;SpecTint;7;0;Create;True;0;0;False;0;False;1,1,1,1;0.1786668,0.5188679,0.2920555,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-4120.401,515.7056;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;-4862.774,2407.74;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;3;-4236.961,341.4124;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-3956.023,333.3208;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-3954.319,1778.362;Inherit;False;Property;_SpecIntensity;SpecIntensity;8;0;Create;True;0;0;False;0;False;0;0.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;100;-1704.831,137.6286;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-5183.421,3085.67;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;85;-4608.378,2276.867;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-4005.395,1567.344;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;97;-1703.736,-126.1592;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;86;-4381.924,2306.277;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;88;-4612.574,2916.76;Inherit;True;Property;_UnderWater;UnderWater;11;0;Create;True;0;0;False;0;False;-1;None;c6f92f968154a49d4b66775d0e27070a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-3641.377,290.5395;Inherit;True;Property;_ReflectionTex;ReflectionTex;0;0;Create;True;0;0;False;0;False;-1;None;e893982f069b3f94fb12346936e62f66;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;99;-1419.82,-35.39642;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-3733.771,1699.542;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;102;-1156.852,31.30446;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;95;-4203.458,2975.016;Inherit;False;UnderWaterColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-3473.115,1816.61;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;54;-3209.679,335.3625;Inherit;False;ReflectColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;103;-916.1614,74.53513;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-933.3203,-59.03208;Inherit;False;54;ReflectColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;-2971.367,1895.366;Inherit;False;SpecColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-953.3654,-195.7953;Inherit;False;95;UnderWaterColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-716.5511,207.0632;Inherit;False;74;SpecColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;104;-655.4695,-58.35038;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-4833.057,3185.871;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;111;-5690.558,3864.705;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-474.2092,165.7225;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;112;-5672.558,3741.705;Inherit;False;Property;_WaterDetph;WaterDetph;12;0;Create;True;0;0;False;0;False;0;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;110;-5208.458,3320.101;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;ccc/water_3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;16;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;13;2;15;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;27;0;7;0
WireConnection;27;1;28;0
WireConnection;45;0;13;0
WireConnection;45;1;44;0
WireConnection;24;0;27;0
WireConnection;24;1;45;0
WireConnection;9;0;7;0
WireConnection;9;1;13;0
WireConnection;4;1;9;0
WireConnection;22;1;24;0
WireConnection;29;0;4;0
WireConnection;29;1;22;0
WireConnection;18;0;29;0
WireConnection;31;0;18;0
WireConnection;31;1;32;0
WireConnection;34;0;31;0
WireConnection;34;1;31;0
WireConnection;35;0;34;0
WireConnection;36;0;35;0
WireConnection;38;0;31;0
WireConnection;38;2;36;0
WireConnection;39;0;38;0
WireConnection;40;0;39;0
WireConnection;56;0;55;0
WireConnection;56;1;57;0
WireConnection;58;0;56;0
WireConnection;47;0;48;0
WireConnection;43;0;42;0
WireConnection;49;0;50;0
WireConnection;49;1;47;4
WireConnection;60;0;61;0
WireConnection;60;1;58;0
WireConnection;107;0;106;0
WireConnection;92;0;90;0
WireConnection;63;0;60;0
WireConnection;67;0;66;0
WireConnection;67;1;68;0
WireConnection;79;0;78;0
WireConnection;79;1;80;0
WireConnection;46;0;43;0
WireConnection;46;1;49;0
WireConnection;65;0;63;0
WireConnection;65;1;67;0
WireConnection;108;0;107;0
WireConnection;108;1;109;0
WireConnection;93;0;92;0
WireConnection;93;1;91;0
WireConnection;81;0;82;0
WireConnection;81;1;79;0
WireConnection;20;0;46;0
WireConnection;20;1;21;0
WireConnection;84;0;82;0
WireConnection;84;1;83;0
WireConnection;3;0;2;0
WireConnection;17;0;3;0
WireConnection;17;1;20;0
WireConnection;105;0;93;0
WireConnection;105;1;108;0
WireConnection;85;0;81;0
WireConnection;85;1;84;0
WireConnection;69;0;65;0
WireConnection;69;1;70;0
WireConnection;86;0;85;0
WireConnection;88;1;105;0
WireConnection;1;1;17;0
WireConnection;99;0;97;0
WireConnection;99;1;100;0
WireConnection;72;0;69;0
WireConnection;72;1;73;0
WireConnection;102;0;99;0
WireConnection;95;0;88;0
WireConnection;87;0;72;0
WireConnection;87;1;86;0
WireConnection;54;0;1;0
WireConnection;103;0;102;0
WireConnection;74;0;87;0
WireConnection;104;0;96;0
WireConnection;104;1;52;0
WireConnection;104;2;103;0
WireConnection;113;0;105;0
WireConnection;113;1;110;0
WireConnection;76;0;104;0
WireConnection;76;1;77;0
WireConnection;110;1;112;0
WireConnection;110;2;111;0
WireConnection;0;2;76;0
ASEEND*/
//CHKSM=4A9F6BE29085E8D417D3975B0C2D0BBA2C580B60
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/Sky"
{
	Properties
	{
		[NoScaleOffset]_Tex("Cubemap", CUBE) = "black" {}
		[HideInInspector]_Tex_HDR("DecodeInstructions", Vector) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Background"  "Queue" = "Background+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  "PreviewType"="Skybox" }
		Cull Off
		ZWrite Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float3 vertexToFrag774;
			float3 worldPos;
		};

		uniform half4 _Tex_HDR;
		uniform samplerCUBE _Tex;


		inline half3 DecodeHDR1189( float4 Data )
		{
			return DecodeHDR(Data, _Tex_HDR);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 appendResult1129 = (float3(ase_worldPos.x , ase_worldPos.y , ase_worldPos.z));
			float3 normalizeResult1130 = normalize( appendResult1129 );
			o.vertexToFrag774 = normalizeResult1130;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			half4 Data1189 = texCUBE( _Tex, i.vertexToFrag774 );
			half3 localDecodeHDR1189 = DecodeHDR1189( Data1189 );
			half4 CUBEMAP222 = ( float4( localDecodeHDR1189 , 0.0 ) * unity_ColorSpaceDouble * half4(0.5,0.5,0.5,1) );
			float3 ase_worldPos = i.worldPos;
			float3 normalizeResult319 = normalize( ase_worldPos );
			float lerpResult678 = lerp( saturate( pow( (0.0 + (abs( normalizeResult319.y ) - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) , 1.0 ) ) , 0.0 , (unity_FogColor).a);
			half FOG_MASK359 = lerpResult678;
			float4 lerpResult317 = lerp( unity_FogColor , CUBEMAP222 , FOG_MASK359);
			o.Emission = lerpResult317.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
7;1;1906;1010;1792.803;136.9509;1.452156;True;True
Node;AmplifyShaderEditor.CommentaryNode;700;-2440.435,577.1061;Inherit;False;2085.962;467.6702;Fog Coords on Screen;13;678;1110;316;677;315;1108;314;1109;320;319;318;359;1195;BUILT-IN FOG;0,0.4980392,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1191;-2429.143,-57.81717;Inherit;False;1782.183;570.3788;Base;11;222;1174;774;238;1130;1129;1192;1173;1189;1175;41;;0,0.4980392,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;318;-2392.435,625.1055;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;238;-2396.895,-5.679482;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;319;-2136.434,625.1055;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;1129;-2217.314,19.32654;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;320;-1944.434,625.1055;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.NormalizeNode;1130;-2089.314,19.32654;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;1108;-1624.435,753.1059;Half;False;Constant;_Float39;Float 39;55;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;314;-1624.435,625.1055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1109;-1624.435,849.1049;Half;False;Constant;_Float40;Float 40;55;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexToFragmentNode;774;-1945.364,19.12537;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;315;-1432.435,625.1055;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;677;-1176.434,625.1055;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1719.426,-7.817242;Inherit;True;Property;_Tex;Cubemap;0;1;[NoScaleOffset];Create;False;0;0;False;0;-1;None;a9f053d430424adb925523ba78342596;True;0;False;black;LockedToCube;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;1194;-1253.676,916.4751;Inherit;False;unity_FogColor;0;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1110;-984.4345,817.1061;Half;False;Constant;_Float41;Float 41;55;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;1195;-982.6761,906.4751;Inherit;False;FLOAT;3;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;1189;-1335.426,-7.817242;Half;False;DecodeHDR(Data, _Tex_HDR);3;False;1;True;Data;FLOAT4;0,0,0,0;In;;Float;False;DecodeHDR;True;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorSpaceDouble;1175;-1335.426,72.18285;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;316;-984.4345,625.1055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1173;-1330.6,234.9128;Half;False;Constant;_TintColor;Tint Color;0;1;[Gamma];Create;True;0;0;False;1;Header(Cubemap);0.5,0.5,0.5,1;0.5,0.5,0.5,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;678;-728.4346,625.1055;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1174;-1014.426,-8.817243;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;222;-861.2274,-14.87335;Half;False;CUBEMAP;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1167;-521.9852,59.82798;Inherit;False;618;357;;4;436;228;312;317;FINAL COLOR;0.4980392,1,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;359;-574.0104,621.8488;Half;False;FOG_MASK;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;312;-471.9858,109.8279;Inherit;False;unity_FogColor;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;228;-471.9858,221.828;Inherit;False;222;CUBEMAP;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;436;-471.9858,301.8281;Inherit;False;359;FOG_MASK;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;317;-87.98634,109.8279;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;1192;-1680.642,219.6802;Half;False;Property;_Tex_HDR;DecodeInstructions;2;1;[HideInInspector];Create;False;0;0;True;0;0,0,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;26;128,64;Float;False;True;-1;0;ASEMaterialInspector;0;0;Unlit;Mobile/Sky;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;True;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0;True;False;0;True;Background;;Background;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;1;PreviewType=Skybox;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;319;0;318;0
WireConnection;1129;0;238;1
WireConnection;1129;1;238;2
WireConnection;1129;2;238;3
WireConnection;320;0;319;0
WireConnection;1130;0;1129;0
WireConnection;314;0;320;1
WireConnection;774;0;1130;0
WireConnection;315;0;314;0
WireConnection;315;1;1108;0
WireConnection;315;3;1108;0
WireConnection;315;4;1109;0
WireConnection;677;0;315;0
WireConnection;41;1;774;0
WireConnection;1195;0;1194;0
WireConnection;1189;0;41;0
WireConnection;316;0;677;0
WireConnection;678;0;316;0
WireConnection;678;1;1110;0
WireConnection;678;2;1195;0
WireConnection;1174;0;1189;0
WireConnection;1174;1;1175;0
WireConnection;1174;2;1173;0
WireConnection;222;0;1174;0
WireConnection;359;0;678;0
WireConnection;317;0;312;0
WireConnection;317;1;228;0
WireConnection;317;2;436;0
WireConnection;26;2;317;0
ASEEND*/
//CHKSM=F6DCCDB60D2C951CD57B6B4E5C776987E565D887
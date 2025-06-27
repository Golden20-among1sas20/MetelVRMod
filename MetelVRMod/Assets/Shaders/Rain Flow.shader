// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/Rain Flow"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		_CubeMap("CubeMap", CUBE) = "white" {}
		_RainDrops("Rain Drops", 2D) = "bump" {}
		_RainFlow("Rain Flow", 2D) = "bump" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Lambert keepalpha noshadow nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
			float2 uv2_texcoord2;
			float3 worldNormal;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform samplerCUBE _CubeMap;
		uniform sampler2D _RainFlow;
		uniform sampler2D _RainDrops;

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float2 temp_cast_0 = (20.0).xx;
			float2 uv2_TexCoord13 = i.uv2_texcoord2 * temp_cast_0;
			float4 appendResult26 = (float4(frac( uv2_TexCoord13.x ) , frac( uv2_TexCoord13.y ) , 0.0 , 0.0));
			float temp_output_4_0_g18 = 8.0;
			float temp_output_5_0_g18 = 8.0;
			float2 appendResult7_g18 = (float2(temp_output_4_0_g18 , temp_output_5_0_g18));
			float totalFrames39_g18 = ( temp_output_4_0_g18 * temp_output_5_0_g18 );
			float2 appendResult8_g18 = (float2(totalFrames39_g18 , temp_output_5_0_g18));
			float temp_output_25_0 = ( _Time.y * 20.0 );
			float clampResult42_g18 = clamp( 0.0 , 0.0001 , ( totalFrames39_g18 - 1.0 ) );
			float temp_output_35_0_g18 = frac( ( ( temp_output_25_0 + clampResult42_g18 ) / totalFrames39_g18 ) );
			float2 appendResult29_g18 = (float2(temp_output_35_0_g18 , ( 1.0 - temp_output_35_0_g18 )));
			float2 temp_output_15_0_g18 = ( ( appendResult26.xy / appendResult7_g18 ) + ( floor( ( appendResult8_g18 * appendResult29_g18 ) ) / appendResult7_g18 ) );
			float2 temp_cast_2 = (40.0).xx;
			float2 uv_TexCoord14 = i.uv_texcoord * temp_cast_2;
			float2 appendResult27 = (float2(frac( uv_TexCoord14.x ) , frac( uv_TexCoord14.y )));
			float temp_output_4_0_g19 = 4.0;
			float temp_output_5_0_g19 = 4.0;
			float2 appendResult7_g19 = (float2(temp_output_4_0_g19 , temp_output_5_0_g19));
			float totalFrames39_g19 = ( temp_output_4_0_g19 * temp_output_5_0_g19 );
			float2 appendResult8_g19 = (float2(totalFrames39_g19 , temp_output_5_0_g19));
			float clampResult42_g19 = clamp( 0.0 , 0.0001 , ( totalFrames39_g19 - 1.0 ) );
			float temp_output_35_0_g19 = frac( ( ( temp_output_25_0 + clampResult42_g19 ) / totalFrames39_g19 ) );
			float2 appendResult29_g19 = (float2(temp_output_35_0_g19 , ( 1.0 - temp_output_35_0_g19 )));
			float2 temp_output_15_0_g19 = ( ( appendResult27 / appendResult7_g19 ) + ( floor( ( appendResult8_g19 * appendResult29_g19 ) ) / appendResult7_g19 ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_cast_3 = (saturate( ( ase_worldNormal.y * 1.0 ) )).xxx;
			float3 lerpResult51 = lerp( normalize( WorldReflectionVector( i , UnpackNormal( tex2D( _RainFlow, temp_output_15_0_g18 ) ) ) ) , normalize( WorldReflectionVector( i , UnpackNormal( tex2D( _RainDrops, temp_output_15_0_g19 ) ) ) ) , saturate( ( (WorldNormalVector( i , temp_cast_3 )).y * 1.0 ) ));
			float4 lerpResult8 = lerp( tex2D( _Diffuse, uv_Diffuse ) , texCUBE( _CubeMap, lerpResult51 ) , ( 0.3 * i.vertexColor.r ));
			o.Albedo = lerpResult8.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
0;0;1920;1019;3788.585;631.634;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;61;-3297.15,-108.7647;Inherit;False;Constant;_TileFlow;Tile Flow;3;0;Create;True;0;0;0;False;0;False;20;0.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-3347.126,-592.0613;Inherit;False;Constant;_TileDrops;Tile Drops;2;0;Create;True;0;0;0;False;0;False;40;3.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-3103.243,-107.7604;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;45;-3100.233,50.38963;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-3115.768,-622.6687;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-3187.926,255.0846;Float;False;Constant;_Amount;Amount;3;0;Create;True;0;0;0;False;0;False;1;0.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2858.832,97.28957;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;17;-2814.993,-58.48139;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;16;-2964.168,-490.8687;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2949.268,-364.5687;Inherit;False;Constant;_Speed;Speed;8;0;Create;True;0;0;0;False;0;False;20;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;21;-2815.086,-125.0424;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;22;-2827.518,-573.3896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;19;-2827.611,-639.9507;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-2698.133,105.6896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;27;-2692.525,-646.8296;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-2696,-76.92133;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-2707.068,-456.8687;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-2470,-428.3217;Inherit;False;Flipbook;-1;;18;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;8;False;5;FLOAT;8;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.WorldNormalVector;48;-2279.832,175.888;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;30;-2489.674,-655.6156;Inherit;False;Flipbook;-1;;19;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;4;False;5;FLOAT;4;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.SamplerNode;34;-2160.444,-482.2827;Inherit;True;Property;_RainFlow;Rain Flow;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;1;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-2066.127,253.8847;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-2166.315,-666.8677;Inherit;True;Property;_RainDrops;Rain Drops;2;0;Create;True;0;0;0;False;0;False;-1;None;dc1c6439556de0347b193757ca1960b3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;50;-1911.032,259.9873;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;60;-1629.691,69.52086;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldReflectionVector;7;-1404.479,-44.72183;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VertexColorNode;64;-539.309,367.1635;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;51;-1139.561,172.7838;Inherit;False;3;0;FLOAT3;0,0,0.5;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;2;-667.9609,181.8167;Inherit;True;Property;_CubeMap;CubeMap;1;0;Create;True;0;0;0;False;0;False;-1;None;56a68e301a0ff55469ae441c0112d256;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-666.9609,-5.183258;Inherit;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;0;False;0;False;-1;None;138df4511c079324cabae1f7f865c1c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-327.1804,299.9077;Inherit;False;2;2;0;FLOAT;0.3;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;8;-153.9611,115.8167;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-5,115;Float;False;True;-1;0;ASEMaterialInspector;0;0;Lambert;Mobile/Rain Flow;False;False;False;False;False;False;True;True;True;False;True;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;61;0
WireConnection;14;0;10;0
WireConnection;47;0;45;2
WireConnection;47;1;46;0
WireConnection;17;0;13;2
WireConnection;21;0;13;1
WireConnection;22;0;14;2
WireConnection;19;0;14;1
WireConnection;53;0;47;0
WireConnection;27;0;19;0
WireConnection;27;1;22;0
WireConnection;26;0;21;0
WireConnection;26;1;17;0
WireConnection;25;0;16;0
WireConnection;25;1;20;0
WireConnection;32;13;26;0
WireConnection;32;2;25;0
WireConnection;48;0;53;0
WireConnection;30;13;27;0
WireConnection;30;2;25;0
WireConnection;34;1;32;0
WireConnection;49;0;48;2
WireConnection;49;1;46;0
WireConnection;35;1;30;0
WireConnection;50;0;49;0
WireConnection;60;0;34;0
WireConnection;7;0;35;0
WireConnection;51;0;60;0
WireConnection;51;1;7;0
WireConnection;51;2;50;0
WireConnection;2;1;51;0
WireConnection;62;1;64;1
WireConnection;8;0;1;0
WireConnection;8;1;2;0
WireConnection;8;2;62;0
WireConnection;0;0;8;0
ASEEND*/
//CHKSM=183D905A4D22D67BE513CFD62E8B65A90A56EDA4
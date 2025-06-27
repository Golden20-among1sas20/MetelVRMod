// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/Landscape High"
{
	Properties
	{
		_SplatMap("Splat Map", 2D) = "white" {}
		_TextureBase("Texture (Base)", 2D) = "white" {}
		_TextureR("Texture (R)", 2D) = "white" {}
		_TextureG("Texture (G)", 2D) = "white" {}
		_TextureB("Texture (B)", 2D) = "white" {}
		_CubeMap1("CubeMap", CUBE) = "white" {}
		_RainDropsNormal1("Rain Drops Normal", 2D) = "bump" {}
		_Tile("Tile", Float) = 300
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Lambert keepalpha noshadow nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform sampler2D _SplatMap;
		uniform float4 _SplatMap_ST;
		uniform sampler2D _TextureBase;
		uniform float4 _TextureBase_ST;
		uniform sampler2D _TextureR;
		uniform float4 _TextureR_ST;
		uniform sampler2D _TextureG;
		uniform float4 _TextureG_ST;
		uniform sampler2D _TextureB;
		uniform float4 _TextureB_ST;
		uniform samplerCUBE _CubeMap1;
		uniform sampler2D _RainDropsNormal1;
		uniform float _Tile;

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_SplatMap = i.uv_texcoord * _SplatMap_ST.xy + _SplatMap_ST.zw;
			float4 tex2DNode330 = tex2D( _SplatMap, uv_SplatMap );
			float2 uv_TextureBase = i.uv_texcoord * _TextureBase_ST.xy + _TextureBase_ST.zw;
			float2 uv_TextureR = i.uv_texcoord * _TextureR_ST.xy + _TextureR_ST.zw;
			float2 uv_TextureG = i.uv_texcoord * _TextureG_ST.xy + _TextureG_ST.zw;
			float2 uv_TextureB = i.uv_texcoord * _TextureB_ST.xy + _TextureB_ST.zw;
			float3 layeredBlendVar322 = (tex2DNode330).rgb;
			float4 layeredBlend322 = ( lerp( lerp( lerp( tex2D( _TextureBase, uv_TextureBase ) , tex2D( _TextureR, uv_TextureR ) , layeredBlendVar322.x ) , tex2D( _TextureG, uv_TextureG ) , layeredBlendVar322.y ) , tex2D( _TextureB, uv_TextureB ) , layeredBlendVar322.z ) );
			float2 temp_cast_0 = (_Tile).xx;
			float2 uv_TexCoord387 = i.uv_texcoord * temp_cast_0;
			float4 appendResult393 = (float4(frac( uv_TexCoord387.x ) , frac( uv_TexCoord387.y ) , 0.0 , 0.0));
			float temp_output_4_0_g22 = 4.0;
			float temp_output_5_0_g22 = 4.0;
			float2 appendResult7_g22 = (float2(temp_output_4_0_g22 , temp_output_5_0_g22));
			float totalFrames39_g22 = ( temp_output_4_0_g22 * temp_output_5_0_g22 );
			float2 appendResult8_g22 = (float2(totalFrames39_g22 , temp_output_5_0_g22));
			float temp_output_392_0 = ( _Time.y * 20.0 );
			float clampResult42_g22 = clamp( 0.0 , 0.0001 , ( totalFrames39_g22 - 1.0 ) );
			float temp_output_35_0_g22 = frac( ( ( temp_output_392_0 + clampResult42_g22 ) / totalFrames39_g22 ) );
			float2 appendResult29_g22 = (float2(temp_output_35_0_g22 , ( 1.0 - temp_output_35_0_g22 )));
			float2 temp_output_15_0_g22 = ( ( appendResult393.xy / appendResult7_g22 ) + ( floor( ( appendResult8_g22 * appendResult29_g22 ) ) / appendResult7_g22 ) );
			float2 temp_cast_2 = (( _Tile / 0.9 )).xx;
			float2 uv_TexCoord371 = i.uv_texcoord * temp_cast_2 + float2( -0.67,0.14 );
			float4 appendResult378 = (float4(frac( uv_TexCoord371.x ) , frac( uv_TexCoord371.y ) , 0.0 , 0.0));
			float temp_output_4_0_g21 = 4.0;
			float temp_output_5_0_g21 = 4.0;
			float2 appendResult7_g21 = (float2(temp_output_4_0_g21 , temp_output_5_0_g21));
			float totalFrames39_g21 = ( temp_output_4_0_g21 * temp_output_5_0_g21 );
			float2 appendResult8_g21 = (float2(totalFrames39_g21 , temp_output_5_0_g21));
			float clampResult42_g21 = clamp( 0.0 , 0.0001 , ( totalFrames39_g21 - 1.0 ) );
			float temp_output_35_0_g21 = frac( ( ( temp_output_392_0 + clampResult42_g21 ) / totalFrames39_g21 ) );
			float2 appendResult29_g21 = (float2(temp_output_35_0_g21 , ( 1.0 - temp_output_35_0_g21 )));
			float2 temp_output_15_0_g21 = ( ( appendResult378.xy / appendResult7_g21 ) + ( floor( ( appendResult8_g21 * appendResult29_g21 ) ) / appendResult7_g21 ) );
			float3 ase_worldReflection = normalize( WorldReflectionVector( i, float3( 0, 0, 1 ) ) );
			float3 ase_worldPos = i.worldPos;
			float clampResult381 = clamp( pow( ( distance( _WorldSpaceCameraPos , ase_worldPos ) / 30.0 ) , 0.13 ) , 0.0 , 1.0 );
			float3 lerpResult383 = lerp( normalize( WorldReflectionVector( i , BlendNormals( UnpackNormal( tex2D( _RainDropsNormal1, temp_output_15_0_g22 ) ) , UnpackNormal( tex2D( _RainDropsNormal1, temp_output_15_0_g21 ) ) ) ) ) , ase_worldReflection , clampResult381);
			float4 lerpResult369 = lerp( layeredBlend322 , texCUBE( _CubeMap1, lerpResult383 ) , tex2DNode330.a);
			o.Albedo = lerpResult369.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
0;0;2560;1019;1151.12;-986.4072;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;385;-424.3734,1261.034;Inherit;False;Property;_Tile;Tile;7;0;Create;True;0;0;0;False;0;False;300;300;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;386;-375.1295,1613.288;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;370;-480.1295,1826.288;Inherit;False;Constant;_Offcet1;Offcet;7;0;Create;True;0;0;0;False;0;False;-0.67,0.14;-0.67,0.14;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;387;-196.6024,1173.034;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;371;-230.0775,1755.942;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.6,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;389;91.55458,1155.752;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;375;58.0795,1738.66;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;390;-76.10237,1499.134;Inherit;False;Constant;_Speed1;Speed;7;0;Create;True;0;0;0;False;0;False;20;24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;388;-91.0024,1372.834;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;391;91.6476,1222.313;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;373;58.17252,1805.221;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;374;404.0765,1924.816;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;372;341.0306,1797.516;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;392;166.0976,1406.834;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;378;177.1656,1786.781;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;393;210.6406,1203.873;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;394;390.5722,1017.073;Inherit;True;Property;_RainDropsNormal1;Rain Drops Normal;6;0;Create;True;0;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DistanceOpNode;376;606.6622,1799.271;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;395;383.492,1208.087;Inherit;False;Flipbook;-1;;22;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;4;False;5;FLOAT;4;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.FunctionNode;396;403.1656,1435.381;Inherit;False;Flipbook;-1;;21;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;4;False;5;FLOAT;4;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.RangedFloatNode;377;607.7518,1890.933;Inherit;False;Constant;_A;A;7;0;Create;True;0;0;0;False;0;False;30;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;398;706.8503,1196.835;Inherit;True;Property;_RainDrops1;Rain Drops;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;397;712.7217,1381.42;Inherit;True;Property;_RainDrops2;Rain Drops;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;399;604.2132,1967.148;Inherit;False;Constant;_B;B;8;0;Create;True;0;0;0;False;0;False;0.13;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;379;824.7574,1805.391;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;380;966.5024,1804.642;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;382;1101.823,1472.208;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldReflectionVector;401;1302.537,1635.382;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldReflectionVector;400;1300.677,1469.274;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;381;1133.541,1798.079;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;330;1894.107,903.6689;Inherit;True;Property;_SplatMap;Splat Map;0;0;Create;True;0;0;0;False;0;False;-1;None;8276ff1f6d104ac4289bf3f0482df4f0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;334;1909.15,1482.841;Inherit;True;Property;_TextureG;Texture (G);3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;328;2183.998,919.5095;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;335;1910.428,1666.36;Inherit;True;Property;_TextureB;Texture (B);4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;383;1832.231,2053.392;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;333;1908.15,1296.841;Inherit;True;Property;_TextureR;Texture (R);2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;331;1905.15,1107.841;Inherit;True;Property;_TextureBase;Texture (Base);1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LayeredBlendNode;322;2432.998,1086.51;Inherit;False;6;0;FLOAT3;1,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;3,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;366;2170.341,2000.511;Inherit;True;Property;_CubeMap1;CubeMap;5;0;Create;True;0;0;0;False;0;False;-1;None;56a68e301a0ff55469ae441c0112d256;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;369;2684.34,1934.511;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2865.839,1935.681;Float;False;True;-1;0;ASEMaterialInspector;0;0;Lambert;Mobile/Landscape High;False;False;False;False;False;False;True;True;True;False;True;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;386;0;385;0
WireConnection;387;0;385;0
WireConnection;371;0;386;0
WireConnection;371;1;370;0
WireConnection;389;0;387;1
WireConnection;375;0;371;1
WireConnection;391;0;387;2
WireConnection;373;0;371;2
WireConnection;392;0;388;0
WireConnection;392;1;390;0
WireConnection;378;0;375;0
WireConnection;378;1;373;0
WireConnection;393;0;389;0
WireConnection;393;1;391;0
WireConnection;376;0;372;0
WireConnection;376;1;374;0
WireConnection;395;13;393;0
WireConnection;395;2;392;0
WireConnection;396;13;378;0
WireConnection;396;2;392;0
WireConnection;398;0;394;0
WireConnection;398;1;395;0
WireConnection;397;0;394;0
WireConnection;397;1;396;0
WireConnection;379;0;376;0
WireConnection;379;1;377;0
WireConnection;380;0;379;0
WireConnection;380;1;399;0
WireConnection;382;0;398;0
WireConnection;382;1;397;0
WireConnection;400;0;382;0
WireConnection;381;0;380;0
WireConnection;328;0;330;0
WireConnection;383;0;400;0
WireConnection;383;1;401;0
WireConnection;383;2;381;0
WireConnection;322;0;328;0
WireConnection;322;1;331;0
WireConnection;322;2;333;0
WireConnection;322;3;334;0
WireConnection;322;4;335;0
WireConnection;366;1;383;0
WireConnection;369;0;322;0
WireConnection;369;1;366;0
WireConnection;369;2;330;4
WireConnection;0;0;369;0
ASEEND*/
//CHKSM=78CC5DFDC02517AE8E728A0A80DC45C75597C0D7
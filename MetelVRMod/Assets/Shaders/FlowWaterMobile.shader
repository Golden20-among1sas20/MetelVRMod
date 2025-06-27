// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/FlowWater"
{
	Properties
	{
		_FlowMap("FlowMap", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_FlowMapTile("FlowMap Tile", Float) = 1
		_SpeedFlow("SpeedFlow", Float) = 0
		_Texture("Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform sampler2D _Texture;
			uniform sampler2D _NormalMap;
			uniform sampler2D _FlowMap;
			uniform float4 _FlowMap_ST;
			uniform float _SpeedFlow;
			uniform float _FlowMapTile;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float2 uv015 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv_FlowMap = i.ase_texcoord.xy * _FlowMap_ST.xy + _FlowMap_ST.zw;
				float2 blendOpSrc16 = uv015;
				float2 blendOpDest16 = (tex2D( _FlowMap, uv_FlowMap )).rg;
				float2 temp_output_16_0 = ( saturate( (( blendOpDest16 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest16 ) * ( 1.0 - blendOpSrc16 ) ) : ( 2.0 * blendOpDest16 * blendOpSrc16 ) ) ));
				float temp_output_1_0_g21 = _Time.y;
				float temp_output_13_0 = (0.0 + (( ( temp_output_1_0_g21 - floor( ( temp_output_1_0_g21 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
				float TimeA18 = ( -temp_output_13_0 * _SpeedFlow );
				float2 lerpResult17 = lerp( uv015 , temp_output_16_0 , TimeA18);
				float2 temp_cast_0 = (_FlowMapTile).xx;
				float2 uv028 = i.ase_texcoord.xy * temp_cast_0 + float2( 0,0 );
				float2 DiffuseTilling30 = uv028;
				float2 FlowA21 = ( lerpResult17 + DiffuseTilling30 );
				float temp_output_1_0_g20 = (_Time.y*1.0 + 0.5);
				float TimeB40 = ( _SpeedFlow * -(0.0 + (( ( temp_output_1_0_g20 - floor( ( temp_output_1_0_g20 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) );
				float2 lerpResult42 = lerp( uv015 , temp_output_16_0 , TimeB40);
				float2 FlowB45 = ( lerpResult42 + DiffuseTilling30 );
				float BlendTime55 = saturate( abs( ( 1.0 - ( temp_output_13_0 / 0.5 ) ) ) );
				float3 lerpResult48 = lerp( UnpackNormal( tex2D( _NormalMap, FlowA21 ) ) , UnpackNormal( tex2D( _NormalMap, FlowB45 ) ) , BlendTime55);
				float2 uv088 = i.ase_texcoord.xy * float2( 1,1 ) + lerpResult48.xy;
				
				
				finalColor = tex2D( _Texture, uv088 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17500
7;29;1906;1004;2590.919;1063.669;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;34;-2777.794,475.8869;Inherit;False;1843.356;505.3228;TimeA;18;10;55;54;53;52;50;40;18;39;14;38;13;37;9;41;76;77;78;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;10;-2746.897,659.7414;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;41;-2396.965,750.2093;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;37;-2169.712,760.4675;Inherit;True;Sawtooth Wave;-1;;20;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;9;-2174.531,529.3252;Inherit;True;Sawtooth Wave;-1;;21;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-1954.973,760.6719;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;13;-1959.79,529.5296;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-1947.711,692.3625;Inherit;False;Property;_SpeedFlow;SpeedFlow;3;0;Create;True;0;0;False;0;0;54.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;39;-1769.108,759.4692;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;14;-1773.925,528.3269;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;32;-2776.305,-25.03501;Inherit;False;1527.906;464.0924;FlowMap;13;45;21;35;44;36;17;43;42;19;16;15;3;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;31;-2777.093,-228.7703;Inherit;False;696.1582;170;Normal Map Tilling;3;30;28;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2727.093,-159.5209;Inherit;False;Property;_FlowMapTile;FlowMap Tile;2;0;Create;True;0;0;False;0;1;-6.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-1616.711,521.3625;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-2749.958,240.459;Inherit;True;Property;_FlowMap;FlowMap;0;0;Create;True;0;0;False;0;-1;None;106562a03a0d57642aabce7f4800550a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-1598.711,749.3625;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-2450.433,125.118;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1400.29,519.8869;Inherit;False;TimeA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;3;-2431.675,240.6426;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-2551.935,-176.7701;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-1411.473,764.0292;Inherit;False;TimeB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-2174.483,320.8578;Inherit;False;18;TimeA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;16;-2188.297,193.6082;Inherit;False;Overlay;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-2314.935,-178.7701;Inherit;False;DiffuseTilling;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-2402.778,55.52084;Inherit;False;40;TimeB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;50;-1757.061,629.4644;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;-1801.901,20.81235;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;17;-1804.372,147.5751;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;52;-1595.535,629.6423;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-1841.686,270.4883;Inherit;False;30;DiffuseTilling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-1583.363,146.1172;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-1609.953,21.83234;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;53;-1434.334,625.7422;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;89;-2771.453,-809.2381;Inherit;False;1565.617;557.298;Normal Map;9;24;23;47;46;8;56;48;88;87;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-1457.97,70.10095;Inherit;False;FlowB;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;-1457.081,140.3542;Inherit;False;FlowA;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;54;-1303.035,625.7425;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-2713.435,-660.5546;Inherit;False;45;FlowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;23;-2721.453,-554.311;Inherit;True;Property;_NormalMap;Normal Map;1;0;Create;True;0;0;False;0;None;dce0715949bfa364a93b9a91654c680a;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-1156.134,619.2425;Inherit;False;BlendTime;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2714.494,-732.4909;Inherit;False;21;FlowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-2359.418,-743.4208;Inherit;True;Property;_def;def;1;0;Create;True;0;0;False;0;-1;None;9fbef4b79ca3b784ba023cb1331520d5;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;-2355.658,-556.0735;Inherit;True;Property;_def2;def2;1;0;Create;True;0;0;False;0;-1;None;9fbef4b79ca3b784ba023cb1331520d5;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;56;-2259.646,-366.9404;Inherit;False;55;BlendTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-1935.262,-683.0022;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;88;-1766.32,-729.5748;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;87;-1526.835,-759.2381;Inherit;True;Property;_Texture;Texture;4;0;Create;True;0;0;False;0;-1;None;80ab37a9e4f49c842903bb43bdd7bcd2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;93;-957.7462,-752.7656;Float;False;True;-1;2;ASEMaterialInspector;100;1;Mobile/FlowWater;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;41;0;10;0
WireConnection;37;1;41;0
WireConnection;9;1;10;0
WireConnection;38;0;37;0
WireConnection;13;0;9;0
WireConnection;39;0;38;0
WireConnection;14;0;13;0
WireConnection;77;0;14;0
WireConnection;77;1;78;0
WireConnection;76;0;78;0
WireConnection;76;1;39;0
WireConnection;18;0;77;0
WireConnection;3;0;1;0
WireConnection;28;0;27;0
WireConnection;40;0;76;0
WireConnection;16;0;15;0
WireConnection;16;1;3;0
WireConnection;30;0;28;0
WireConnection;50;0;13;0
WireConnection;42;0;15;0
WireConnection;42;1;16;0
WireConnection;42;2;43;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;17;2;19;0
WireConnection;52;0;50;0
WireConnection;35;0;17;0
WireConnection;35;1;36;0
WireConnection;44;0;42;0
WireConnection;44;1;36;0
WireConnection;53;0;52;0
WireConnection;45;0;44;0
WireConnection;21;0;35;0
WireConnection;54;0;53;0
WireConnection;55;0;54;0
WireConnection;8;0;23;0
WireConnection;8;1;24;0
WireConnection;46;0;23;0
WireConnection;46;1;47;0
WireConnection;48;0;8;0
WireConnection;48;1;46;0
WireConnection;48;2;56;0
WireConnection;88;1;48;0
WireConnection;87;1;88;0
WireConnection;93;0;87;0
ASEEND*/
//CHKSM=EACCDF04EB5F9DEA8990D1B6F4EEC69A108DED94
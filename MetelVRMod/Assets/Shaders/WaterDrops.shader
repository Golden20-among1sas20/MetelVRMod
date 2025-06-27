// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/WaterDrops"
{
	Properties
	{
		_Texture("Texture", 2D) = "black" {}
		_Reflection("Reflection", CUBE) = "white" {}
		_WaterNormal("Water Normal", 2D) = "bump" {}
		_Drops("Drops", 2D) = "bump" {}
		_Columns("Columns", Float) = 0
		_Rows("Rows", Float) = 0
		_AnimationSpeed("Animation Speed", Range( 0 , 50)) = 0

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
			#include "UnityStandardUtils.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform samplerCUBE _Reflection;
			uniform sampler2D _WaterNormal;
			uniform float4 _WaterNormal_ST;
			uniform sampler2D _Drops;
			uniform float _Columns;
			uniform float _Rows;
			uniform float _AnimationSpeed;
			uniform sampler2D _Texture;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord.xyz = ase_worldNormal;
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldTangent = UnityObjectToWorldDir(v.ase_tangent);
				o.ase_texcoord3.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord4.xyz = ase_worldBitangent;
				
				o.ase_texcoord2.xyz = v.ase_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
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
				float3 ase_worldNormal = i.ase_texcoord.xyz;
				float3 ase_worldPos = i.ase_texcoord1.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldReflection = reflect(-ase_worldViewDir, ase_worldNormal);
				float3 normalizedWorldRefl = normalize( ase_worldReflection );
				float2 uv0_WaterNormal = i.ase_texcoord2.xyz * _WaterNormal_ST.xy + _WaterNormal_ST.zw;
				float2 panner22 = ( 1.0 * _Time.y * float2( -0.03,0 ) + uv0_WaterNormal);
				float2 panner19 = ( 1.0 * _Time.y * float2( 0.04,0.04 ) + uv0_WaterNormal);
				float2 uv0187 = i.ase_texcoord2.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult190 = (float2(frac( uv0187.x ) , frac( uv0187.y )));
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles191 = _Columns * _Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset191 = 1.0f / _Columns;
				float fbrowsoffset191 = 1.0f / _Rows;
				// Speed of animation
				float fbspeed191 = _Time[ 1 ] * _AnimationSpeed;
				// UV Tiling (col and row offset)
				float2 fbtiling191 = float2(fbcolsoffset191, fbrowsoffset191);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex191 = round( fmod( fbspeed191 + 0.0, fbtotaltiles191) );
				fbcurrenttileindex191 += ( fbcurrenttileindex191 < 0) ? fbtotaltiles191 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox191 = round ( fmod ( fbcurrenttileindex191, _Columns ) );
				// Multiply Offset X by coloffset
				float fboffsetx191 = fblinearindextox191 * fbcolsoffset191;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy191 = round( fmod( ( fbcurrenttileindex191 - fblinearindextox191 ) / _Columns, _Rows ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy191 = (int)(_Rows-1) - fblinearindextoy191;
				// Multiply Offset Y by rowoffset
				float fboffsety191 = fblinearindextoy191 * fbrowsoffset191;
				// UV Offset
				float2 fboffset191 = float2(fboffsetx191, fboffsety191);
				// Flipbook UV
				half2 fbuv191 = appendResult190 * fbtiling191 + fboffset191;
				// *** END Flipbook UV Animation vars ***
				float3 temp_output_195_0 = BlendNormals( BlendNormals( UnpackScaleNormal( tex2D( _WaterNormal, panner22 ), 0.5 ) , UnpackScaleNormal( tex2D( _WaterNormal, panner19 ), 0.5 ) ) , UnpackNormal( tex2D( _Drops, fbuv191 ) ) );
				float3 ase_worldTangent = i.ase_texcoord3.xyz;
				float3 ase_worldBitangent = i.ase_texcoord4.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 worldRefl178 = normalize( reflect( -ase_worldViewDir, float3( dot( tanToWorld0, temp_output_195_0 ), dot( tanToWorld1, temp_output_195_0 ), dot( tanToWorld2, temp_output_195_0 ) ) ) );
				float3 lerpResult201 = lerp( normalizedWorldRefl , worldRefl178 , 0.1);
				float2 uv0199 = i.ase_texcoord2.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float3 lerpResult198 = lerp( float3( uv0199 ,  0.0 ) , temp_output_195_0 , 0.1);
				float4 tex2DNode186 = tex2D( _Texture, lerpResult198.xy );
				float4 lerpResult183 = lerp( texCUBE( _Reflection, lerpResult201 ) , tex2DNode186 , tex2DNode186.a);
				
				
				finalColor = lerpResult183;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17500
7;92;1906;938;333.5595;1285.634;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;187;-715.9552,-623.4794;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;151;-935.9057,-1082.484;Inherit;False;1281.603;457.1994;Blend panning normals to fake noving ripples;6;19;23;24;21;22;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FractNode;189;-449.1687,-614.6682;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-885.9058,-1005.185;Inherit;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;188;-446.6886,-551.8369;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;192;-583.7351,-275.8963;Float;False;Property;_AnimationSpeed;Animation Speed;6;0;Create;True;0;0;False;0;0;0;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;19;-610.9061,-919.9849;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.04,0.04;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-472.7526,-415.6818;Inherit;False;Property;_Rows;Rows;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;190;-303.9464,-594.1177;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;196;-461.7526,-487.6818;Inherit;False;Property;_Columns;Columns;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;22;-613.2062,-1032.484;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.03,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;23;-268.2061,-1024.185;Inherit;True;Property;_Normal2;Normal2;2;0;Create;True;0;0;False;0;-1;None;None;True;0;True;bump;Auto;True;Instance;17;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.5;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-256.3054,-815.2847;Inherit;True;Property;_WaterNormal;Water Normal;2;0;Create;True;0;0;False;0;-1;None;e9742c575b8f4644fb9379e7347ff62e;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.5;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;191;-183.9137,-568.0141;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;16;False;2;FLOAT;8;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;193;80.09137,-598.2202;Inherit;True;Property;_Drops;Drops;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;24;170.697,-880.858;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;195;437.2446,-693.7894;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;199;442.8253,-837.676;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;200;692.8253,-599.676;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;203;548.8387,-1180.653;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldReflectionVector;178;633.8405,-1032.468;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;198;885.8253,-817.676;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;201;871.9606,-1056.003;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;179;1039.65,-1188.063;Inherit;True;Property;_Reflection;Reflection;1;0;Create;True;0;0;False;0;-1;None;56a68e301a0ff55469ae441c0112d256;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;186;1055.007,-998.063;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;183;1632.007,-1152.063;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;185;1786.665,-1151.805;Float;False;True;-1;2;ASEMaterialInspector;100;1;Mobile/WaterDrops;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;189;0;187;1
WireConnection;188;0;187;2
WireConnection;19;0;21;0
WireConnection;190;0;189;0
WireConnection;190;1;188;0
WireConnection;22;0;21;0
WireConnection;23;1;22;0
WireConnection;17;1;19;0
WireConnection;191;0;190;0
WireConnection;191;1;196;0
WireConnection;191;2;197;0
WireConnection;191;3;192;0
WireConnection;193;1;191;0
WireConnection;24;0;23;0
WireConnection;24;1;17;0
WireConnection;195;0;24;0
WireConnection;195;1;193;0
WireConnection;178;0;195;0
WireConnection;198;0;199;0
WireConnection;198;1;195;0
WireConnection;198;2;200;0
WireConnection;201;0;203;0
WireConnection;201;1;178;0
WireConnection;201;2;200;0
WireConnection;179;1;201;0
WireConnection;186;1;198;0
WireConnection;183;0;179;0
WireConnection;183;1;186;0
WireConnection;183;2;186;4
WireConnection;185;0;183;0
ASEEND*/
//CHKSM=1A6175F4E47D799173FC982DA1C4BA7197F3A9F9
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/CustomCharacter"
{
	Properties
	{
		_Color_Primary("Color_Primary", Color) = (0.2431373,0.4196079,0.6196079,0)
		_Color_Secondary("Color_Secondary", Color) = (0.8196079,0.6431373,0.2980392,0)
		_Color_Leather_Primary("Color_Leather_Primary", Color) = (0.282353,0.2078432,0.1647059,0)
		_Color_Metal_Primary("Color_Metal_Primary", Color) = (0.5960785,0.6117647,0.627451,0)
		_Color_Leather_Secondary("Color_Leather_Secondary", Color) = (0.372549,0.3294118,0.2784314,0)
		_Color_Metal_Dark("Color_Metal_Dark", Color) = (0.1764706,0.1960784,0.2156863,0)
		_Color_Metal_Secondary("Color_Metal_Secondary", Color) = (0.345098,0.3764706,0.3960785,0)
		_Color_Hair("Color_Hair", Color) = (0.2627451,0.2117647,0.1333333,0)
		_Color_Skin("Color_Skin", Color) = (1,0.8000001,0.682353,1)
		_Color_Stubble("Color_Stubble", Color) = (0.8039216,0.7019608,0.6313726,1)
		_Color_Scar("Color_Scar", Color) = (0.9294118,0.6862745,0.5921569,1)
		_Color_BodyArt("Color_BodyArt", Color) = (0.2283196,0.5822246,0.7573529,1)
		[HideInInspector]_Texture_Color_Metal_Primary("Texture_Color_Metal_Primary", 2D) = "white" {}
		_Texture("Texture", 2D) = "white" {}
		[HideInInspector]_Texture_Base_Secondary("Texture_Base_Secondary", 2D) = "white" {}
		[HideInInspector]_Texture_Metal_Secondary("Texture_Metal_Secondary", 2D) = "white" {}
		[HideInInspector]_Texture_Color_Metal_Dark("Texture_Color_Metal_Dark", 2D) = "white" {}
		[HideInInspector]_Texture_BodyArt("Texture_BodyArt", 2D) = "white" {}
		[HideInInspector]_Mask_Primary("Mask_Primary", 2D) = "white" {}
		[HideInInspector]_Mask_Secondary("Mask_Secondary", 2D) = "white" {}
		[HideInInspector]_Texture_Base_Primary("Texture_Base_Primary", 2D) = "white" {}
		[HideInInspector]_Texture_Hair("Texture_Hair", 2D) = "white" {}
		[HideInInspector]_Texture_Skin("Texture_Skin", 2D) = "white" {}
		[HideInInspector]_Texture_Stubble("Texture_Stubble", 2D) = "white" {}
		[HideInInspector]_Texture_Scar("Texture_Scar", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_BodyArt_Amount("BodyArt_Amount", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color_BodyArt;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float4 _Color_Primary;
		uniform sampler2D _Mask_Primary;
		uniform float4 _Mask_Primary_ST;
		uniform float4 _Color_Secondary;
		uniform sampler2D _Mask_Secondary;
		uniform float4 _Mask_Secondary_ST;
		uniform float4 _Color_Leather_Primary;
		uniform sampler2D _Texture_Base_Primary;
		uniform float4 _Texture_Base_Primary_ST;
		uniform float4 _Color_Leather_Secondary;
		uniform sampler2D _Texture_Base_Secondary;
		uniform float4 _Texture_Base_Secondary_ST;
		uniform float4 _Color_Metal_Primary;
		uniform sampler2D _Texture_Color_Metal_Primary;
		uniform float4 _Texture_Color_Metal_Primary_ST;
		uniform float4 _Color_Metal_Secondary;
		uniform sampler2D _Texture_Metal_Secondary;
		uniform float4 _Texture_Metal_Secondary_ST;
		uniform float4 _Color_Metal_Dark;
		uniform sampler2D _Texture_Color_Metal_Dark;
		uniform float4 _Texture_Color_Metal_Dark_ST;
		uniform float4 _Color_Hair;
		uniform sampler2D _Texture_Hair;
		uniform float4 _Texture_Hair_ST;
		uniform float4 _Color_Skin;
		uniform sampler2D _Texture_Skin;
		uniform float4 _Texture_Skin_ST;
		uniform float4 _Color_Stubble;
		uniform sampler2D _Texture_Stubble;
		uniform float4 _Texture_Stubble_ST;
		uniform float4 _Color_Scar;
		uniform sampler2D _Texture_Scar;
		uniform float4 _Texture_Scar_ST;
		uniform sampler2D _Texture_BodyArt;
		uniform float4 _Texture_BodyArt_ST;
		uniform float _BodyArt_Amount;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float2 uv_Mask_Primary = i.uv_texcoord * _Mask_Primary_ST.xy + _Mask_Primary_ST.zw;
			float temp_output_25_0_g2 = 0.5;
			float temp_output_22_0_g2 = step( tex2D( _Mask_Primary, uv_Mask_Primary, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g2 );
			float4 lerpResult35 = lerp( tex2D( _Texture, uv_Texture, float2( 0,0 ), float2( 0,0 ) ) , _Color_Primary , temp_output_22_0_g2);
			float2 uv_Mask_Secondary = i.uv_texcoord * _Mask_Secondary_ST.xy + _Mask_Secondary_ST.zw;
			float temp_output_25_0_g3 = 0.5;
			float temp_output_22_0_g3 = step( tex2D( _Mask_Secondary, uv_Mask_Secondary, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g3 );
			float4 lerpResult41 = lerp( lerpResult35 , _Color_Secondary , temp_output_22_0_g3);
			float2 uv_Texture_Base_Primary = i.uv_texcoord * _Texture_Base_Primary_ST.xy + _Texture_Base_Primary_ST.zw;
			float temp_output_25_0_g4 = 0.5;
			float temp_output_22_0_g4 = step( tex2D( _Texture_Base_Primary, uv_Texture_Base_Primary, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g4 );
			float4 lerpResult45 = lerp( lerpResult41 , _Color_Leather_Primary , temp_output_22_0_g4);
			float2 uv_Texture_Base_Secondary = i.uv_texcoord * _Texture_Base_Secondary_ST.xy + _Texture_Base_Secondary_ST.zw;
			float temp_output_25_0_g9 = 0.5;
			float temp_output_22_0_g9 = step( tex2D( _Texture_Base_Secondary, uv_Texture_Base_Secondary, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g9 );
			float4 lerpResult65 = lerp( lerpResult45 , _Color_Leather_Secondary , temp_output_22_0_g9);
			float2 uv_Texture_Color_Metal_Primary = i.uv_texcoord * _Texture_Color_Metal_Primary_ST.xy + _Texture_Color_Metal_Primary_ST.zw;
			float temp_output_25_0_g10 = 0.5;
			float temp_output_22_0_g10 = step( tex2D( _Texture_Color_Metal_Primary, uv_Texture_Color_Metal_Primary, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g10 );
			float4 lerpResult124 = lerp( lerpResult65 , _Color_Metal_Primary , temp_output_22_0_g10);
			float2 uv_Texture_Metal_Secondary = i.uv_texcoord * _Texture_Metal_Secondary_ST.xy + _Texture_Metal_Secondary_ST.zw;
			float temp_output_25_0_g11 = 0.5;
			float temp_output_22_0_g11 = step( tex2D( _Texture_Metal_Secondary, uv_Texture_Metal_Secondary, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g11 );
			float4 lerpResult132 = lerp( lerpResult124 , _Color_Metal_Secondary , temp_output_22_0_g11);
			float2 uv_Texture_Color_Metal_Dark = i.uv_texcoord * _Texture_Color_Metal_Dark_ST.xy + _Texture_Color_Metal_Dark_ST.zw;
			float temp_output_25_0_g12 = 0.5;
			float temp_output_22_0_g12 = step( tex2D( _Texture_Color_Metal_Dark, uv_Texture_Color_Metal_Dark, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g12 );
			float4 lerpResult140 = lerp( lerpResult132 , _Color_Metal_Dark , temp_output_22_0_g12);
			float2 uv_Texture_Hair = i.uv_texcoord * _Texture_Hair_ST.xy + _Texture_Hair_ST.zw;
			float temp_output_25_0_g14 = 0.5;
			float temp_output_22_0_g14 = step( tex2D( _Texture_Hair, uv_Texture_Hair, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g14 );
			float4 lerpResult49 = lerp( lerpResult140 , _Color_Hair , temp_output_22_0_g14);
			float2 uv_Texture_Skin = i.uv_texcoord * _Texture_Skin_ST.xy + _Texture_Skin_ST.zw;
			float temp_output_25_0_g15 = 0.5;
			float temp_output_22_0_g15 = step( tex2D( _Texture_Skin, uv_Texture_Skin, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g15 );
			float4 lerpResult53 = lerp( lerpResult49 , _Color_Skin , temp_output_22_0_g15);
			float2 uv_Texture_Stubble = i.uv_texcoord * _Texture_Stubble_ST.xy + _Texture_Stubble_ST.zw;
			float temp_output_25_0_g16 = 0.5;
			float temp_output_22_0_g16 = step( tex2D( _Texture_Stubble, uv_Texture_Stubble, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g16 );
			float4 lerpResult57 = lerp( lerpResult53 , _Color_Stubble , temp_output_22_0_g16);
			float2 uv_Texture_Scar = i.uv_texcoord * _Texture_Scar_ST.xy + _Texture_Scar_ST.zw;
			float temp_output_25_0_g18 = 0.5;
			float temp_output_22_0_g18 = step( tex2D( _Texture_Scar, uv_Texture_Scar, float2( 0,0 ), float2( 0,0 ) ).r , temp_output_25_0_g18 );
			float4 lerpResult61 = lerp( lerpResult57 , _Color_Scar , temp_output_22_0_g18);
			float2 uv_Texture_BodyArt = i.uv_texcoord * _Texture_BodyArt_ST.xy + _Texture_BodyArt_ST.zw;
			float4 color151 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 lerpResult152 = lerp( tex2D( _Texture_BodyArt, uv_Texture_BodyArt, float2( 0,0 ), float2( 0,0 ) ) , color151 , _BodyArt_Amount);
			float4 lerpResult69 = lerp( _Color_BodyArt , lerpResult61 , lerpResult152);
			o.Albedo = lerpResult69.rgb;
			float temp_output_154_0 = _Smoothness;
			o.Metallic = temp_output_154_0;
			o.Smoothness = temp_output_154_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
2582;118;1839;1304;-4162.431;437.3278;1;True;False
Node;AmplifyShaderEditor.SamplerNode;31;-273.2831,-7.288284;Float;True;Property;_Mask_Primary;Mask_Primary;18;1;[HideInInspector];Create;True;0;0;True;0;0cbc66454a3a56244affa05554162bac;0cbc66454a3a56244affa05554162bac;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;86;19.2157,-41.26794;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;32;179.0768,-1.25197;Float;True;Property;_Mask_Secondary;Mask_Secondary;19;1;[HideInInspector];Create;True;0;0;True;0;a3f6d4c76177b704299c630a6bf515f1;a3f6d4c76177b704299c630a6bf515f1;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;42;639.5076,-2.321199;Float;True;Property;_Texture_Base_Primary;Texture_Base_Primary;20;1;[HideInInspector];Create;True;0;0;True;0;ebf19c917bd322643a829ec6095a2178;ebf19c917bd322643a829ec6095a2178;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;88;474.2157,-50.26794;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;85;-323.7843,-54.26794;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;62;1041.422,1.21092;Float;True;Property;_Texture_Base_Secondary;Texture_Base_Secondary;14;1;[HideInInspector];Create;True;0;0;True;0;f65b920b34d4fc248a504cef1ed177d8;f65b920b34d4fc248a504cef1ed177d8;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;94;931.2158,-44.26788;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;34;-279.8109,-167.2174;Float;False;MaskingFunction;-1;;2;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;87;145.2157,-61.26794;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;127;1476.681,8.992779;Float;True;Property;_Texture_Color_Metal_Primary;Texture_Color_Metal_Primary;12;1;[HideInInspector];Create;True;0;0;True;0;8c6f956b63336ae44ad322451df5a7a2;8c6f956b63336ae44ad322451df5a7a2;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;93;579.2158,-59.26788;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;99;1340.216,-45.26788;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;91;57.2157,-200.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;39;186.6764,-155.1506;Float;False;MaskingFunction;-1;;3;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.SamplerNode;134;1908.691,10.39542;Float;True;Property;_Texture_Metal_Secondary;Texture_Metal_Secondary;15;1;[HideInInspector];Create;True;0;0;True;0;59d30079bf12f254dbd2470a093f97a6;59d30079bf12f254dbd2470a093f97a6;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;33;-281.0458,-343.9193;Float;False;Property;_Color_Primary;Color_Primary;0;0;Create;True;0;0;False;0;0.2431373,0.4196079,0.6196079,0;0.7205882,0.4397069,0.190744,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;90;518.2157,-189.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;92;-20.7843,-252.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;43;630.107,-150.5898;Float;False;MaskingFunction;-1;;4;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;100;1010.216,-59.26788;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;129;1760.771,-28.48636;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;37;-612.9012,-407.6611;Float;True;Property;_Texture;Texture;13;0;Create;True;0;0;False;0;8c67aa4b531f4c74786135793df0a695;8c67aa4b531f4c74786135793df0a695;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;64;1047.021,-152.0576;Float;False;MaskingFunction;-1;;9;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.SamplerNode;142;2344.875,18.98821;Float;True;Property;_Texture_Color_Metal_Dark;Texture_Color_Metal_Dark;16;1;[HideInInspector];Create;True;0;0;True;0;91094db90fa56c74c8a1d41d52b2e566;91094db90fa56c74c8a1d41d52b2e566;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;182.6165,-321.0611;Float;False;Property;_Color_Secondary;Color_Secondary;1;0;Create;True;0;0;False;0;0.8196079,0.6431373,0.2980392,0;0.1647059,0.1647059,0.1647059,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;35;19.80534,-381.929;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;95;958.2158,-182.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;89;427.2157,-246.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;136;2192.782,-27.08372;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;128;1445.771,-44.48635;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;46;2836.234,29.28452;Float;True;Property;_Texture_Hair;Texture_Hair;21;1;[HideInInspector];Create;True;0;0;True;0;724afb924514bc14d9d8fcc045e8ed4d;724afb924514bc14d9d8fcc045e8ed4d;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;135;1877.781,-43.08373;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;144;2628.965,-18.49084;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;96;865.2158,-224.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;98;1381.216,-179.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;626.047,-320.1301;Float;False;Property;_Color_Leather_Primary;Color_Leather_Primary;2;0;Create;True;0;0;False;0;0.282353,0.2078432,0.1647059,0;0.282353,0.2078432,0.1647059,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;41;454.2986,-380.0913;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;126;1482.28,-144.2758;Float;False;MaskingFunction;-1;;10;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;143;2313.965,-34.49084;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;97;1293.216,-222.2679;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;45;897.7284,-379.1605;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;63;1042.961,-321.5981;Float;False;Property;_Color_Leather_Secondary;Color_Leather_Secondary;4;0;Create;True;0;0;False;0;0.372549,0.3294118,0.2784314,0;0.372549,0.3294118,0.2784314,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;130;1810.771,-175.4864;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;3311.763,0.9144592;Float;True;Property;_Texture_Skin;Texture_Skin;22;1;[HideInInspector];Create;True;0;0;True;0;9bf5625bb4be4ec4e9a0bcc009d10ab4;9bf5625bb4be4ec4e9a0bcc009d10ab4;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;133;1914.29,-142.873;Float;False;MaskingFunction;-1;;11;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;104;3130.826,-22.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;54;3759.769,0.9135399;Float;True;Property;_Texture_Stubble;Texture_Stubble;23;1;[HideInInspector];Create;True;0;0;True;0;daa5c40d542aa5b47b3583b6285941ea;daa5c40d542aa5b47b3583b6285941ea;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;141;2350.474,-132.2801;Float;False;MaskingFunction;-1;;12;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;106;3585.826,-22.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;137;2242.782,-174.0837;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;65;1314.643,-380.6285;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;103;2791.827,-31.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;131;1731.771,-218.4863;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;125;1478.22,-313.8161;Float;False;Property;_Color_Metal_Primary;Color_Metal_Primary;3;0;Create;True;0;0;False;0;0.5960785,0.6117647,0.627451,0;0.5560662,0.625,0.5995657,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;4194.632,23.17324;Float;True;Property;_Texture_Scar;Texture_Scar;24;1;[HideInInspector];Create;True;0;0;True;0;8a1bc2c7e543a3545a87c5593e1bf765;8a1bc2c7e543a3545a87c5593e1bf765;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;145;2678.965,-165.4909;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;111;4049.83,-18.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;105;3270.826,-27.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;139;1910.23,-312.4134;Float;False;Property;_Color_Metal_Secondary;Color_Metal_Secondary;6;0;Create;True;0;0;False;0;0.345098,0.3764706,0.3960785,0;0.3998181,0.4044118,0.3568339,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;124;1749.902,-372.8465;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;47;2831.833,-127.9842;Float;False;MaskingFunction;-1;;14;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;138;2163.782,-217.0837;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;102;3157.826,-162.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;51;3307.362,-124.3543;Float;False;MaskingFunction;-1;;15;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.WireNode;146;2599.965,-208.4908;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;132;2181.913,-371.4437;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;112;3723.829,-29.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;114;4474.834,-3.869209;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;147;2346.414,-303.821;Float;False;Property;_Color_Metal_Dark;Color_Metal_Dark;5;0;Create;True;0;0;False;0;0.1764706,0.1960784,0.2156863,0;0.1764706,0.1960784,0.2156863,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;101;3074.826,-222.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;140;2618.095,-362.8513;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;113;4166.832,-14.86921;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;107;3641.827,-143.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;2827.773,-297.5243;Float;False;Property;_Color_Hair;Color_Hair;7;0;Create;True;0;0;False;0;0.2627451,0.2117647,0.1333333,0;0.8970588,0.7747027,0.620026,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;55;3755.367,-124.3551;Float;False;MaskingFunction;-1;;16;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.ColorNode;52;3303.302,-293.8943;Float;False;Property;_Color_Skin;Color_Skin;8;0;Create;True;0;0;False;0;1,0.8000001,0.682353,1;1,0.8000001,0.682353,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;110;4087.83,-146.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;108;3542.826,-188.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;49;3099.455,-356.5544;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;59;4198.231,-117.0952;Float;False;MaskingFunction;-1;;18;571aab6f8c08f1c4d9bd4012d2958d88;0;3;21;FLOAT;0;False;30;FLOAT;0;False;25;FLOAT;0.5;False;3;FLOAT;0;FLOAT;32;FLOAT;28
Node;AmplifyShaderEditor.ColorNode;56;3751.307,-293.8953;Float;False;Property;_Color_Stubble;Color_Stubble;9;0;Create;True;0;0;False;0;0.8039216,0.7019608,0.6313726,1;0.9294118,0.6862745,0.5921569,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;53;3573.773,-352.9244;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;115;4535.835,-135.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;109;3988.83,-208.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;60;4194.171,-286.6357;Float;False;Property;_Color_Scar;Color_Scar;10;0;Create;True;0;0;False;0;0.9294118,0.6862745,0.5921569,1;0.9294118,0.6862745,0.5921569,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;116;4446.833,-179.8692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;151;4625.355,91.13812;Float;False;Constant;_Color0;Color 0;26;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;66;4622.293,-109.8861;Float;True;Property;_Texture_BodyArt;Texture_BodyArt;17;1;[HideInInspector];Create;True;0;0;True;0;b5c72be73bd8cad4394ec40cc22be615;b5c72be73bd8cad4394ec40cc22be615;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;148;4621.355,264.1381;Float;False;Property;_BodyArt_Amount;BodyArt_Amount;26;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;57;4021.78,-352.9254;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;68;4621.832,-280.6949;Float;False;Property;_Color_BodyArt;Color_BodyArt;11;0;Create;True;0;0;False;0;0.2283196,0.5822246,0.7573529,1;0.05071366,0.675916,0.9852941,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;152;5027.355,-104.8619;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;61;4464.645,-345.6659;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;154;5037.64,383.2585;Float;False;Property;_Smoothness;Smoothness;25;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;69;5201.305,-375.7252;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5405.913,-376.9774;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SyntyStudios/CustomCharacter;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;86;0;31;0
WireConnection;88;0;32;0
WireConnection;85;0;86;0
WireConnection;94;0;42;0
WireConnection;34;21;85;0
WireConnection;87;0;88;0
WireConnection;93;0;94;0
WireConnection;99;0;62;0
WireConnection;91;0;34;32
WireConnection;39;21;87;0
WireConnection;90;0;39;32
WireConnection;92;0;91;0
WireConnection;43;21;93;0
WireConnection;100;0;99;0
WireConnection;129;0;127;0
WireConnection;64;21;100;0
WireConnection;35;0;37;0
WireConnection;35;1;33;0
WireConnection;35;2;92;0
WireConnection;95;0;43;32
WireConnection;89;0;90;0
WireConnection;136;0;134;0
WireConnection;128;0;129;0
WireConnection;135;0;136;0
WireConnection;144;0;142;0
WireConnection;96;0;95;0
WireConnection;98;0;64;32
WireConnection;41;0;35;0
WireConnection;41;1;40;0
WireConnection;41;2;89;0
WireConnection;126;21;128;0
WireConnection;143;0;144;0
WireConnection;97;0;98;0
WireConnection;45;0;41;0
WireConnection;45;1;44;0
WireConnection;45;2;96;0
WireConnection;130;0;126;32
WireConnection;133;21;135;0
WireConnection;104;0;46;0
WireConnection;141;21;143;0
WireConnection;106;0;50;0
WireConnection;137;0;133;32
WireConnection;65;0;45;0
WireConnection;65;1;63;0
WireConnection;65;2;97;0
WireConnection;103;0;104;0
WireConnection;131;0;130;0
WireConnection;145;0;141;32
WireConnection;111;0;54;0
WireConnection;105;0;106;0
WireConnection;124;0;65;0
WireConnection;124;1;125;0
WireConnection;124;2;131;0
WireConnection;47;21;103;0
WireConnection;138;0;137;0
WireConnection;102;0;47;32
WireConnection;51;21;105;0
WireConnection;146;0;145;0
WireConnection;132;0;124;0
WireConnection;132;1;139;0
WireConnection;132;2;138;0
WireConnection;112;0;111;0
WireConnection;114;0;58;0
WireConnection;101;0;102;0
WireConnection;140;0;132;0
WireConnection;140;1;147;0
WireConnection;140;2;146;0
WireConnection;113;0;114;0
WireConnection;107;0;51;32
WireConnection;55;21;112;0
WireConnection;110;0;55;32
WireConnection;108;0;107;0
WireConnection;49;0;140;0
WireConnection;49;1;48;0
WireConnection;49;2;101;0
WireConnection;59;21;113;0
WireConnection;53;0;49;0
WireConnection;53;1;52;0
WireConnection;53;2;108;0
WireConnection;115;0;59;32
WireConnection;109;0;110;0
WireConnection;116;0;115;0
WireConnection;57;0;53;0
WireConnection;57;1;56;0
WireConnection;57;2;109;0
WireConnection;152;0;66;0
WireConnection;152;1;151;0
WireConnection;152;2;148;0
WireConnection;61;0;57;0
WireConnection;61;1;60;0
WireConnection;61;2;116;0
WireConnection;69;0;68;0
WireConnection;69;1;61;0
WireConnection;69;2;152;0
WireConnection;0;0;69;0
WireConnection;0;3;154;0
WireConnection;0;4;154;0
ASEEND*/
//CHKSM=89096D467401927AAFA9CD9CFBA7896E6487B972
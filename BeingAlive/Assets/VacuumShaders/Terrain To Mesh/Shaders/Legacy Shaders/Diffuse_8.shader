﻿Shader "VacuumShaders/Terrain To Mesh/Legacy Shaders/Diffuse/8 Textures" 
{
	Properties 
	{
		_Color("Tint Color", color) = (1, 1, 1, 1)
		_V_T2M_Control ("Control Map (RGBA)", 2D) = "black" {}

		//TTM	
		[V_T2M_Layer] _V_T2M_Splat1 ("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat1_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat2 ("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat2_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat3 ("Layer 3 (B)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat3_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat4 ("Layer 4 (A)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat4_uvScale("", float) = 1	


		 
		_V_T2M_Control2 ("Control Map 2 (RGBA)", 2D) = "black" {}

		//TTM				
		[V_T2M_Layer] _V_T2M_Splat5 ("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat5_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat6 ("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat6_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat7 ("Layer 3 (B)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat7_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat8 ("Layer 4 (A)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat8_uvScale("", float) = 1	



		//Fallback use only
		[NoScaleOffset]_MainTex ("BaseMap (Fallback use only!)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert 
		#pragma target 2.0
		#pragma exclude_renderers gles 
		 

		#define V_T2M_2_CONTROL_MAPS 
		#define V_T2M_3_TEX
		#define V_T2M_4_TEX
		#define V_T2M_5_TEX
		#define V_T2M_6_TEX
		#define V_T2M_7_TEX
		#define V_T2M_8_TEX

		#include "../cginc/T2M_Deferred.cginc"		

		ENDCG
	}  

	FallBack "Legacy Shaders/Diffuse"
}

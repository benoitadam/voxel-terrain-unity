Shader "Custom/VShader" {
	SubShader {
	    Pass {
	        Fog { Mode Off }
			CGPROGRAM
			#pragma vertex vert
			//#pragma fragment frag
			#pragma multi_compile_builtin
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			float3 hsv2rgb(float3 HSV)
	        {
				float3 RGB = HSV.z;
				float var_h = HSV.x * 6;
				float var_i = floor(var_h);
				float var_1 = HSV.z * (1.0 - HSV.y);
				float var_2 = HSV.z * (1.0 - HSV.y * (var_h-var_i));
				float var_3 = HSV.z * (1.0 - HSV.y * (1-(var_h-var_i)));
				if      (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
				else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
				else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
				else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
				else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
				else                 { RGB = float3(HSV.z, var_1, var_2); }
				return (RGB);
	        }
	        
			// appdata to vert
			struct a2v {
			    float4 vertex : POSITION;
			    float4 normal : NORMAL;
			    float4 color  : COLOR;
			};
			
			// vert to frag
			struct v2f {
			
			    float4 pos      : SV_POSITION;
			    
			    fixed4 color    : COLOR;
			    
				//LIGHTING_COORDS
				
				float3 viewDir  : TEXCOORD1;
				float3 normal   : TEXCOORD2;
				float3 lightDir : TEXCOORD3;
			    
			};
			
			// first step appdata to vert
			v2f vert (a2v v) {
			
			    v2f o;
			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
			    o.color.xyz = hsv2rgb(float3(v.color.x, 1.0, 1.0));
			    o.color.w = 1.0;
			    
			    return o;
			}
			
			//float4 frag (v2f i)  : COLOR
			//{    
			//	half4 texcol = i.color;
			//	return SpecularLightWrap( i.lightDir, i.viewDir, i.normal, texcol, 10, LIGHT_ATTENUATION(i));
			//}
			
			ENDCG
	    }

//		Tags { "RenderType" = "Opaque" }
//		
//		CGPROGRAM
//		#pragma surface surf SimpleLambert vertex:vert fragment:frag
//		
//		half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
//			half NdotL = dot (s.Normal, lightDir);
//			half4 c;
//			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
//			c.a = s.Alpha;
//			return c;
//		}
//		
//		struct Input {
//			float4 color : COLOR;
//		};
//		
//		// function hst2rgb
//		float3 hsv2rgb(float3 HSV)
//        {
//			float3 RGB = HSV.z;
//			float var_h = HSV.x * 6;
//			float var_i = floor(var_h);
//			float var_1 = HSV.z * (1.0 - HSV.y);
//			float var_2 = HSV.z * (1.0 - HSV.y * (var_h-var_i));
//			float var_3 = HSV.z * (1.0 - HSV.y * (1-(var_h-var_i)));
//			if      (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
//			else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
//			else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
//			else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
//			else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
//			else                 { RGB = float3(HSV.z, var_1, var_2); }
//			return (RGB);
//        }
//		
//		void vert (inout appdata_full v) {
//			//v.color.xyz = float3(0.5,0.5,0.5);
//			v.color.xyz = hsv2rgb(float3(v.color.x, 1.0, 1.0));
//		}
//		
//		// pixel render
//		half4 frag (v2f i) : COLOR
//        {
//        	half3 texcol = i.color.xyz;
//			return float4(texcol, 1.0);
//        }
//		
//		void surf (Input IN, inout SurfaceOutput o) {
//			o.Albedo = IN.color;
//		}
//		
//		ENDCG
		
	}
	Fallback "Diffuse"
}

//
//
//	Properties {
//	
//		_Top1Tex ("Top1", 2D) = "white" {}
//		_Top2Tex ("Top2", 2D) = "white" {}
//		_Top3Tex ("Top3", 2D) = "white" {}
//		
//		_Side1Tex ("Side1", 2D) = "white" {}
//		_Side2Tex ("Side2", 2D) = "white" {}
//		_Side3Tex ("Side3", 2D) = "white" {}
//		
//		_BumpMap ("Bump (RGB)", 2D) = "white" {}
//		
//	}
//	SubShader {
//		Tags { "RenderType" = "Opaque" }
//		//Fog { Color [_AddFog] }
//		Pass {
//	    	Tags { "LightMode" = "Always" }
//	    	
//	        //Fog { Mode Off }
//			CGPROGRAM
//			#pragma vertex vert
//        	#pragma fragment frag
//			
//			#include "UnityCG.cginc"
//			
//			// vertex of mesh input
//			struct appdata {
//			    float4 vertex    : POSITION;	// position
//			    float4 color     : COLOR;		// color
//			    float4 normal    : NORMAL;		// normal
//			    float4 texcoord  : TEXCOORD0;	// uv
//			    float4 texcoord1 : TEXCOORD1;	// uv1
//			};
//			
//			// vertex out
//			struct v2f {
//			    float4 pos   : SV_POSITION;		// position
//			    fixed4 color : COLOR;			// color
//			    float4 uv[3] : TEXCOORD0;
//			};
//			
//			// textures
//			uniform sampler2D _Top1Tex,  _Top2Tex,  _Top3Tex;
//			uniform sampler2D _Side1Tex, _Side2Tex, _Side3Tex;
//			
//			uniform float4 _Top1Tex_ST,  _Top2Tex_ST,  _Top3Tex_ST;
//			uniform float4 _Side1Tex_ST, _Side2Tex_ST, _Side3Tex_ST;
//			
//			// vertex in to vertex out
//			v2f vert (appdata v) {
//				// vertex out object
//			    v2f o; 
//			    // position
//			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//			    
//			    //float4(v.texcoord.xy, 0, 0)+0.5F;
//			    float x = abs(v.normal.x);
//			    float y = abs(v.normal.y);
//			    float z = abs(v.normal.z);
//			    float t = x+y+z;
//			    
//			    float2 planX = float2(v.texcoord.y,v.texcoord1.y);
//			    float2 planY = float2(v.texcoord.xy);
//			    float2 planZ = float2(v.texcoord1.xy);
//			    
//			    o.color = v.color;
//			    
//			    // TRANSFORM_TEX <- Tiling and Offset
//			    o.uv[0].xy = TRANSFORM_TEX(planX, _Top1Tex);
//			    o.uv[1].xy = TRANSFORM_TEX(planY, _Side1Tex);
//			    o.uv[2].xy = TRANSFORM_TEX(planZ, _Side1Tex);
//			    
//			    o.uv[0].z = x/t;
//			    o.uv[1].z = y/t;
//			    o.uv[2].z = z/t;
//			    
//			    return o;
//			}
//			
//			uniform float4 _Color;
//			
//			// pixel render
//			half4 frag (v2f i) : COLOR
//	        {
//	        
//	        	half3 texcol  = i.color.r * tex2D(_Top1Tex, i.uv[1].xy).rgb * i.uv[1].z; // top1
//	        	      texcol += i.color.g * tex2D(_Top1Tex, i.uv[1].xy).rgb * i.uv[1].z; // top2
//	        	      texcol += i.color.b * tex2D(_Top1Tex, i.uv[1].xy).rgb * i.uv[1].z; // top3
//	        	
//	        	      texcol += i.color.r * tex2D(_Side1Tex, i.uv[0].xy).rgb * i.uv[0].z; // side1
//	        	      texcol += i.color.g * tex2D(_Side2Tex, i.uv[0].xy).rgb * i.uv[0].z; // side1
//	        	      texcol += i.color.b * tex2D(_Side3Tex, i.uv[0].xy).rgb * i.uv[0].z; // side1
//	        	
//	        	      texcol += i.color.r * tex2D(_Side1Tex, i.uv[2].xy).rgb * i.uv[2].z; // side1
//	        	      texcol += i.color.g * tex2D(_Side2Tex, i.uv[2].xy).rgb * i.uv[2].z; // side1
//	        	      texcol += i.color.b * tex2D(_Side3Tex, i.uv[2].xy).rgb * i.uv[2].z; // side1
//
//				return float4(texcol, 1.0);
//				
//	        }
//			ENDCG
//	    }
//	}
//	Fallback "VertexLit"
//}



//	SubShader {
//	    Pass {
//	        Fog { Mode Off }
//			CGPROGRAM
//			#pragma vertex vert
//			
//			// vertex input: position, normal
//			struct appdata {
//			    float4 vertex : POSITION;
//			    float3 normal : NORMAL;
//			};
//			
//			struct v2f {
//			    float4 pos : SV_POSITION;
//			    fixed4 color : COLOR;
//			};
//			v2f vert (appdata v) {
//			    v2f o;
//			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//			    o.color.xyz = v.normal * 0.5 + 0.5;
//			    o.color.w = 1.0;
//			    return o;
//			}
//			ENDCG
//	    }
//	}
//}


//	Properties {
//		_T1Tex ("Texture1", 2D) = "white" {}
//		_T2Tex ("Texture2", 2D) = "white" {}
//		_T3Tex ("Texture3", 2D) = "white" {}
//		_BumpMap ("Bumpmap", 2D) = "bump" {}
//	}
//	SubShader {
//		Tags { "RenderType" = "Opaque" }
//			CGPROGRAM
//			#pragma surface surf Lambert
//			struct Input {
//				float2 uv_MainTex;
//				float2 uv2_MainTex;
//				float2 uv_BumpMap;
//			};
//			sampler2D _T1Tex;
//			sampler2D _T2Tex;
//			sampler2D _T3Tex;
//			sampler2D _BumpMap;
//			void surf (Input IN, inout SurfaceOutput o) {
//				o.Albedo = tex2D (_T1Tex, IN.uv_MainTex).rgb;
//				//o.Albedo *= tex2D (_T2Tex, IN.uv_MainTex).rgb * 2;
//				o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
//			}
//			ENDCG
//	} 
//	Fallback "Diffuse"
//}


//	Properties {
//		_ColorTop ("Color Top", Color) = (1,0,0,0.5)
//		_ColorSide ("Color Side", Color) = (0,1,0,0.5)
//		_ColorBottom ("Color Bottom", Color) = (0,0,1,0.5)
//	}
//	SubShader {
//	    Pass {
//	        Fog { Mode Off }
//			CGPROGRAM
//			#pragma vertex vert
//			
//			// vertex input: position, color, normal
//			struct appdata {
//			    float4 vertex : POSITION;
//			    float4 color : COLOR;
//			    float4 normal : NORMAL;
//			    float4 texcoord : TEXCOORD1;
//			};
//			
//			
//			struct v2f {
//			    float4 pos : SV_POSITION;
//			    fixed4 color : COLOR;
//			};
//			
//			v2f vert (appdata v) {
//			    v2f o;
//			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//			    o.color = float4(v.texcoord.xy, 0, 0)+0.5F; //(v.normal*0.5+0.5 + v.color)/2;
//			    return o;
//			}
//			
//			ENDCG
//	    }
//	}
//}



//SubShader {
//    Pass {
//        Fog { Mode Off }
//CGPROGRAM
//#pragma vertex vert
//#pragma fragment frag
//
//// vertex input: position, UV
//struct appdata {
//    float4 vertex : POSITION;
//    float4 texcoord : TEXCOORD0;
//};
//
//struct v2f {
//    float4 pos : SV_POSITION;
//    float4 uv : TEXCOORD0;
//    float4 uv1 : TEXCOORD1;
//    float4 uv2 : TEXCOORD2;
//};
//v2f vert (appdata v) {
//    v2f o;
//    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//    o.uv = float4( v.texcoord.xy, 0, 0 );
//    return o;
//}
//half4 frag( v2f i ) : COLOR {
//    half4 c = frac( i.uv );
//    if (any(saturate(i.uv) - i.uv))
//        c.b = 0.5;
//    return c;
//}
//ENDCG
//    }
//}
//}

//


//Shader "Custom/VShader" {
//
//	Properties {
//	    _ColorTop ("Color Top", Color) = (1,0,0,0.5)
//	    _ColorSide ("Color Side", Color) = (0,1,0,0.5)
//	    _ColorBottom ("Color Bottom", Color) = (0,0,1,0.5)
//	}
//
//	// subshader for graphics hardware A
//	SubShader { 
//	
//	    Pass {
//	    
//	    	// ... the usual pass state setup ...
//	
//			CGPROGRAM
//			// compilation directives for this snippet, e.g.:
//			#pragma vertex vert
//			#pragma fragment frag
//			
//			#include "UnityCG.cginc"
//			
//			float4 _ColorTop;
//			float4 _ColorSide;
//			float4 _ColorBottom;
//			
//			// vertex input: position, color
//			struct appdata {
//			    float4 vertex : POSITION;
//			    float4 color : COLOR;
//			};
//			
//			struct v2f {
//			    float4 pos : SV_POSITION;
//			    float4 color : COLOR;
//			};
//			
//			v2f vert (appdata_base v)
//			{
//			    v2f o;
//			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
//			    o.color = v.color;//v.normal*0.5+0.5;//v.normal * 0.5 + 0.5;
//			    return o;
//			}
//			
////			half4 frag (v2f i) : COLOR
////			{
////			    return half4 (i.color, 1);
////			}
//			
//			ENDCG
//			// ... the rest of pass setup ...
//	
//	    }
//	}
//	
//	// Optional fallback
//	Fallback "VertexLit"
//	
//}
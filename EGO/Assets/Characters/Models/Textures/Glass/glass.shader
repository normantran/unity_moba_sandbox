// Glass with fresnel reflection & transparency...
// Tested on: ATI Radeon Mobility

SHADER "FX/Glass" { 
	Properties {
		_Ambient ("Ambient Color", COLOR) = (.9,0,0,0)
		_Color ("Base color", COLOR)  = ( 0 , .1 , 0 , .1)
		_Specular ("Specular color", COLOR) = (.9,.9,1,1)
		_Shininess ("Shininess", Range (0, 128)) = 64
		_FresnelTransp ("Edge Transp.", Range (0, 1)) = .5
		_ReflectionMap ("Environment Reflection", cube) = "spheremap.jpg" {
			Cube
			texgen CubeReflect
		}
		_FresnelControl ("Fresnel control map", 2D) = "fresnel.png" { 
			TexGen SphereMap
		}
	}
	SUBSHADER { 
//		Cull Off
		Tags { "Queue" = "Transparent" }
		ZWrite off
		Lighting On
		Blend SrcAlpha OneMinusSrcAlpha
		// Pass 1 main effect...
		Pass {
			Material {
				Diffuse [_Color] Ambient [_Ambient] Specular [_Specular] 
				Shininess [_Shininess]
			}
			// Get fresnel value into alpha. This is used in next stage.
			SetTexture [_FresnelControl] { combine texture, texture }
			// This is the biggie: It lerps the reflection map in at the edges (in color).
			// And lerps the alpha value between the center and the edge.
			SetTexture [_ReflectionMap] {
				ConstantColor(0,0,0,[_FresnelTransp])					// Blend edge to _fresnelTransp Alpha value/
				Matrix [_Reflection]

				combine 
					texture lerp (previous) primary,		// Color
					constant lerp (previous) primary	// Alpha
			}
		}
		 
		// Pass 2 Specular
	        Pass {
			Cull back
			Material {
				Specular [_Specular] 
				Shininess [_Shininess]
			}
			Blend OneMinusDstColor One	
   		}
	}
}
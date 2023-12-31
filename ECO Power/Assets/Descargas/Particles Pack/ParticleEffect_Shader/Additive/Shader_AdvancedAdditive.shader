﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ParticleEffect_Shader/(Shader) Advanced Additive"{
	Properties{
		[HDR]_TintColor("For Red Color",Color) = (1,1,1,1)
		_TintColorFactor("Tint Color Factor",float) = 1.0
		[HDR]_TintColor2("For Green Color",Color) = (1,1,1,1)
		_TintColorFactor2("Tint Color2 Factor",float) = 1.0
		[HDR]_TintColor3("For Blue Color",Color) = (1,1,1,1)
		_TintColorFactor3("Tint Color3 Factor",float) = 1.0
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particle Factor", Range(0.01,3.0)) = 1.0
	}
		Category{
				Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
				Blend SrcAlpha One
				ColorMask RGB
				Cull Off
				Lighting Off
				ZWrite Off

				SubShader {
					Pass{
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						#pragma target 3.0
						#pragma multi_compile_particles
						#pragma multi_compile_fog

						#include "UnityCG.cginc"

						sampler2D _MainTex;

						fixed4 _TintColor;
						fixed4 _TintColor2;
						fixed4 _TintColor3;

						float _TintColorFactor;
						float _TintColorFactor2;
						float _TintColorFactor3;

						struct appdata_t {
							float4 vertex : POSITION;
							fixed4 color : COLOR;
							float2 texcoord : TEXCOORD0;
						};

						struct v2f {
							float4 vertex : SV_POSITION;
							fixed4 color : COLOR;
							float2 texcoord : TEXCOORD0;
							UNITY_FOG_COORDS(1)
							#ifdef SOFTPARTICLES_ON
								float4 projPos : TEXCOORD2;
							#endif
						};

						float4 _MainTex_ST;

						v2f vert(appdata_t v)
						{
							v2f o;
							o.vertex = UnityObjectToClipPos(v.vertex);
							
							#ifdef SOFTPARTICLES_ON
								o.projPos = ComputeScreenPos(o.vertex);
								COMPUTE_EYEDEPTH(o.projPos.z);
							#endif
							o.color = v.color;
							o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
							UNITY_TRANSFER_FOG(o, o.vertex);
							return o;
						}
						
						sampler2D _CameraDepthTexture;
						float _InvFade;

						fixed4 frag(v2f i ) : SV_Target
						{
                           #ifdef SOFTPARTICLES_ON
                                float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.projPos))));
                                float partZ = i.projPos.z;
                                float fade = saturate(_InvFade * (sceneZ - partZ));
                                i.color.a *= fade;
                            #endif

							fixed4 tex = tex2D(_MainTex,i.texcoord);
							fixed4 color = fixed4(0, 0, 0, 1);
							
							color += tex.r *_TintColor *_TintColorFactor *0.75f;
							color += tex.g *_TintColor2 * _TintColorFactor2 *0.75f;
							color += tex.b *_TintColor3 * _TintColorFactor3 *0.75f;
							
							//if(tex.a == 1)
								color.a = tex.a;
							
							fixed4 res = i.color * color;
							UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(0, 0, 0, 0));
							return res;
						}
						ENDCG
				}
			}
	}
}
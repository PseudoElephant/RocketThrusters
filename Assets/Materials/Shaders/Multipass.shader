Shader "PseudoElephant/Blur"
{
    Properties
    {
        
        _Sigma("Sigma",float) = 11
        _MainTex("Diffuse", 2D) = "white" {}
        _RenderTexture("Render Texture",2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}

        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    ENDHLSL

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex CombinedShapeLightVertex
            #pragma fragment CombinedShapeLightFragment
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color        : COLOR;
                float2  uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4  positionCS  : SV_POSITION;
                float4  color       : COLOR;
                float2	uv          : TEXCOORD0;
                float2	lightingUV  : TEXCOORD1;
           
            };

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            TEXTURE2D(_RenderTexture);
            SAMPLER(sampler_RenderTexture);
            
            half4 _MainTex_ST;
            half4 _NormalMap_ST;
            half4 _MainTex_TexelSize;

            half4 _RenderTexture_ST;
            half4 _RenderTexture_TexelSize;
            #if USE_SHAPE_LIGHT_TYPE_0
            SHAPE_LIGHT(0)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_1
            SHAPE_LIGHT(1)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_2
            SHAPE_LIGHT(2)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_3
            SHAPE_LIGHT(3)
            #endif

            float normpdf(in float x, in float sigma)
            {
	            return 0.39894*exp(-0.5*x*x/(sigma*sigma))/sigma;
            }

            CBUFFER_START(UnityMaterial)
            float _Sigma;
            CBUFFER_END
            
            Varyings CombinedShapeLightVertex(Attributes v)
            {
                Varyings o = (Varyings)0;

                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = TRANSFORM_TEX(v.uv, _RenderTexture);
                float4 clipVertex = o.positionCS / o.positionCS.w;
                o.lightingUV = ComputeScreenPos(clipVertex).xy;
                o.color = v.color;
                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            half4 CombinedShapeLightFragment(Varyings i) : SV_Target
            {
                float3 c = _RenderTexture.Sample(sampler_RenderTexture, i.uv / _RenderTexture_TexelSize.zw);
                //declare stuff
		        const int mSize = 11;
		        const int kSize = (mSize-1)/2;
		        float kernel[mSize];
		        float3 final_colour = float3(0.0f,0.0f,0.0f);
		        
		        //create the 1-D kernel
		        float sigma = _Sigma;
		        float Z = 0.0;
		        for (int r = 0; r <= kSize; ++r)
		        {
			        kernel[kSize+r] = kernel[kSize-r] = normpdf(float(r), sigma);
		        }
		        
		        //get the normalization factor (as the gaussian has been clamped)
		        for (int j = 0; j < mSize; ++j)
		        {
			        Z += kernel[j];
		        }
		        
		        //read out the texels
		        for (int s=-kSize; s <= kSize; ++s)
		        {
			        for (int t=-kSize; t <= kSize; ++t)
			        {
				        final_colour += kernel[kSize+t]*kernel[kSize+s]*_RenderTexture.Sample(sampler_RenderTexture, (i.uv+(float2(float(s),float(t)) / _RenderTexture_TexelSize.zw))).rgb;
	        
			        }
		        }
		        float4 color = float4(final_colour/(Z*Z), 1.0) * i.color;
                half4 main = color;
                half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
                return CombinedShapeLightShared(main, mask, i.lightingUV);
            }
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "NormalsRendering"}
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex NormalsRenderingVertex
            #pragma fragment NormalsRenderingFragment

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color		: COLOR;
                float2 uv			: TEXCOORD0;
                float4 tangent      : TANGENT;
            };

            struct Varyings
            {
                float4  positionCS		: SV_POSITION;
                float4  color			: COLOR;
                float2	uv				: TEXCOORD0;
                float3  normalWS		: TEXCOORD1;
                float3  tangentWS		: TEXCOORD2;
                float3  bitangentWS		: TEXCOORD3;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            float4 _NormalMap_ST;  // Is this the right way to do this?

            Varyings NormalsRenderingVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                o.uv = TRANSFORM_TEX(attributes.uv, _NormalMap);
                o.uv = attributes.uv;
                o.color = attributes.color;
                o.normalWS = TransformObjectToWorldDir(float3(0, 0, -1));
                o.tangentWS = TransformObjectToWorldDir(attributes.tangent.xyz);
                o.bitangentWS = cross(o.normalWS, o.tangentWS) * attributes.tangent.w;
                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

            float4 NormalsRenderingFragment(Varyings i) : SV_Target
            {
                float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));
                return NormalsRenderingShared(mainTex, normalTS, i.tangentWS.xyz*2, i.bitangentWS.xyz, i.normalWS.xyz);
            }
            ENDHLSL
        }
        Pass
        {
            Tags { "LightMode" = "UniversalForward" "Queue"="Transparent" "RenderType"="Transparent"}

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex UnlitVertex
            #pragma fragment UnlitFragment

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color		: COLOR;
                float2 uv			: TEXCOORD0;
            };

            struct Varyings
            {
                float4  positionCS		: SV_POSITION;
                float4  color			: COLOR;
                float2	uv				: TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            Varyings UnlitVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
                o.uv = attributes.uv;
                o.color = attributes.color;
                return o;
            }

            float4 UnlitFragment(Varyings i) : SV_Target
            {
                float4 mainTex = i.color * 0.0f* SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return mainTex;
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}

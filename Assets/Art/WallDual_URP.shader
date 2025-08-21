Shader "TimeFragments/WallDual_URP"
{
    Properties
    {
        _MainTex     ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _BaseColor   ("Base Tint", Color) = (1,1,1,1)

        _GlowTex     ("Glow (RGB) Alpha (A)", 2D) = "black" {}
        _GlowColor   ("Glow Color", Color) = (0,1,0.86,1)   // cyan-ish
        _GlowIntensity("Glow Intensity", Range(0,5)) = 1.2
        _GlowPulse   ("Glow Pulse Speed", Range(0,5)) = 0.0 // 0 = no pulse
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);   SAMPLER(sampler_MainTex);
            TEXTURE2D(_GlowTex);   SAMPLER(sampler_GlowTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _GlowTex_ST;
                float4 _BaseColor;
                float4 _GlowColor;
                float  _GlowIntensity;
                float  _GlowPulse;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;       // sprite tint
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv0 = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv, _GlowTex);
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // base sprite (with tint)
                half4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv0) * _BaseColor * i.color;

                // glow sprite (additive contribution)
                half4 glowTex = SAMPLE_TEXTURE2D(_GlowTex, sampler_GlowTex, i.uv1);
                // optional gentle pulse
                half pulse = (_GlowPulse > 0.0) ? (0.5h + 0.5h * sin(_Time.y * _GlowPulse * 6.283185h)) : 1.0h;
                half glowAmt = _GlowIntensity * pulse;

                // convert glow to additive energy but keep base alpha for sorting
                half3 glowRGB = glowTex.rgb * _GlowColor.rgb * glowAmt;

                half4 outCol;
                outCol.rgb = baseCol.rgb + glowRGB; // additive glow
                outCol.a   = baseCol.a;             // keep sprite alpha for blending
                return outCol;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}

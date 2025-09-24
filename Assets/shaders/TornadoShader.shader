Shader "Custom/URP_TornadoWave"
{
    Properties
    {
        _BaseMap("Sprite Texture", 2D) = "white" {}
        _BaseColor("Tint", Color) = (1,1,1,1)
        _Amplitude("Wave Amplitude", Float) = 0.05
        _Frequency("Wave Frequency", Float) = 10
        _Speed("Wave Speed", Float) = 2
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalRenderPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float4 _BaseColor;

            float _Amplitude;
            float _Frequency;
            float _Speed;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // wave sólo en X y más fuerte en la parte superior (según Y)
                float wave = sin(uv.y * _Frequency + _Time.y * _Speed) * _Amplitude * uv.y;
                uv.x += wave;

                half4 c = tex2D(_BaseMap, uv) * _BaseColor;
                return c;
            }
            ENDHLSL
        }
    }
}

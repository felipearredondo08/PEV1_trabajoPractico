Shader "Custom/DitheringFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Dither Threshold", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Cutoff; // Controla el dithering

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Matriz de Dithering 4x4
            static const float ditherMatrix[16] = {
                0.0, 0.5, 0.125, 0.625,
                0.75, 0.25, 0.875, 0.375,
                0.1875, 0.6875, 0.0625, 0.5625,
                0.9375, 0.4375, 0.8125, 0.3125
            };

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Obtener coordenadas de píxel
                int x = fmod(i.vertex.x, 4);
                int y = fmod(i.vertex.y, 4);
                int index = y * 4 + x;

                // Comparar con el threshold
                if (col.a < ditherMatrix[index] * _Cutoff)
                    discard;

                return col;
            }
            ENDCG
        }
    }
}
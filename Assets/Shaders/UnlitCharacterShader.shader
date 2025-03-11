Shader "Custom/DamageHighlight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DamageIntensity ("Damage Intensity", Range(0, 1)) = 0
        _DamageColor ("Damage Color", Color) = (1, 0, 0, 1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "DamageHighlight.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DamageIntensity;
            float4 _DamageColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Apply vertex color
                col *= i.color;
                
                // Use the new function to blend the damage color
                col = PlayDamageHilight(col, _DamageColor, _DamageIntensity);
                
                return col;
            }
            ENDCG
        }
    }
}
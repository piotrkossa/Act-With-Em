Shader "Custom/TransparencyBasedOnDistance"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _TransparencyDistance ("Transparency Distance", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : POSITION;
                float3 worldPos : TEXCOORD0;
            };
            
            float _TransparencyDistance;
            sampler2D _MainTex;
            
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            half4 frag(v2f i) : COLOR
            {
                float distanceToCamera = length(_WorldSpaceCameraPos - i.worldPos);
                float transparency = saturate(distanceToCamera / _TransparencyDistance);
                
                half4 texColor = tex2D(_MainTex, i.worldPos.xy);
                texColor.a *= 1 - transparency;
                return texColor;
            }
            ENDCG
        }
    }
}

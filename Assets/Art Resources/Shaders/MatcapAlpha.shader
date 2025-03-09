Shader "Custom/MatcapAlpha"
{
    Properties
    {
        [Header(RenderMode)]
        [Space(20)]

        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("Src Blend",int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("Dst Blend",int) = 1
        [Enum(UnityEngine.Rendering.CullMode)]_Cull("Cull",int) = 2
        // [Enum(on,1,off,0)]_ZWriteSwitch("ZWriteSwitch",int) = 1

        [Space(20)]
        [Header(Base)]
        [Space(20)]

        _MatCap ("MatCap", 2D) = "white" {}
        _Color ("Color",Color) = (1,1,1,1)
        _Intensity ("Intensity",Range(0,1)) = 1
       
    }
    SubShader
    {
        Tags{"Queue"="Transparent"}
        Blend [_SrcBlend] [_DstBlend]
        Cull [_Cull]
        
        Pass
        {
            ZWrite On
            ColorMask 0
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"



            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            sampler2D _MatCap;
            fixed4 _Color;
            half _Intensity;

            

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 viewNorm = mul(UNITY_MATRIX_IT_MV,v.normal);

                o.uv = viewNorm.xy * 0.5 + 0.5;
                
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c;
                c = tex2D(_MatCap, i.uv);
                c *= _Color * _Intensity;

                return c;
            }
            ENDCG
        }
    }
}

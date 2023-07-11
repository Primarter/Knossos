Shader "CustomRenderTexture/DappleLightShader"
{
    Properties
    {
        _Texture1("Texture1", 2D) = "white" {}
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Texture2("Texture2", 2D) = "white" {}
        _Color2 ("Color2", Color) = (1,1,1,1)
     }

     SubShader
     {
        Blend One Zero

        Pass
        {
            Name "DappleLightShader"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            sampler2D   _Texture1;
            sampler2D   _Texture2;
            float4      _Color1;
            float4      _Color2;
            float4      _Texture1_ST;
            float4      _Texture2_ST;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float2 uv1 = IN.globalTexcoord.xy * _Texture1_ST.xy + _Texture1_ST.zw;
                float2 uv2 = IN.globalTexcoord.xy * _Texture2_ST.xy + _Texture2_ST.zw;
                float4 color1 = tex2D(_Texture1, uv1) * _Color1;
                float4 color2 = tex2D(_Texture2, uv2) * _Color2;
                float4 color = color1 + color2;

                return color;
            }
            ENDCG
        }
    }
}

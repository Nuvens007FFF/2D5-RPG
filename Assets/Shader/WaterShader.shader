Shader "Custom/WaterShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Speed ("Speed", Range (0, 10)) = 1
        _WaveHeight ("Wave Height", Range (0, 1)) = 0.1
    }

    CGINCLUDE
    #include "UnityCG.cginc"
    ENDCG

    SubShader {
        Tags {"Queue" = "Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed _Speed;
        fixed _WaveHeight;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed2 waves = tex2D (_MainTex, IN.uv_MainTex).rg;
            o.Albedo = waves.r;
            o.Alpha = waves.g;
        }
        ENDCG
    }

    SubShader {
        Tags {"Queue" = "Overlay" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            ENDCG
        }
    }
}
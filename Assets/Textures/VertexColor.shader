// From http://wiki.unity3d.com/index.php?title=VertexColor

Shader "BoxLand/Vertex Color Diffuse" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader {
        Pass {
            Material {
                Shininess [_Shininess]
                Specular [_SpecColor]
                Emission [_Emission]    
            }
            ColorMaterial AmbientAndDiffuse
            Lighting On
            SetTexture [_MainTex] {
                Combine texture * primary, texture * primary
            }
            SetTexture [_MainTex] {
                constantColor [_Color]
                Combine previous * constant DOUBLE, previous * constant
            } 
        }
    }
    Fallback "VertexLit", 1
}
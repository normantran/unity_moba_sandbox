Shader "Unlit Transparent Vertex Colored" {
Properties {
_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

Category {
Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
ZWrite Off
Alphatest Greater 0
Blend SrcAlpha OneMinusSrcAlpha 
SubShader {
Pass {
BindChannels {
Bind "Vertex", vertex
Bind "texcoord", texcoord 
Bind "Color", color 
}

Fog { Mode Off }
Lighting Off
SetTexture [_MainTex] {
Combine texture * primary, texture * primary
}
}
} 
}
}
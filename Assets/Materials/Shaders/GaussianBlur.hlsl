#ifndef GUASSIANBLUR_INCLUDED
#define GUASSIANBLUR_INCLUDED

void GaussianBlur_float(Texture2D tex, float2 TextureUV, SamplerState samp, float2 texelSize, float3 weight, float2 dir, out float4 FragmentColor)
{
    FragmentColor = tex.Sample(samp, TextureUV) * weight[0];
    for (int r=1; r<3; r++) {
        FragmentColor += tex.Sample(samp, TextureUV + (1/texelSize)*r*dir) * weight[r];
        FragmentColor += tex.Sample(samp, TextureUV - (1/texelSize)*r*dir) * weight[r];
    }

    

    // for (int s=1; s<3; s++) {
    //     FragmentColor += tex.Sample(samp, (float2)TextureUV[s, 0]) * weight[s];
    //     FragmentColor += tex.Sample(samp, (float2)TextureUV[-s, 0]) * weight[s];
    // }
}
 
#endif
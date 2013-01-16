float2 halfPixel;

texture AlbedoMap;
texture LightMap;

sampler albedoSampler = sampler_state
{
	texture = (AlbedoMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
};

sampler lightSampler = sampler_state
{
	texture = (LightMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
};

struct VertexShaderInput
{
    float3 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = float4(input.Position, 1.0f);
	output.TexCoord = input.TexCoord - halfPixel;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float3 diffuseColor = tex2D(albedoSampler, input.TexCoord).rgb;
	float4 light = tex2D(lightSampler, input.TexCoord);

	float3 diffuseLight = light.rgb;
	float specularLight = light.a;

	return float4((diffuseColor * diffuseLight + specularLight), 1);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}


float percent;

sampler textureSample: register(s0);
float4 pixelShader(float2 Tex: TEXCOORD0) : COLOR0
{
	float4 Color = tex2D(textureSample, Tex);
	float g = Color.g;
	float b = Color.b;
	g = g-percent;
	b = b-percent;
	Color.g = g;
	Color.b = b;
	return Color;
}

technique Damage
{
	pass pass1
	{
		PixelShader = compile ps_2_0 pixelShader();
	}
}
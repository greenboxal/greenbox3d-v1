
void main()
{
	gl_FragColor = vec4(texture(uTexture, gTexCoord).rgb, uAlpha);
}

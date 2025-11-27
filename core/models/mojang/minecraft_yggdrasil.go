package mojang

type MinecraftYggdrasilAuthorization struct{
	IdentityToken string `json:"identityToken"`
}

type MinecraftYggdrasilAuthorizationResult struct{
	Username string `json:"username"`
	Roles []interface{} `json:"roles"`
	AccessToken string `json:"access_token"`
	TokenType string `json:"token_type"`
	ExpiresIn uint32 `json:"expires_in"`
}

package xboxlive

type XboxLiveProperties struct{
	AuthMethod string
	SiteName string
	RpsTicket string
}

type XboxLiveAuthorization struct{
	Properties XboxLiveProperties
	RelyingParty string
	TokenType string
}

type XboxLiveAuthorizationResult struct{
	IssueInstant string
	NotAfter string
	Token string
	DisplayClaims XboxLiveDisplayClaims
}

type XboxLiveDisplayClaims struct{
	Xui []XboxUserHash `json:"xui"`
}

type XboxUserHash struct{
	UserHash string `json:"uhs"`
}

type XSTSProperties struct{
	SandboxId string
	UserTokens []string
}

type XSTSAuthorization struct{
	Properties XSTSProperties
	RelyingParty string
	TokenType string
}

